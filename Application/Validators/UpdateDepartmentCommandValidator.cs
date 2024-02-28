using Application.Commands.Departments;
using FluentValidation;

namespace Application.Validators;

public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentCommandValidator()
    {
        RuleFor(command => command.DepartmentId)
            .NotEmpty().WithMessage("DepartmentId is required");
            
        RuleFor(command => command.DepartmentId)
            .NotEmpty().WithMessage("DepartmentId is required");
        
        RuleFor(command => command.Phone)
            .Matches(@"^\+[1-9]\d{1,14}$").WithMessage("Invalid phone number format")
            .When(command => !string.IsNullOrEmpty(command.Phone));
    }
}