
using Shared;
using System.Linq.Expressions;

namespace SIGEBI.Shared.Base
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<OperationResult<T>> GetByIdAsync(int id);
        Task<OperationResult<IEnumerable<T>>> GetAllAsync();
        Task<OperationResult<IEnumerable<T>>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<OperationResult<T>> AddAsync(T entity);
        Task<OperationResult<T>> UpdateAsync(T entity);
        Task<OperationResult> DeleteAsync(int id);
    }
}
