using Domain;
using Infrastructure.Contracts;
using MediatR;

namespace Application.Commands.Departments;

public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, bool>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateDepartmentCommandHandler(IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork)
    {
        _departmentRepository = departmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = new Department()
        {
            Id = request.DepartmentId,
            Name = request.Name,
            Phone = request.Phone
        };

        await _unitOfWork.StartTransaction(cancellationToken);
        var result = await _departmentRepository.UpdateAsync(department, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }
}

public class UpdateDepartmentCommand : IRequest<bool>
{
    public int DepartmentId { get; init; }
    public string Name { get; init; }
    public string Phone { get; init; }
}