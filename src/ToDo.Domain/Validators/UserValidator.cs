using FluentValidation;
using ToDo.Domain.Entities;

namespace ToDo.Domain.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty()
            .WithMessage("O funcionario tem que ter um nome")
            .NotNull()
            .WithMessage("O nome nao pode ser nulo")
            .MinimumLength(3)
            .WithMessage("O nome deve ter no minimo 3 caracteres")
            .MaximumLength(70)
            .WithMessage("O nome deve ter no maximo 70 caracteres");
        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("O funcionario tem que ter um email")
            .NotNull()
            .WithMessage("O email nao pode ser nulo")
            .MinimumLength(11)
            .WithMessage("O email deve ter no minimo 11 caracteres")
            .MaximumLength(180)
            .WithMessage("O nome deve ter no maximo 180 caracteres")
            .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Senha não pode estar vazia")
            .NotNull().WithMessage("Senha não pode estar nula")
            .MinimumLength(4).WithMessage("A senha deve ter no minimo 4 caracteres")
            .MaximumLength(40).WithMessage("A senha deve ter no maximo 40 caracteres");
    }
}