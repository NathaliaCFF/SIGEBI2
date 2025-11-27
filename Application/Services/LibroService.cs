using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using SIGEBI.Shared.Base;

namespace SIGEBI.Application.Services
{
    // ============================================================================
    // SERVICIO: LibroService
    // MÓDULO: Gestión de Libros
    // DESCRIPCIÓN: Contiene la lógica de aplicación para la administración y
    // consulta de libros, coordinando la interacción entre la interfaz de usuario
    // y la capa de persistencia.
    // CASOS DE USO RELACIONADOS:
    //   - CU-01: Registrar libro
    //   - CU-02: Editar libro
    //   - CU-03: Buscar libro
    //   - CU-04: Consultar estado de libro
    // CAPA: Aplicación
    // ============================================================================
    public class LibroService : ILibroService
    {
        private readonly ILibroRepository _repo;

        public LibroService(ILibroRepository repo)
        {
            _repo = repo;
        }

        // ============================================================================
        // CU-01 - Registrar libro
        // ============================================================================
        public async Task<ServiceResult<Libro>> CrearAsync(Libro libro)
        {
            if (await _repo.ExisteISBNAsync(libro.ISBN))
                return ServiceResult<Libro>.Fail("El ISBN ya existe.");

            libro.Activo = true;
            libro.Disponible = true;
            libro.FechaCreacion = DateTime.UtcNow;

            var result = await _repo.AddAsync(libro);

            return result.Success
                ? ServiceResult<Libro>.Ok(result.Data!, "Libro registrado correctamente.")
                : ServiceResult<Libro>.Fail(result.Message ?? "Error al registrar el libro.");
        }

        // ============================================================================
        // CU-02 - Editar libro
        // ============================================================================
        public async Task<ServiceResult<Libro>> ActualizarAsync(int id, Libro libro)
        {
            var existente = await _repo.GetByIdAsync(id);
            if (!existente.Success || existente.Data == null)
                return ServiceResult<Libro>.Fail("Libro no encontrado.");

            // Validar ISBN duplicado si fue modificado
            if (!string.Equals(existente.Data.ISBN, libro.ISBN, StringComparison.OrdinalIgnoreCase)
                && await _repo.ExisteISBNAsync(libro.ISBN))
                return ServiceResult<Libro>.Fail("El ISBN ya existe.");

            var e = existente.Data;

            // Datos modificables
            e.Titulo = libro.Titulo;
            e.Autor = libro.Autor;
            e.ISBN = libro.ISBN;
            e.Editorial = libro.Editorial;
            e.Categoria = libro.Categoria;
            e.AnioPublicacion = libro.AnioPublicacion;


            e.Disponible = libro.Disponible;
            e.Activo = libro.Activo;

            e.FechaModificacion = DateTime.UtcNow;

            var update = await _repo.UpdateAsync(e);

            return update.Success
                ? ServiceResult<Libro>.Ok(update.Data!, "Libro actualizado correctamente.")
                : ServiceResult<Libro>.Fail(update.Message ?? "Error al actualizar el libro.");
        }

        // ============================================================================
        // CU-03 - Buscar libro
        // ============================================================================
        public async Task<ServiceResult<IEnumerable<Libro>>> BuscarAsync(string criterio)
        {
            var lista = await _repo.BuscarPorTituloOAutorAsync(criterio ?? string.Empty);
            return ServiceResult<IEnumerable<Libro>>.Ok(lista, "Búsqueda completada.");
        }

        // ============================================================================
        // CU-04 - Consultar estado de disponibilidad
        // ============================================================================
        public async Task<ServiceResult<bool>> EstaDisponibleAsync(int id)
        {
            var disponible = await _repo.EstaDisponibleAsync(id);
            return ServiceResult<bool>.Ok(disponible, "Consulta completada.");
        }

        // ============================================================================
        // Extensión CU-04: Cambiar disponibilidad
        // ============================================================================
        public async Task<ServiceResult> CambiarDisponibilidadAsync(int id, bool disponible)
        {
            var ok = await _repo.CambiarDisponibilidadAsync(id, disponible);
            return ok
                ? ServiceResult.Ok("Disponibilidad actualizada correctamente.")
                : ServiceResult.Fail("No se pudo cambiar la disponibilidad.");
        }

        // ============================================================================
        // Desactivar libro (Soft Delete)
        // ============================================================================
        public async Task<ServiceResult> DesactivarAsync(int id)
        {
            var del = await _repo.DeleteAsync(id);
            return del.Success
                ? ServiceResult.Ok(del.Message ?? "Libro desactivado correctamente.")
                : ServiceResult.Fail(del.Message ?? "Error al desactivar el libro.");
        }

        // ============================================================================
        // Obtener libro por ID
        // ============================================================================
        public async Task<ServiceResult<Libro>> ObtenerPorIdAsync(int id)
        {
            var get = await _repo.GetByIdAsync(id);
            return get.Success && get.Data != null
                ? ServiceResult<Libro>.Ok(get.Data, "Consulta exitosa.")
                : ServiceResult<Libro>.Fail(get.Message ?? "No se pudo obtener el libro.");
        }

        // ============================================================================
        // Listar todos
        // ============================================================================
        public async Task<ServiceResult<IEnumerable<Libro>>> ListarAsync()
        {
            var all = await _repo.GetAllAsync();
            return all.Success
                ? ServiceResult<IEnumerable<Libro>>.Ok(all.Data!, "Listado obtenido correctamente.")
                : ServiceResult<IEnumerable<Libro>>.Fail(all.Message ?? "Error al listar los libros.");
        }

        // ============================================================================
        // ACTIVAR LIBRO 
        // ============================================================================
        public async Task<ServiceResult> ActivarAsync(int id)
        {
            var libro = await _repo.GetByIdAsync(id);
            if (!libro.Success || libro.Data == null)
                return ServiceResult.Fail("Libro no encontrado.");

            libro.Data.Activo = true;
            libro.Data.Disponible = true;
            libro.Data.FechaModificacion = DateTime.UtcNow;

            var update = await _repo.UpdateAsync(libro.Data);

            return update.Success
                ? ServiceResult.Ok("Libro activado correctamente.")
                : ServiceResult.Fail(update.Message);
        }
    }
}
