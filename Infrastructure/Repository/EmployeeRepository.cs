using Dapper;
using Domain;
using Infrastructure.Contracts;
using Infrastructure.Extensions;
using Npgsql;

namespace Infrastructure.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
    public EmployeeRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task<int> CreateAsync(Employee item, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            INSERT INTO Employee (Name, Surname, Phone, CompanyId, Type, Number, DepartmentId)
            VALUES (@Name, @Surname, @Phone, @CompanyId, @Type, @Number, @DepartmentId)
            RETURNING Id;
        ";
        
        var parameters = new
        {
            Name = item.Name,
            Surname = item.Surname,
            Phone = item.Phone,
            CompanyId = item.CompanyId,
            Type = item.Passport.Type,
            Number = item.Passport.Number,
            DepartmentId = item.Department.Id
        };

        var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
        var newId = await connection.ExecuteScalarAsync<int>(sql, param: parameters);

        return newId;
    }

    public async Task<IEnumerable<EmployeeDTO>> GetAllAsync(int limit, int offset, CancellationToken cancellationToken = default)
    { 
        string sql = @"
            SELECT 
                e.id, e.name, e.surname, e.phone, e.companyid, 0 split,
                e.type, e.number, 0 split,
                d.name, d.phone
            FROM employee AS e
            LEFT JOIN department AS d ON d.id = e.departmentId
            OFFSET @Offset";
        
        if (limit != 0)
            sql += "\rLIMIT @Limit";
        
        var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
        
        var employees = await connection.QueryAsync<EmployeeDTO, PassportDTO, DepartmentDTO, EmployeeDTO>(sql,
            (employee, passport, department) =>
            {
                return new EmployeeDTO
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Surname = employee.Surname,
                    Phone = employee.Phone,
                    CompanyId = employee.CompanyId,
                    Passport = new PassportDTO{Type = passport.Type, Number = passport.Number},
                    Department = new DepartmentDTO{Name = department.Name, Phone = department.Phone}
                };
            }, splitOn:"split, split", param: new {Offset = offset, Limit = limit});
        
        return employees;
    }

    public async Task<IEnumerable<EmployeeDTO>> GetAllByCompanyIdAsync(int companyId, int limit, int offset, CancellationToken cancellationToken = default)
    {
        string sql = @"
            SELECT 
                e.id, e.name, e.surname, e.phone, e.companyid, 0 split,
                e.type, e.number, 0 split,
                d.name, d.phone
            FROM employee AS e
            LEFT JOIN department AS d ON d.id = e.departmentId
            WHERE e.CompanyId = @CompanyId
            OFFSET @Offset";
        
        if (limit != 0)
            sql += "\rLIMIT @Limit";
        
        var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
        
        var employees = await connection.QueryAsync<EmployeeDTO, PassportDTO, DepartmentDTO, EmployeeDTO>(sql,
            (employee, passport, department) =>
            {
                return new EmployeeDTO
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Surname = employee.Surname,
                    Phone = employee.Phone,
                    CompanyId = employee.CompanyId,
                    Passport = new PassportDTO{Type = passport.Type, Number = passport.Number},
                    Department = new DepartmentDTO{Name = department.Name, Phone = department.Phone}
                };
            }, splitOn:"split, split", param: new {Offset = offset, Limit = limit, CompanyId = companyId});
        
        return employees;
    }

    public async Task<IEnumerable<EmployeeDTO>> GetAllByDepartmentIdAsync(int departmentId, int limit, int offset, CancellationToken cancellationToken = default)
    {
        string sql = @"
            SELECT 
                e.id, e.name, e.surname, e.phone, e.companyid, 0 split,
                e.type, e.number, 0 split,
                d.name, d.phone
            FROM employee AS e
            LEFT JOIN department AS d ON d.id = e.departmentId
            WHERE e.departmentId = @DepartmentId
            OFFSET @Offset";
        
        if (limit != 0)
            sql += "\rLIMIT @Limit";
        
        var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
        
        var employees = await connection.QueryAsync<EmployeeDTO, PassportDTO, DepartmentDTO, EmployeeDTO>(sql,
            (employee, passport, department) =>
            {
                return new EmployeeDTO
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Surname = employee.Surname,
                    Phone = employee.Phone,
                    CompanyId = employee.CompanyId,
                    Passport = new PassportDTO{Type = passport.Type, Number = passport.Number},
                    Department = new DepartmentDTO{Name = department.Name, Phone = department.Phone}
                };
            }, splitOn:"split, split", param: new {Offset = offset, Limit = limit, DepartmentId = departmentId});
        
        return employees;
    }
    public async Task<bool> UpdateAsync(Employee item, CancellationToken cancellationToken = default)
    {
        var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
        var parameters = new
        {
            Id = item.Id,
            Name = item.Name,
            Surname = item.Surname,
            Phone = item.Phone,
            CompanyId = item.CompanyId,
            Type = item.Passport.Type,
            Number = item.Passport.Number,
            DepartmentId = item.Department.Id
        };
        var isSuccess = connection.UpdateFields<Employee>(param: parameters);
        
        return isSuccess;
    }

    public async Task<bool> DeleteAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            DELETE
            FROM Employee
            WHERE Id = @Id;";
        
        var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
        
        var affectedRows = await connection.ExecuteAsync(sql, param: new { Id = employeeId });
        var isSuccess = affectedRows > 0;

        return isSuccess;
    }
}
