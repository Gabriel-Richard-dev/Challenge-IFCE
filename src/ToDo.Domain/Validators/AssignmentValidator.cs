using FluentValidation;
using ToDo.Domain.Entities;

namespace ToDo.Domain.Validators;

public class AssignmentValidator : AbstractValidator<Assignment>
{
    public AssignmentValidator()
    {
        RuleFor(a => a.Title)
            .NotNull()
            .WithMessage("A tarefa tem que ter um titulo!")
            .NotEmpty()
            .WithMessage("O titulo da tarefa não pode estar vazio"); 
        RuleFor(a => a.Description)
            .MaximumLength(300)
            .WithMessage("A descrição da tarefa no maximo pode ter 300 caracteres");
        RuleFor(a => a.DateConcluded)
            .NotNull().When(a => a.Concluded == true)
            .WithMessage("Se você colocou que foi concluida você tem que inserir o valor")
            .NotEmpty().When(a => a.Concluded == true)
            .WithMessage("Se você colocou que foi concluida você tem que inserir o valor");
    }
}