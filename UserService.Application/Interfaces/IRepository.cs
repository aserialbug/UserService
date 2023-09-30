using System.Threading.Tasks;
using UserService.Domain.Common;

namespace UserService.Application.Interfaces;

public interface IRepository<TEntity, in TId> 
    where TEntity : Entity<TId> 
    where TId : BaseId
{
    Task<TEntity> this[TId id] { get; }
    Task Add(TEntity entity);
    Task Remove(TEntity entity);
}