namespace UserService.Infrastructure.Context;

public enum EntityState
{
    New,
    Clean,
    Dirty,
    Deleted
}