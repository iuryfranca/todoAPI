using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Todo.Data.Models;
public class Task
{
  [Key]
  public int Id { get; set; }

  [Column("name")]
  [MaxLength(20)]
  public string Name { get; set; }

  [Column("description")]
  [MaxLength(256)]
  public string Description { get; set; }

  public DateTime StartTime { get; set; }

  public DateTime FinishTime { get; set; }

  public DateTime CreatedAt { get; set; }

  public DateTime UpdatedAt { get; set; }

  public bool Active { get; set; }
}