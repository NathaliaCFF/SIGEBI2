using SIGEBI.Application.Interfaces;
using SIGEBI.Shared.Base;

namespace SIGEBI.Application.Buses.Configuration
{
    public class UpdateConfiguracionHandler
    {
        private readonly IConfiguracionService _service;

        public UpdateConfiguracionHandler(IConfiguracionService service)
        {
            _service = service;
        }

        public Task<ServiceResult> Handle(UpdateConfiguracionCommand cmd)
        {
            return _service.ActualizarDuracionPrestamoDiasAsync(cmd.Dias);
        }
    }
}
