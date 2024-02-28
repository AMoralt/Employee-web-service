using Application.Commands.Employees;
using FluentValidation;

namespace Application.Validators;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("Id is required");
        
        RuleFor(command => command.Phone)
            .Matches(@"^\+[1-9]\d{1,14}$").WithMessage("Invalid phone number format")
            .When(command => !string.IsNullOrEmpty(command.Phone));
        
        RuleFor(command => command.CompanyId)
            .GreaterThan(0).WithMessage("CompanyId must be greater than 0")
            .When(command => command.CompanyId.HasValue); // Применяем только если CompanyId задан
        
        RuleFor(command => command.DepartmentId)
            .GreaterThan(0).WithMessage("DepartmentId must be greater than 0")
            .When(command => command.DepartmentId.HasValue); // Применяем только если DepartmentId задан
    }
}