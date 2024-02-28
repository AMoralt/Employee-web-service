using Domain;
using Infrastructure.Contracts;
using MediatR;

namespace Application.Handlers.Employees;

public class GetEmployeesByDepartmentIdQueryHandler : IRequestHandler<GetEmployeesByDepartmentIdQuery, IEnumerable<EmployeeDTO>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetEmployeesByDepartmentIdQueryHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<EmployeeDTO>> Handle(GetEmployeesByDepartmentIdQuery request, CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.GetAllByDepartmentIdAsync(request.DepartmentId,request.Limit, request.Offset, cancellationToken);

        if (!employees.Any())
            throw new System.Exception("No employees found in the repository");

        return employees;
    }
}

public class GetEmployeesByDepartmentIdQuery : IRequest<IEnumerable<EmployeeDTO>>
{
    public int DepartmentId { get; init; }
    public int Limit { get; init; }
    public int Offset { get; set; }
}