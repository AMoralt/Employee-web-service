using Domain;

namespace Infrastructure.Contracts;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<int> CreateAsync(Employee item, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmployeeDTO>> GetAllAsync(int limit, int offset, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmployeeDTO>> GetAllByCompanyIdAsync(int companyId, int limit, int offset, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmployeeDTO>> GetAllByDepartmentIdAsync(int departmentId, int limit, int offset, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Employee item, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int employeeId, CancellationToken cancellationToken = default);
}