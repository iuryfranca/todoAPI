using System.Linq.Expressions;
using System.Linq;
namespace Todo.Data.Repositories;

public class TaskRepository
{
  // private List<Models.Task> Tasks = new();
  private readonly TodoDBContext _todoDBContext;

  public TaskRepository(TodoDBContext todoDBContext)
  {
    _todoDBContext = todoDBContext;
  }

  public List<Models.Task> GetList()
  {
    return _todoDBContext.Tasks.ToList();
  }

  public Models.Task? GetById(int id)
  {
    return _todoDBContext.Tasks.FirstOrDefault((task) => task.Id == id);
  }

  public void Create(Models.Task task)
  {
    task.CreatedAt = DateTime.UtcNow;
    task.UpdatedAt = DateTime.UtcNow;

    _todoDBContext.Tasks.Add(task);
    _todoDBContext.SaveChanges();
  }

  public List<Models.Task> Query(Expression<Func<Models.Task, bool>> expression)
  {
    return _todoDBContext.Tasks.AsQueryable().Where(expression).ToList();
  }

  public void Update(Models.Task task)
  {
    var taskUpdate = _todoDBContext.Tasks.FirstOrDefault((t) => t.Id == task.Id)!;

    if (taskUpdate is null) throw new Exception("Tarefa não encontrada.");

    taskUpdate.UpdatedAt = DateTime.UtcNow;

    taskUpdate.Name = task.Name;
    taskUpdate.Description = task.Description;
    taskUpdate.StartTime = task.StartTime;
    taskUpdate.FinishTime = task.FinishTime;
    taskUpdate.Active = task.Active;

    _todoDBContext.Update(taskUpdate);
    _todoDBContext.SaveChanges();
  }

  public void Delete(int id)
  {
    var task = _todoDBContext.Tasks.FirstOrDefault((t) => t.Id == id)!;

    if (task is null) throw new Exception("Tarefa não encontrada.");
    _todoDBContext.Tasks.Remove(task);
    _todoDBContext.SaveChanges();
  }
}