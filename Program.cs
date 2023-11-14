using Todo.Data.Repositories;
using Gcom.Logger.AspNet;
using Gcom.Logger.Microsoft.Extenssions.DependencyInjection;
using Gcom.Logger.MongoDb;
using Todo.Data;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configurationRepository = builder.Configuration.GetSection("GcomLogger").GetSection("Repository").GetSection("Config").Get<LoggerRepositoryConfig>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGcomLogger();

builder.Services.AddTransient<TaskRepository>();

builder.Services.ConfigureLoggerDependencyInjection()
    .SetRepositoryConfig<AspNetLog>(configurationRepository);

builder.Services.AddDbContext<TodoDBContext>(o => o.UseSqlServer("Server=localhost,1433;Database=todo_db;User ID=sa;Password=1q2w3e4r@#$;Trusted_Connection=False; TrustServerCertificate=True;"));

var app = builder.Build();

app.UseGcomLogger("todo-api-" + app.Environment.EnvironmentName.ToLower());
app.LogUnhandledErrors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
