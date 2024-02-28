using Application.Commands.Employees;
using FluentValidation;

namespace Application.Validators;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("Name is required");
        
        RuleFor(command => command.Surname)
            .NotEmpty().WithMessage("Surname is required");

        RuleFor(command => command.Phone)
            .NotEmpty().WithMessage("Phone is required")
            .Matches(@"^\+[1-9]\d{1,14}$").WithMessage("Invalid phone number format");
        
        RuleFor(command => command.CompanyId)
            .GreaterThan(0).WithMessage("CompanyId must be greater than 0");
        
        RuleFor(command => command.Passport.Type)
            .NotEmpty().WithMessage("PassportType is required");
        
        RuleFor(command => command.Passport.Number)
            .NotEmpty().WithMessage("PassportNumber is required");
        
        RuleFor(command => command.DepartmentId)
            .GreaterThan(0).WithMessage("DepartmentId must be greater than 0");
    }
}