using Microsoft.EntityFrameworkCore;
using Shared;
using SIGEBI.Persistence.Context;
using SIGEBI.Shared.Base;
using System.Linq.Expressions;

namespace SIGEBI.Persistence.Base
{
    // ============================================================================
    // REPOSITORIO BASE: BaseRepository<T>
    // MÓDULO: Repositorios Genéricos
    // DESCRIPCIÓN: Implementa las operaciones CRUD comunes para todas las entidades
    // del sistema SIGEBI, proporcionando un punto de acceso estándar a la base de datos
    // mediante Entity Framework Core.
    // CASOS DE USO RELACIONADOS:
    //   - CU-01 al CU-14 (Soporte transversal para creación, consulta, actualización
    //     y desactivación de entidades en todos los módulos del sistema)
    // CAPA: Persistencia
    // ============================================================================
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // ============================================================================
        // READ: Obtener por ID
        // DESCRIPCIÓN: Recupera una entidad específica según su identificador.
        // UTILIZADO EN: Consultas de detalle en múltiples casos de uso (ej. CU-02, CU-06, CU-09)
        // ============================================================================
        public async Task<OperationResult<T>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _dbSet
                   .AsNoTracking()
                   .FirstOrDefaultAsync(e => e.Id == id);

                return entity != null
                    ? OperationResult<T>.Ok(entity)
                    : OperationResult<T>.Fail("Entidad no encontrada.");
            }
            catch (Exception ex)
            {
                return OperationResult<T>.Fail($"Error al obtener entidad: {ex.Message}");
            }
        }

        // ============================================================================
        // READ: Obtener todos
        // DESCRIPCIÓN: Devuelve todas las entidades activas o registradas en la tabla.
        // UTILIZADO EN: Listados generales (CU-03, CU-11, CU-13, etc.)
        // ============================================================================
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

        // ============================================================================
        // READ: Buscar con predicado
        // DESCRIPCIÓN: Permite aplicar filtros personalizados mediante expresiones lambda.
        // UTILIZADO EN: búsquedas dinámicas (CU-03, CU-11, CU-12, CU-13, etc.)
        // ============================================================================
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

        // ============================================================================
        // CREATE: Agregar nueva entidad
        // DESCRIPCIÓN: Inserta un nuevo registro en la base de datos y confirma los cambios.
        // UTILIZADO EN: Creación de entidades (CU-01, CU-05, CU-09, etc.)
        // ============================================================================
        public async Task<OperationResult<T>> AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return OperationResult<T>.Ok(entity, "Entidad agregada correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<T>.Fail($"Error al agregar entidad: {ex.Message}");
            }
        }

        // ============================================================================
        // UPDATE: Actualizar entidad existente
        // DESCRIPCIÓN: Modifica un registro existente en la base de datos y guarda los cambios.
        // UTILIZADO EN: Edición y actualización (CU-02, CU-06, CU-10, CU-14)
        // ============================================================================
        public async Task<OperationResult<T>> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return OperationResult<T>.Ok(entity, "Entidad actualizada correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<T>.Fail($"Error al actualizar entidad: {ex.Message}");
            }
        }

        // ============================================================================
        // DELETE (Soft Delete): Desactivar entidad
        // DESCRIPCIÓN: Realiza una eliminación lógica cambiando el estado de Activo a false,
        // preservando la integridad referencial y manteniendo el historial del registro.
        // UTILIZADO EN: Eliminaciones y desactivaciones (CU-08, CU-05, CU-02)
        // ============================================================================
        public async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                    return OperationResult.Fail("Entidad no encontrada.");

                // CU-08 / CU-05 / CU-02: Desactivación lógica
                entity.Activo = false;
                entity.FechaModificacion = DateTime.UtcNow;

                _dbSet.Update(entity);
                await _context.SaveChangesAsync();

                return OperationResult.Ok("Entidad desactivada correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult.Fail($"Error al desactivar entidad: {ex.Message}");
            }
        }
    }
}
