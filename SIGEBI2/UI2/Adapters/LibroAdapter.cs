using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Entities;
using UI2.Models.Common;
using UI2.Models.Libros;

namespace UI2.Adapters
{
    public class LibroAdapter
    {
        private readonly ILibroService _libroService;

        public LibroAdapter(ILibroService libroService)
        {
            _libroService = libroService;
        }

        public async Task<AdapterResult<IList<LibroListItemModel>>> ListarLibrosAsync()
        {
            var resultado = await _libroService.ListarAsync();
            if (!resultado.Success || resultado.Data == null)
            {
                return AdapterResult<IList<LibroListItemModel>>.Fail(resultado.Message ?? "No se pudieron obtener los libros.");
            }

            return AdapterResult<IList<LibroListItemModel>>.Ok(Mapear(resultado.Data), resultado.Message ?? "Libros cargados correctamente.");
        }

        public async Task<AdapterResult<IList<LibroListItemModel>>> BuscarLibrosAsync(string criterio)
        {
            var resultado = await _libroService.BuscarAsync(criterio);
            if (!resultado.Success || resultado.Data == null)
            {
                return AdapterResult<IList<LibroListItemModel>>.Fail(resultado.Message ?? "No se encontraron libros.");
            }

            return AdapterResult<IList<LibroListItemModel>>.Ok(Mapear(resultado.Data), resultado.Message ?? "Búsqueda completada.");
        }

        public async Task<AdapterResult<LibroListItemModel>> CrearLibroAsync(Libro libro)
        {
            var resultado = await _libroService.CrearAsync(libro);
            if (!resultado.Success || resultado.Data == null)
            {
                return AdapterResult<LibroListItemModel>.Fail(resultado.Message ?? "No se pudo registrar el libro.");
            }

            return AdapterResult<LibroListItemModel>.Ok(Mapear(resultado.Data), resultado.Message ?? "Libro registrado correctamente.");
        }

        public async Task<AdapterResult<LibroListItemModel>> ActualizarLibroAsync(int id, Libro libro)
        {
            var resultado = await _libroService.ActualizarAsync(id, libro);
            if (!resultado.Success || resultado.Data == null)
            {
                return AdapterResult<LibroListItemModel>.Fail(resultado.Message ?? "No se pudo actualizar el libro.");
            }

            return AdapterResult<LibroListItemModel>.Ok(Mapear(resultado.Data), resultado.Message ?? "Libro actualizado correctamente.");
        }

        private static IList<LibroListItemModel> Mapear(IEnumerable<Libro> libros)
        {
            return libros
                .Select(Mapear)
                .ToList();
        }

        private static LibroListItemModel Mapear(Libro libro)
        {
            return new LibroListItemModel
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Autor = libro.Autor,
                ISBN = libro.ISBN,
                Editorial = libro.Editorial,
                AnioPublicacion = libro.AnioPublicacion,
                Categoria = libro.Categoria,
                Disponible = libro.Disponible,
                Activo = libro.Activo
            };
        }
    }
}