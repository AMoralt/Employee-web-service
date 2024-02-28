using Dapper;
using Domain;
using Infrastructure.Contracts;
using Infrastructure.Extensions;
using Npgsql;

namespace Infrastructure.Repository;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
    public DepartmentRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<int> CreateAsync(Department item, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            INSERT INTO Department (Name, Phone)
            VALUES (@Name, @Phone)
            RETURNING Id;
        ";
        
        var parameters = new
        {
            Name = item.Name,
            Phone = item.Phone
        };

        var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
        var newId = await connection.ExecuteScalarAsync<int>(sql, param: parameters);

        return newId;
    }

    public async Task<IEnumerable<DepartmentDTO>> GetAllAsync(int limit, int offset, CancellationToken cancellationToken)
    {
        string sql = @"
            SELECT
                d.id, d.name, d.phone
            FROM Department AS d
            OFFSET @Offset";
        
        if (limit != 0)
            sql += "\rLIMIT @Limit";
        
        var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
        
        var departments = await connection.QueryAsync<DepartmentDTO>(sql, param: new {Offset = offset, Limit = limit});
        
        return departments;
    }

    public async Task<bool> UpdateAsync(Department item, CancellationToken cancellationToken = default)
    {
        var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
        var parameters = new
        {
            Id = item.Id,
            Name = item.Name,
            Phone = item.Phone,
        };
        var isSuccess = connection.UpdateFields<Department>(param: parameters);
        
        return isSuccess;
    }

    public async Task<bool> DeleteAsync(int departmentId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            DELETE
            FROM Department
            WHERE Id = @DepartmentId";
        
        var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
        
        var affectedRows =await connection.ExecuteAsync(sql, param: new { DepartmentId = departmentId });
        var isSuccess = affectedRows > 0;

        return isSuccess;
    }
}
