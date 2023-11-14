namespace Todo.ViewModels;

public class TaskViewModel
{
  public int Id { get; set; }
  public string Name { get; set; }
  public string Description { get; set; }
  public DateTime StartTime { get; set; }
  public DateTime FinishTime { get; set; }
  public bool Active { get; set; }
}