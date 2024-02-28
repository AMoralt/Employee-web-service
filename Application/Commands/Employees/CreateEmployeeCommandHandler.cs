using Domain;
using Infrastructure.Contracts;
using MediatR;

namespace Application.Commands.Employees;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, int>
{
    private readonly IEmployeeRepository _employeeRepository;
    public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = new Employee
        {
            Name = request.Name,
            Surname = request.Surname,
            Phone = request.Phone,
            CompanyId = request.CompanyId,
            Department = new Department{Id = request.DepartmentId},
            Passport = new Passport{Type = request.Passport.Type, Number = request.Passport.Number} 
        };
        
        var newId = await _employeeRepository.CreateAsync(employee, cancellationToken);
        
        return newId;
    }
}
public class CreateEmployeeCommand : IRequest<int>
{
    public string Name { get; init; }
    public string Surname { get; init; }
    public string Phone { get; init; }
    public int CompanyId { get; init; }
    public PassportDTO Passport { get; init; } = new PassportDTO();
    public int DepartmentId { get; init; }
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