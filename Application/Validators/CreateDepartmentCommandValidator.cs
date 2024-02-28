using Application.Commands.Departments;
using Application.Commands.Employees;
using FluentValidation;

namespace Application.Validators;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(command => command.Phone)
            .NotEmpty().WithMessage("Phone is required")
            .Matches(@"^\+[1-9]\d{1,14}$").WithMessage("Invalid phone number format");
    }
}