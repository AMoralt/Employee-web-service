
using Microsoft.Extensions.Options;
using Npgsql;

public class NpgsqlConnectionFactory : IDbConnectionFactory<NpgsqlConnection>
{
    private readonly string _connectionString;
    private  NpgsqlConnection? _connection; 
    public NpgsqlConnectionFactory(IOptions<DatabaseConnectionOptions> options)
    {
        _connectionString = options.Value.ConnectionString;
    }
    public async Task<NpgsqlConnection> CreateConnection(CancellationToken token)
    {
        if (_connection is not null)
            return _connection;

        _connection = new NpgsqlConnection(_connectionString);
        await _connection.OpenAsync(token);
        return _connection;
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}