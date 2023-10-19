using MassTransit;

namespace UserService.Domain.Common;

public abstract class SimpleId : BaseId
{
    // NewId это тип сортируемых уникальных идентификатороу
    private readonly NewId _id;

    protected SimpleId(NewId id)
    {
        if (id == NewId.Empty)
            throw new ArgumentNullException(nameof(id));

        _id = id;
    }

    public override string ToString()
    {
        return $"{GetType().Name}_{_id:N}";
    }

    public Guid ToGuid()
    {
        return _id.ToGuid();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return _id;
    }
}