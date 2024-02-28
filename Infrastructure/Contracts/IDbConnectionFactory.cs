public interface IDbConnectionFactory<TConnection> : IDisposable
{
    Task<TConnection> CreateConnection(CancellationToken token);
}