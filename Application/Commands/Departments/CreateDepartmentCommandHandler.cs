using Domain;
using Infrastructure.Contracts;
using MediatR;

namespace Application.Commands.Departments;

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, int>
{
    private readonly IDepartmentRepository _departmentRepository;
    public CreateDepartmentCommandHandler(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<int> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = new Department()
        {
            Name = request.Name,
            Phone = request.Phone
        };
        
        var newId = await _departmentRepository.CreateAsync(department, cancellationToken);
        
        return newId;
    }
}
public class CreateDepartmentCommand : IRequest<int>
{
    public string Name { get; init; }
    public string Phone { get; init; }
}
/*
public record CreateEmployeeCommand(
    [Required(ErrorMessage = "Name is required")]
    string Name, 
    
    [Required(ErrorMessage = "Surname is required")]
    string Surname, 
    
    [Required(ErrorMessage = "Phone is required")]
    [RegularExpression(@"^\+[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number format")]
    string Phone, 
    
    [Required(ErrorMessage = "CompanyId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "CompanyId must be greater than 0")]
    int CompanyId, 
    
    [Required(ErrorMessage = "PassportType is required")]
    string PassportType, 
    [Required(ErrorMessage = "PassportNumber is required")]
    string PassportNumber, 
    
    [Required(ErrorMessage = "DepartmentId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "DepartmentId must be greater than 0")]
    int DepartmentId
) : IRequest<int>;
*/