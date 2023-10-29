using UserService.Domain.Common;

namespace UserService.Infrastructure.Context;

public class DomainEventSet
{
    private readonly UnitOfWork _unitOfWork;

    public IEnumerable<DomainEvent> Events => _unitOfWork
        .EventTrackers
        .Select(t => t.Event);

    public DomainEventSet(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task RegisterEvent(DomainEvent domainEvent)
    {
        _unitOfWork.RegisterEvent(domainEvent);
        return Task.CompletedTask;
    }
}