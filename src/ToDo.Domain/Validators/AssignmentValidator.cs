using FluentValidation;
using ToDo.Domain.Entities;

namespace ToDo.Domain.Validators;

public class AssignmentValidator : AbstractValidator<Assignment>
{
    public AssignmentValidator()
    {
        RuleFor(a => a.Title)
            .NotNull().WithMessage("A tarefa tem que ter um titulo!")
            .NotEmpty().WithMessage("O titulo da tarefa não pode estar vazio");        
        RuleFor(a => a.Deadline)
            .NotNull().WithMessage("A tarefa tem que ter um titulo!")
            .NotEmpty().WithMessage("O titulo da tarefa não pode estar vazio");
    }
}