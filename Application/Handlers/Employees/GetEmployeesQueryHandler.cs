using Domain;
using Infrastructure.Contracts;
using MediatR;

namespace Application.Handlers.Employees;

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, IEnumerable<EmployeeDTO>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetEmployeesQueryHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<EmployeeDTO>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.GetAllAsync(request.Limit, request.Offset, cancellationToken);

        if (!employees.Any())
            throw new System.Exception("No employees found in the repository");

        return employees;
    }
}


public class GetEmployeesQuery : IRequest<IEnumerable<EmployeeDTO>>
{
    public int Limit { get; init; }
    public int Offset { get; set; }
}