using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Mappers;
using SIGEBI.Application.DTOs;
using System.Threading.Tasks;

namespace SIGEBI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfiguracionController : ControllerBase
    {
        private readonly IConfiguracionService _service;

        public ConfiguracionController(IConfiguracionService service)
        {
            _service = service;
        }

        // ============================================================
        // CU-14: Obtener configuración del sistema
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> ObtenerConfiguracion()
        {
            var result = await _service.ObtenerConfiguracionAsync();
            if (!result.Success || result.Data == null)
                return NotFound(result.Message);

            return Ok(result.Data.ToDTO());
        }

        // ============================================================
        // CU-15: Actualizar duración del préstamo
        // ============================================================
        [HttpPut("duracion/{dias:int}")]
        public async Task<IActionResult> ActualizarDuracionPrestamo(int dias)
        {
            var result = await _service.ActualizarDuracionPrestamoDiasAsync(dias);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }
    }
}
