using UserService.Domain.Common;

namespace UserService.Domain.User;

public class UserRegisteredDomainEvent : DomainEvent
{
    public UserId RegisteredUserId { get; }

    public UserRegisteredDomainEvent(DomainEventId id,
        DateTime createdAt,
        UserId registeredUserId) : base(id,
        createdAt)
    {
        RegisteredUserId = registeredUserId
            ?? throw new ArgumentNullException(nameof(registeredUserId));
    }

    public static UserRegisteredDomainEvent New(UserId userId)
        => new UserRegisteredDomainEvent(
            DomainEventId.New(),
            DateTime.UtcNow,
            userId);
}