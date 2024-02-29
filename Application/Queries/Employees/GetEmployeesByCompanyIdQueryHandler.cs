using Domain;
using Infrastructure.Contracts;
using MediatR;

namespace Application.Queries.Employees;

public class GetEmployeesByCompanyIdQueryHandler : IRequestHandler<GetEmployeesByCompanyIdQuery, IEnumerable<EmployeeDTO>>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeesByCompanyIdQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<EmployeeDTO>> Handle(GetEmployeesByCompanyIdQuery request, CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.GetAllByCompanyIdAsync(request.CompanyId, request.Limit, request.Offset, cancellationToken);

        if (!employees.Any())
            throw new System.Exception("No employees found in the repository");

        return employees;
    }
}

public class GetEmployeesByCompanyIdQuery : IRequest<IEnumerable<EmployeeDTO>>
{
    public int CompanyId { get; init; }
    public int Limit { get; init; }
    public int Offset { get; set; }
}