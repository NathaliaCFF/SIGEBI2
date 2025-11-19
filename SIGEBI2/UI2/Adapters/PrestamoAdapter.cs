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

        public async Task<AdapterResult<IList<PrestamoListItemModel>>> ObtenerPrestamosActivosAsync(int usuarioId)
        {
            var response = await _apiClient.SendAsync<List<PrestamoDTO>>(HttpMethod.Get,
                                                                         $"api/prestamo/activos/{usuarioId}");

            if (!response.Success || response.Data == null)
            {
                return AdapterResult<IList<PrestamoListItemModel>>.Fail(response.Message ?? "No se encontraron préstamos activos para el usuario seleccionado.");
            }

            return AdapterResult<IList<PrestamoListItemModel>>.Ok(Mapear(response.Data), "Préstamos cargados correctamente.");
        }

        public async Task<AdapterResult<IList<PrestamoListItemModel>>> ObtenerPrestamosVencidosAsync()
        {
            var response = await _apiClient.SendAsync<List<PrestamoDTO>>(HttpMethod.Get,
                                                                         "api/prestamo/vencidos");

            if (!response.Success || response.Data == null)
            {
                return AdapterResult<IList<PrestamoListItemModel>>.Fail(response.Message ?? "No hay préstamos vencidos registrados.");
            }

            return AdapterResult<IList<PrestamoListItemModel>>.Ok(Mapear(response.Data), "Préstamos cargados correctamente.");
        }

        private static IList<PrestamoListItemModel> Mapear(IEnumerable<PrestamoDTO> prestamos)
        {
            return prestamos
                .Select(p => new PrestamoListItemModel
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    NombreUsuario = p.NombreUsuario,
                    FechaPrestamo = p.FechaPrestamo,
                    FechaVencimiento = p.FechaVencimiento,
                    Activo = p.Activo,
                    Detalles = p.Detalles
                        .Select(d => new PrestamoDetalleItemModel
                        {
                            LibroId = d.LibroId,
                            TituloLibro = d.TituloLibro,
                            Devuelto = d.Devuelto,
                            FechaDevolucion = d.FechaDevolucion
                        })
                        .ToList()
                })
                .ToList();
        }
    }
}

