using Shared;
using SIGEBI.Application.DTOs;
using SIGEBI.Domain.Entities;
using UI2.Models.Common;
using UI2.Models.Libros;
using UI2.Services;

namespace UI2.Adapters
{
    public class LibroAdapter
    {
        private readonly ApiClient _apiClient;

        public LibroAdapter(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<AdapterResult<IList<LibroListItemModel>>> ListarLibrosAsync()
        {
            var response = await _apiClient.SendAsync<List<LibroDTO>>(HttpMethod.Get,
                                                                      "/api/libro/listar",
                                                                      requiresAuth: false);

            if (!response.Success || response.Data == null)
            {
                return AdapterResult<IList<LibroListItemModel>>.Fail(response.Message ?? "No se pudieron obtener los libros.");
            }

            return AdapterResult<IList<LibroListItemModel>>.Ok(Mapear(response.Data), "Libros cargados correctamente.");
        }

        public async Task<AdapterResult<bool>> EliminarLibroAsync(int id)
        {

            var response = await _apiClient.SendAsync<OperationResult<bool>>(
                HttpMethod.Delete,
                $"api/libro/{id}",
                requiresAuth: false
            );

            if (!response.Success || response.Data == null)
            {
                return AdapterResult<bool>.Fail(
                    response.Message ?? "No se pudo eliminar el libro."
                );
            }


            if (!response.Data.Success)
            {
                return AdapterResult<bool>.Fail(
                    response.Data.Message ?? "La API indicó un fallo al eliminar."
                );
            }

            return AdapterResult<bool>.Ok(true, "Libro eliminado correctamente.");
        }



        public async Task<AdapterResult<IList<LibroListItemModel>>> BuscarLibrosAsync(string criterio)
        {
            var response = await _apiClient.SendAsync<List<LibroDTO>>(HttpMethod.Get,
                                                                      $"api/libro/buscar?criterio={Uri.EscapeDataString(criterio)}",
                                                                      requiresAuth: false);

            if (!response.Success || response.Data == null)
            {
                return AdapterResult<IList<LibroListItemModel>>.Fail(response.Message ?? "No se encontraron libros.");
            }

            return AdapterResult<IList<LibroListItemModel>>.Ok(Mapear(response.Data), "Búsqueda completada.");
        }

        public async Task<AdapterResult<LibroListItemModel>> CrearLibroAsync(Libro libro)
        {
            var dto = MapearDto(libro);
            var response = await _apiClient.SendAsync<LibroDTO>(HttpMethod.Post,
                                                                "api/libro/crear",
                                                                dto);

            if (!response.Success || response.Data == null)
            {
                return AdapterResult<LibroListItemModel>.Fail(response.Message ?? "No se pudo registrar el libro.");
            }

            return AdapterResult<LibroListItemModel>.Ok(Mapear(response.Data), "Libro registrado correctamente.");
        }

        public async Task<AdapterResult<LibroListItemModel>> ActualizarLibroAsync(int id, SIGEBI.Domain.Entities.Libro libro)
        {

            var response = await _apiClient.SendAsync<LibroDTO>(
                HttpMethod.Put,
                $"/api/libro/actualizar/{id}",
                libro
            );

            if (!response.Success || response.Data == null)
            {
                return AdapterResult<LibroListItemModel>.Fail(
                    response.Message ?? "No se pudo actualizar el libro."
                );
            }

            var dto = response.Data;


            var model = new LibroListItemModel
            {
                Id = dto.Id,
                Titulo = dto.Titulo,
                Autor = dto.Autor,
                ISBN = dto.ISBN,
                Editorial = dto.Editorial,
                AnioPublicacion = dto.AnioPublicacion,
                Categoria = dto.Categoria,
                Disponible = dto.Disponible,
                Activo = dto.Activo
            };

            return AdapterResult<LibroListItemModel>.Ok(
                model,
                "Libro actualizado correctamente."
            );
        }



        private static LibroDTO MapearDto(Libro libro)
        {
            return new LibroDTO
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

        private static IList<LibroListItemModel> Mapear(IEnumerable<LibroDTO> libros)
        {
            return libros
                .Select(Mapear)
                .ToList();
        }

        private static LibroListItemModel Mapear(LibroDTO libro)
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

