using FluentValidation;
using ToDo.Domain.Entities;

namespace ToDo.Domain.Validators;

public class AssignmentListValidator : AbstractValidator<AssignmentList>
{
    public AssignmentListValidator()
    {
        RuleFor(l => l.Name)
            .NotEmpty().WithMessage("Campo nome não pode estar vazio")
            .NotNull().WithMessage("Campo nome não pode estar nulo");
        RuleFor(l => l.UserId)
            .NotEmpty().WithMessage("Campo UserID não pode estar vazio")
            .NotNull().WithMessage("Campo UserID não pode estar nulo");
    }
}