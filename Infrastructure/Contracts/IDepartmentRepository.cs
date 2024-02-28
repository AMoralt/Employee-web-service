using Domain;

namespace Infrastructure.Contracts;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<int> CreateAsync(Department item, CancellationToken cancellationToken = default);
    Task<IEnumerable<DepartmentDTO>> GetAllAsync(int limit, int offset, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Department item, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int departmentId, CancellationToken cancellationToken = default);
}