namespace Todo.Validations;

using FluentValidation;
using Todo.Dtos;

public class InsertOrUpdateTaskDtoValidator : AbstractValidator<InsertOrUpdateTaskDto>
{
  public InsertOrUpdateTaskDtoValidator()
  {
    RuleFor(x => x.Name).MinimumLength(5).MaximumLength(20);
    RuleFor(x => x.StartTime).GreaterThan(new DateTime(2000, 01, 01)).LessThan(x => x.FinishTime);
    RuleFor(x => x.FinishTime).GreaterThan(x => x.StartTime);
  }
}
