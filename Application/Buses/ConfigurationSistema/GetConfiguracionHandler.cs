using SIGEBI.Application.Interfaces;
using SIGEBI.Shared.Base;
using DomainConfig = SIGEBI.Domain.Entities.Configuration;


namespace SIGEBI.Application.Buses.Configuration
{
    public class GetConfiguracionHandler
    {
        private readonly IConfiguracionService _service;

        public GetConfiguracionHandler(IConfiguracionService service)
        {
            _service = service;
        }

        public Task<ServiceResult<DomainConfig>> Handle()
        {
            return _service.ObtenerConfiguracionAsync();
        }

    }
}
