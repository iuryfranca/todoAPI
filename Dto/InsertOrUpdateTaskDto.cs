namespace Todo.Dtos;
public class InsertOrUpdateTaskDto
{
  public string Name { get; set; }
  public string Description { get; set; }
  public DateTime StartTime { get; set; }
  public DateTime FinishTime { get; set; }
}