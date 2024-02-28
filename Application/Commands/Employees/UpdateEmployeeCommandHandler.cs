using Domain;
using Infrastructure.Contracts;
using MediatR;

namespace Application.Commands.Employees;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, bool>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = new Employee
        {
            Id = request.Id,
            Name = request.Name,
            Surname = request.Surname,
            Phone = request.Phone,
            CompanyId = request.CompanyId,
            Department = new Department{Id = request.DepartmentId} ,
            Passport = new Passport{Type = request.Passport.Type, Number = request.Passport.Number}
        };

        await _unitOfWork.StartTransaction(cancellationToken);
        var result = await _employeeRepository.UpdateAsync(employee, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }
}

public class UpdateEmployeeCommand : IRequest<bool>
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Surname { get; init; }
    public string Phone { get; init; }
    public int? CompanyId { get; init; }
    public int? DepartmentId { get; init; }
    public PassportDTO Passport { get; init; } = new PassportDTO();
}