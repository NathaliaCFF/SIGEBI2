using Shared;
using SIGEBI.Application.DTOs;
using SIGEBI.Domain.Entities;
using UI2.Models.Common;
using UI2.Models.Libros;
using UI2.Services.Interfaces;

namespace UI2.Services.Implementations
{
    public class LibroApiService : ILibroApiService
    {
        private readonly ApiClient _apiClient;

        public LibroApiService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        // LISTAR LIBROS

        public async Task<AdapterResult<IList<LibroListItemModel>>> ListarLibrosAsync()
        {
            var response = await _apiClient.SendAsync<OperationResult<List<LibroDTO>>>(
                HttpMethod.Get,
                "/api/libro/listar",
                requiresAuth: false
            );

            if (!response.Success || response.Data == null)
                return AdapterResult<IList<LibroListItemModel>>.Fail(
                    response.Message ?? "No se pudo obtener la lista de libros."
                );

            var resultado = response.Data;

            if (!resultado.Success || resultado.Data == null)
                return AdapterResult<IList<LibroListItemModel>>.Fail(
                    resultado.Message ?? "Error al obtener datos."
                );

            var lista = MapearLista(resultado.Data);

            return AdapterResult<IList<LibroListItemModel>>.Ok(lista, resultado.Message);
        }


        // BUSCAR LIBROS

        public async Task<AdapterResult<IList<LibroListItemModel>>> BuscarLibrosAsync(string criterio)
        {
            var response = await _apiClient.SendAsync<OperationResult<List<LibroDTO>>>(
                HttpMethod.Get,
                $"api/libro/buscar?criterio={Uri.EscapeDataString(criterio)}",
                requiresAuth: false
            );

            if (!response.Success || response.Data == null)
                return AdapterResult<IList<LibroListItemModel>>.Fail(
                    response.Message ?? "No se pudo procesar la búsqueda."
                );

            var resultado = response.Data;

            if (!resultado.Success || resultado.Data == null)
                return AdapterResult<IList<LibroListItemModel>>.Fail(
                    resultado.Message ?? "Sin resultados."
                );

            return AdapterResult<IList<LibroListItemModel>>.Ok(
                MapearLista(resultado.Data),
                resultado.Message
            );
        }


        // CREAR LIBRO

        public async Task<AdapterResult<LibroListItemModel>> CrearLibroAsync(Libro libro)
        {
            var dto = MapearDto(libro);

            var response = await _apiClient.SendAsync<OperationResult<LibroDTO>>(
                HttpMethod.Post,
                "api/libro/crear",
                dto
            );

            if (!response.Success || response.Data == null)
                return AdapterResult<LibroListItemModel>.Fail(
                    response.Message ?? "No se pudo registrar el libro."
                );

            var resultado = response.Data;

            if (!resultado.Success || resultado.Data == null)
                return AdapterResult<LibroListItemModel>.Fail(
                    resultado.Message ?? "Error al procesar la creación."
                );

            return AdapterResult<LibroListItemModel>.Ok(
                Mapear(resultado.Data),
                resultado.Message
            );
        }

        // ACTUALIZAR LIBRO

        public async Task<AdapterResult<LibroListItemModel>> ActualizarLibroAsync(int id, Libro libro)
        {
            var response = await _apiClient.SendAsync<OperationResult<LibroDTO>>(
                HttpMethod.Put,
                $"api/libro/actualizar/{id}",
                libro
            );

            if (!response.Success || response.Data == null)
                return AdapterResult<LibroListItemModel>.Fail(
                    response.Message ?? "No se pudo actualizar el libro."
                );

            var resultado = response.Data;

            if (!resultado.Success || resultado.Data == null)
                return AdapterResult<LibroListItemModel>.Fail(
                    resultado.Message ?? "Error al procesar la actualización."
                );

            return AdapterResult<LibroListItemModel>.Ok(
                Mapear(resultado.Data),
                resultado.Message
            );
        }

        // ELIMINAR LIBRO

        public async Task<AdapterResult<bool>> EliminarLibroAsync(int id)
        {
            var response = await _apiClient.SendAsync<OperationResult<bool>>(
                HttpMethod.Delete,
                $"api/libro/{id}",
                requiresAuth: false
            );

            if (!response.Success || response.Data == null)
                return AdapterResult<bool>.Fail(
                    response.Message ?? "No se pudo eliminar el libro."
                );

            var resultado = response.Data;

            if (!resultado.Success)
                return AdapterResult<bool>.Fail(
                    resultado.Message ?? "La API indicó un fallo al eliminar."
                );

            return AdapterResult<bool>.Ok(true, resultado.Message ?? "Libro eliminado correctamente.");
        }


        // ACTIVAR LIBRO

        public async Task<AdapterResult<bool>> ActivarLibroAsync(int id)
        {
            var response = await _apiClient.SendAsync<OperationResult<bool>>(
                HttpMethod.Put,
                $"api/libro/{id}/activar",
                requiresAuth: false
            );

            if (!response.Success || response.Data == null)
                return AdapterResult<bool>.Fail(
                    response.Message ?? "No se pudo activar el libro."
                );

            var resultado = response.Data;

            if (!resultado.Success)
                return AdapterResult<bool>.Fail(
                    resultado.Message ?? "Error al activar el libro."
                );

            return AdapterResult<bool>.Ok(true, resultado.Message ?? "Libro activado correctamente.");
        }


        // MAPEOS
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

        private static IList<LibroListItemModel> MapearLista(IEnumerable<LibroDTO> libros)
        {
            return libros.Select(Mapear).ToList();
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
