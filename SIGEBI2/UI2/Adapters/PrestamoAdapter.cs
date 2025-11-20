using Application.DTOs;
using SIGEBI.Application.DTOs;
using System.Net.Http;
using UI2.Models.Common;
using UI2.Models.Prestamos;
using UI2.Services;

namespace UI2.Adapters
{
    public class PrestamoAdapter
    {
        private readonly ApiClient _apiClient;

        public PrestamoAdapter(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

]
        public async Task<AdapterResult<IList<PrestamoListItemModel>>> ObtenerTodosAsync()
        {
            var response = await _apiClient.SendAsync<List<PrestamoDTO>>(
                HttpMethod.Get,
                "/api/prestamo/listado"
            );

            if (!response.Success || response.Data == null)
                return AdapterResult<IList<PrestamoListItemModel>>.Fail("No se encontraron préstamos.");

            return AdapterResult<IList<PrestamoListItemModel>>.Ok(
                Mapear(response.Data),
                "Préstamos cargados correctamente."
            );
        }


        public async Task<AdapterResult<IList<PrestamoListItemModel>>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            var response = await _apiClient.SendAsync<List<PrestamoDTO>>(
                HttpMethod.Get,
                $"api/prestamo/usuario/{usuarioId}"
            );

            if (!response.Success || response.Data == null)
                return AdapterResult<IList<PrestamoListItemModel>>.Fail("No existen préstamos para este usuario.");

            return AdapterResult<IList<PrestamoListItemModel>>.Ok(
                Mapear(response.Data),
                "Préstamos del usuario cargados."
            );
        }

        public async Task<AdapterResult<IList<PrestamoListItemModel>>> ObtenerPrestamosActivosAsync(int usuarioId)
        {
            var response = await _apiClient.SendAsync<List<PrestamoDTO>>(
                HttpMethod.Get,
                $"api/prestamo/activos/{usuarioId}"
            );

            if (!response.Success || response.Data == null)
                return AdapterResult<IList<PrestamoListItemModel>>.Fail("No hay préstamos activos.");

            return AdapterResult<IList<PrestamoListItemModel>>.Ok(
                Mapear(response.Data),
                "Préstamos activos cargados."
            );
        }


        public async Task<AdapterResult<IList<PrestamoListItemModel>>> ObtenerPrestamosVencidosAsync()
        {
            var response = await _apiClient.SendAsync<List<PrestamoDTO>>(
                HttpMethod.Get,
                $"api/prestamo/vencidos"
            );

            if (!response.Success || response.Data == null)
                return AdapterResult<IList<PrestamoListItemModel>>.Fail("No hay vencidos.");

            return AdapterResult<IList<PrestamoListItemModel>>.Ok(
                Mapear(response.Data),
                "Préstamos vencidos cargados."
            );
        }

        public async Task<AdapterResult<IList<PrestamoDetalleItemModel>>> ObtenerDetallesAsync(int prestamoId)
        {
            var response = await _apiClient.SendAsync<List<DetallePrestamoDTO>>(
                HttpMethod.Get,
                $"api/prestamo/{prestamoId}/detalles"
            );

            if (!response.Success || response.Data == null)
                return AdapterResult<IList<PrestamoDetalleItemModel>>.Fail("No se encontraron detalles.");

            var detalles = response.Data.Select(d => new PrestamoDetalleItemModel
            {
                LibroId = d.LibroId,
                TituloLibro = d.TituloLibro,
                Devuelto = d.Devuelto,
                FechaDevolucion = d.FechaDevolucion
            }).ToList();

            return AdapterResult<IList<PrestamoDetalleItemModel>>.Ok(
                detalles,
                "Detalles cargados correctamente."
            );
        }


        public async Task<AdapterResult<PrestamoListItemModel>> RegistrarPrestamoAsync(PrestamoCreateModel model)
        {
            var response = await _apiClient.SendAsync<PrestamoDTO>(
                HttpMethod.Post,
                "api/prestamo/registrar",
                model
            );

            if (!response.Success || response.Data == null)
                return AdapterResult<PrestamoListItemModel>.Fail("No se pudo registrar el préstamo.");

            return AdapterResult<PrestamoListItemModel>.Ok(
                Mapear(response.Data),
                "Préstamo registrado correctamente."
            );
        }


        public async Task<AdapterResult<string>> RegistrarDevolucionAsync(int prestamoId, List<int> librosIds)
        {
            var response = await _apiClient.SendAsync<string>(
                HttpMethod.Put,
                $"api/prestamo/{prestamoId}/devolucion",
                librosIds
            );

            return response.Success
                ? AdapterResult<string>.Ok("Préstamo devuelto con éxito.")
                : AdapterResult<string>.Fail("No se pudo devolver el préstamo.");
        }


        public async Task<AdapterResult<string>> RegistrarDevolucionDetalleAsync(int detalleId)
        {
            var response = await _apiClient.SendAsync<string>(
                HttpMethod.Put,
                $"api/prestamo/detalle/{detalleId}/devolver"
            );

            return response.Success
                ? AdapterResult<string>.Ok("Libro devuelto correctamente.")
                : AdapterResult<string>.Fail("No se pudo devolver el libro.");
        }


        private PrestamoListItemModel Mapear(PrestamoDTO p)
        {
            return new PrestamoListItemModel
            {
                Id = p.Id,
                UsuarioId = p.UsuarioId,
                NombreUsuario = p.NombreUsuario,
                FechaPrestamo = p.FechaPrestamo,
                FechaVencimiento = p.FechaVencimiento,
                Activo = p.Activo,
                Detalles = p.Detalles.Select(d => new PrestamoDetalleItemModel
                {
                    LibroId = d.LibroId,
                    TituloLibro = d.TituloLibro,
                    Devuelto = d.Devuelto,
                    FechaDevolucion = d.FechaDevolucion

                }).ToList()
            };
        }

        private IList<PrestamoListItemModel> Mapear(IEnumerable<PrestamoDTO> prestamos)
        {
            return prestamos.Select(Mapear).ToList();
        }
    }
}

