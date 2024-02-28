
using MediatR;
using Npgsql;

public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private NpgsqlTransaction? _npgsqlTransaction;
        private readonly IDbConnectionFactory<NpgsqlConnection>? _dbConnectionFactory;
        private readonly IPublisher _publisher;

        public UnitOfWork(
            IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async ValueTask StartTransaction(CancellationToken token)
        {
            if (!(_npgsqlTransaction is null && _dbConnectionFactory is not null))
                return;
            var connection = await _dbConnectionFactory.CreateConnection(token);
            _npgsqlTransaction = await connection.BeginTransactionAsync(token);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            if (_npgsqlTransaction is null)
            {
                throw new NoActiveTransactionStartedException();
            }

            await _npgsqlTransaction.CommitAsync(cancellationToken);
        }

        void IDisposable.Dispose()
        {
            _npgsqlTransaction?.Dispose();
            _dbConnectionFactory?.Dispose();
        }
    }