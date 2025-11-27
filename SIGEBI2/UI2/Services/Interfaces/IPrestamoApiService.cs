using UI2.Models.Common;
using UI2.Models.Prestamos;

namespace UI2.Services.Interfaces
{
    public interface IPrestamoApiService
    {
        Task<AdapterResult<IList<PrestamoListItemModel>>> ObtenerTodosAsync();
        Task<AdapterResult<IList<PrestamoListItemModel>>> ObtenerPorUsuarioAsync(int usuarioId);
        Task<AdapterResult<IList<PrestamoListItemModel>>> ObtenerPrestamosActivosAsync(int usuarioId);
        Task<AdapterResult<IList<PrestamoListItemModel>>> ObtenerPrestamosVencidosAsync();
        Task<AdapterResult<IList<PrestamoDetalleItemModel>>> ObtenerDetallesAsync(int prestamoId);

        Task<AdapterResult<PrestamoListItemModel>> RegistrarPrestamoAsync(PrestamoCreateModel model);

        Task<AdapterResult<string>> RegistrarDevolucionAsync(int prestamoId, List<int> librosIds);
        Task<AdapterResult<string>> RegistrarDevolucionDetalleAsync(int detalleId);
    }
}
