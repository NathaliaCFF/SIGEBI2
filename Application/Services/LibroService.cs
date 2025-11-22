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
        // CASO DE USO: CU-01 - Registrar libro
        // DESCRIPCIÓN: Permite al Administrador registrar un nuevo libro, validando
        // duplicidad de ISBN y completando los metadatos del registro.
        // ============================================================================
        public async Task<ServiceResult<Libro>> CrearAsync(Libro libro)
        {
            // CU-01: Validar ISBN duplicado antes del registro
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
        // CASO DE USO: CU-02 - Editar libro
        // DESCRIPCIÓN: Permite al Administrador modificar la información de un libro
        // existente, incluyendo título, autor, ISBN, editorial y categoría.
        // ============================================================================
        public async Task<ServiceResult<Libro>> ActualizarAsync(int id, Libro libro)
        {
            var existente = await _repo.GetByIdAsync(id);
            if (!existente.Success || existente.Data == null)
                return ServiceResult<Libro>.Fail("Libro no encontrado.");

            // CU-02: Validar ISBN duplicado si el valor ha sido modificado
            if (!string.Equals(existente.Data.ISBN, libro.ISBN, StringComparison.OrdinalIgnoreCase)
                && await _repo.ExisteISBNAsync(libro.ISBN))
                return ServiceResult<Libro>.Fail("El ISBN ya existe.");

            var e = existente.Data;
            e.Titulo = libro.Titulo;
            e.Autor = libro.Autor;
            e.ISBN = libro.ISBN;
            e.Editorial = libro.Editorial;
            e.Categoria = libro.Categoria;
            e.AnioPublicacion = libro.AnioPublicacion;
            e.FechaModificacion = DateTime.UtcNow;

            var update = await _repo.UpdateAsync(e);
            return update.Success
                ? ServiceResult<Libro>.Ok(update.Data!, "Libro actualizado correctamente.")
                : ServiceResult<Libro>.Fail(update.Message ?? "Error al actualizar el libro.");
        }

        // ============================================================================
        // CASO DE USO: CU-03 - Buscar libro
        // DESCRIPCIÓN: Permite a usuarios autenticados (docentes/estudiantes)
        // realizar búsquedas por título o autor, mostrando solo libros activos.
        // ============================================================================
        public async Task<ServiceResult<IEnumerable<Libro>>> BuscarAsync(string criterio)
        {
            var lista = await _repo.BuscarPorTituloOAutorAsync(criterio ?? string.Empty);
            return ServiceResult<IEnumerable<Libro>>.Ok(lista, "Búsqueda completada.");
        }

        // ============================================================================
        // CASO DE USO: CU-04 - Consultar estado de libro
        // DESCRIPCIÓN: Permite verificar si un libro se encuentra disponible
        // para préstamo o se encuentra prestado actualmente.
        // ============================================================================
        public async Task<ServiceResult<bool>> EstaDisponibleAsync(int id)
        {
            var disponible = await _repo.EstaDisponibleAsync(id);
            return ServiceResult<bool>.Ok(disponible, "Consulta completada.");
        }

        // ============================================================================
        // CASO DE USO: CU-04 (extensión) - Cambiar disponibilidad
        // DESCRIPCIÓN: Cambia el estado de disponibilidad de un libro. Utilizado
        // por otros módulos (por ejemplo, Préstamos y Devoluciones).
        // ============================================================================
        public async Task<ServiceResult> CambiarDisponibilidadAsync(int id, bool disponible)
        {
            var ok = await _repo.CambiarDisponibilidadAsync(id, disponible);
            return ok
                ? ServiceResult.Ok("Disponibilidad actualizada correctamente.")
                : ServiceResult.Fail("No se pudo cambiar la disponibilidad.");
        }

        // ============================================================================
        // MÉTODO AUXILIAR: Desactivar libro
        // DESCRIPCIÓN: Realiza una desactivación lógica (soft delete) del libro,
        // manteniendo la integridad referencial en la base de datos.
        // ============================================================================
        public async Task<ServiceResult> DesactivarAsync(int id)
        {
            var del = await _repo.DeleteAsync(id);
            return del.Success
                ? ServiceResult.Ok(del.Message ?? "Libro desactivado correctamente.")
                : ServiceResult.Fail(del.Message ?? "Error al desactivar el libro.");
        }

        // ============================================================================
        // MÉTODO AUXILIAR: Obtener libro por ID
        // DESCRIPCIÓN: Recupera los datos de un libro específico según su identificador.
        // ============================================================================
        public async Task<ServiceResult<Libro>> ObtenerPorIdAsync(int id)
        {
            var get = await _repo.GetByIdAsync(id);
            return get.Success && get.Data != null
                ? ServiceResult<Libro>.Ok(get.Data, "Consulta exitosa.")
                : ServiceResult<Libro>.Fail(get.Message ?? "No se pudo obtener el libro.");
        }

        // ============================================================================
        // MÉTODO AUXILIAR: Listar todos los libros
        // DESCRIPCIÓN: Devuelve el listado completo de libros registrados.
        // ============================================================================
        public async Task<ServiceResult<IEnumerable<Libro>>> ListarAsync()
        {
            var all = await _repo.GetAllAsync();
            return all.Success
                ? ServiceResult<IEnumerable<Libro>>.Ok(all.Data!, "Listado obtenido correctamente.")
                : ServiceResult<IEnumerable<Libro>>.Fail(all.Message ?? "Error al listar los libros.");
        }
    }
}
