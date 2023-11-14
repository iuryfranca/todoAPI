namespace Todo.Data;

using Microsoft.EntityFrameworkCore;
using Todo.Controllers;
using Todo.Data.Models;

public class TodoDBContext : DbContext
{
  public TodoDBContext(DbContextOptions<TodoDBContext> options) : base(options)
  {

  }
  public DbSet<Task> Tasks { get; set; }
}