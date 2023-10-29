using UserService.Domain.User;
using UserService.Infrastructure.Adapters;

namespace UserService.Infrastructure.Context;

public class EntitiesContext
{
    private readonly PostgresContext _postgresContext;
    private DatabaseTransaction? _transaction;
    private readonly UnitOfWork _unitOfWork = new();
    private readonly DomainEventsDatabaseAdapter _domainEventsDatabaseAdapter;
    private readonly ServiceFactory _serviceFactory;

    public EntitySet<User, UserId>? Users { get; private set; }

    public EntitiesContext(PostgresContext postgresContext, DomainEventsDatabaseAdapter domainEventsDatabaseAdapter, ServiceFactory serviceFactory)
    {
        _postgresContext = postgresContext;
        _domainEventsDatabaseAdapter = domainEventsDatabaseAdapter;
        _serviceFactory = serviceFactory;
    }

    public async Task<DatabaseTransaction> BeginTransactionAsync()
    {
        if (_transaction is { Active: true })
            throw new InvalidOperationException("There is already running transaction");
        
        _transaction = await _postgresContext.BeginTransactionAsync();
        Users = new EntitySet<User, UserId>(_serviceFactory.GetAdapter<User, UserId>(), _unitOfWork, _transaction);
        
        return _transaction;
    }

    public async Task SaveChangesAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("Transaction has not been initialized");
        
        foreach (var changeTracker in _unitOfWork.Trackers)
        {
            await changeTracker.SaveChangesAsync(_transaction, _domainEventsDatabaseAdapter);
        }

        var domainEvents = _unitOfWork.EventTrackers
            .Where(de => de.State == DomainEventState.New)
            .ToArray();
        await _domainEventsDatabaseAdapter.AddRange(domainEvents.Select(t => t.Event), _transaction);
        foreach (var tracker in domainEvents)
        {
            tracker.Saved();
        }
    }
}