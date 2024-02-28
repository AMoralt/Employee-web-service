
public interface IUnitOfWork : IDisposable
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    ValueTask StartTransaction(CancellationToken cancellationToken = default);
}