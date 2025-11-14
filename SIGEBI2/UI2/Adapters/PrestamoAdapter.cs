using System.Collections.Generic;
using System.Linq;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Entities;
using SIGEBI.Shared.Base;
using UI2.Models.Common;
using UI2.Models.Prestamos;

namespace UI2.Adapters
{
    public class PrestamoAdapter
    {
        private readonly IPrestamoService _prestamoService;

        public PrestamoAdapter(IPrestamoService prestamoService)
        {
            _prestamoService = prestamoService;
        }

        public async Task<AdapterResult<IList<PrestamoListItemModel>>> ObtenerPrestamosActivosAsync(int usuarioId)
        {
            var resultado = await _prestamoService.ObtenerPrestamosActivosPorUsuarioAsync(usuarioId);
            return ProcesarResultado(resultado, "No se encontraron préstamos activos para el usuario seleccionado.");
        }

        public async Task<AdapterResult<IList<PrestamoListItemModel>>> ObtenerPrestamosVencidosAsync()
        {
            var resultado = await _prestamoService.ObtenerPrestamosVencidosAsync();
            return ProcesarResultado(resultado, "No hay préstamos vencidos registrados.");
        }

        private static AdapterResult<IList<PrestamoListItemModel>> ProcesarResultado(ServiceResult<IEnumerable<Prestamo>> resultado, string mensajeError)
        {
            if (!resultado.Success || resultado.Data == null)
            {
                return AdapterResult<IList<PrestamoListItemModel>>.Fail(resultado.Message ?? mensajeError);
            }

            var lista = resultado.Data
                .Select(p => new PrestamoListItemModel
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    NombreUsuario = p.Usuario?.Nombre ?? string.Empty,
                    FechaPrestamo = p.FechaPrestamo,
                    FechaVencimiento = p.FechaVencimiento,
                    Activo = p.Activo,
                    Detalles = p.Detalles
                        .Select(d => new PrestamoDetalleItemModel
                        {
                            LibroId = d.LibroId,
                            TituloLibro = d.Libro?.Titulo ?? string.Empty,
                            Devuelto = d.Devuelto,
                            FechaDevolucion = d.FechaDevolucion
                        })
                        .ToList()
                })
                .ToList();

            return AdapterResult<IList<PrestamoListItemModel>>.Ok(lista, resultado.Message ?? "Préstamos cargados correctamente.");
        }
    }
}