using UserService.Domain.Friends;
using UserService.Domain.Person;
using UserService.Domain.Posts;
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
    public EntitySet<Person, PersonId>? Persons { get; private set; }
    public EntitySet<Post, PostId>? Posts { get; private set; }
    public EntitySet<Friendship, FriendshipId>? Friendships { get; private set; }
    public DomainEventSet DomainEvents { get; }

    public EntitiesContext(PostgresContext postgresContext, DomainEventsDatabaseAdapter domainEventsDatabaseAdapter, ServiceFactory serviceFactory)
    {
        _postgresContext = postgresContext;
        _domainEventsDatabaseAdapter = domainEventsDatabaseAdapter;
        _serviceFactory = serviceFactory;
        DomainEvents = new DomainEventSet(_unitOfWork);
    }

    public async Task<DatabaseTransaction> BeginTransactionAsync()
    {
        if (_transaction is { Active: true })
            throw new InvalidOperationException("There is already running transaction");
        
        _transaction = await _postgresContext.BeginTransactionAsync();
        Users = new EntitySet<User, UserId>(_serviceFactory.GetAdapter<User, UserId>(), _unitOfWork, _transaction);
        Persons = new EntitySet<Person, PersonId>(_serviceFactory.GetAdapter<Person, PersonId>(), _unitOfWork, _transaction);
        Posts = new EntitySet<Post, PostId>(_serviceFactory.GetAdapter<Post, PostId>(), _unitOfWork, _transaction);
        Friendships = new EntitySet<Friendship, FriendshipId>(_serviceFactory.GetAdapter<Friendship, FriendshipId>(), _unitOfWork, _transaction);
        
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