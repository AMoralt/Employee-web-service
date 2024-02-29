using Domain;
using Infrastructure.Contracts;
using MediatR;

namespace Application.Queries.Departments;

public class GetDepartmentsQueryHandler : IRequestHandler<GetDepartmentsQuery, IEnumerable<DepartmentDTO>>
{
    private readonly IDepartmentRepository _departmentRepository;

    public GetDepartmentsQueryHandler(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<IEnumerable<DepartmentDTO>> Handle(GetDepartmentsQuery request, CancellationToken cancellationToken)
    {
        var departments = await _departmentRepository.GetAllAsync(request.Limit, request.Offset, cancellationToken);

        if (!departments.Any())
            throw new System.Exception("No depts found in the repository");

        return departments;
    }
}


public class GetDepartmentsQuery : IRequest<IEnumerable<DepartmentDTO>>
{
    public int Limit { get; init; }
    public int Offset { get; set; }
}