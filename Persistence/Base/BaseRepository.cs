using Microsoft.EntityFrameworkCore;
using SIGEBI.Shared.Base;
using SIGEBI.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Persistence.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<OperationResult<T>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                return entity != null
                    ? OperationResult<T>.Ok(entity)
                    : OperationResult<T>.Fail("Entidad no encontrada");
            }
            catch (Exception ex)
            {
                return OperationResult<T>.Fail($"Error al obtener entidad: {ex.Message}");
            }
        }

        public async Task<OperationResult<IEnumerable<T>>> GetAllAsync()
        {
            try
            {
                var entities = await _dbSet.ToListAsync();
                return OperationResult<IEnumerable<T>>.Ok(entities);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<T>>.Fail($"Error al obtener lista: {ex.Message}");
            }
        }

        public async Task<OperationResult<IEnumerable<T>>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var result = await _dbSet.Where(predicate).ToListAsync();
                return OperationResult<IEnumerable<T>>.Ok(result);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<T>>.Fail($"Error en búsqueda: {ex.Message}");
            }
        }

        public async Task<OperationResult<T>> AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return OperationResult<T>.Ok(entity, "Entidad agregada correctamente");
            }
            catch (Exception ex)
            {
                return OperationResult<T>.Fail($"Error al agregar entidad: {ex.Message}");
            }
        }

        public async Task<OperationResult<T>> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return OperationResult<T>.Ok(entity, "Entidad actualizada correctamente");
            }
            catch (Exception ex)
            {
                return OperationResult<T>.Fail($"Error al actualizar entidad: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                    return OperationResult<bool>.Fail("Entidad no encontrada");

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return OperationResult<bool>.Ok(true, "Entidad eliminada correctamente");
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Fail($"Error al eliminar entidad: {ex.Message}");
            }
        }
    }
}
