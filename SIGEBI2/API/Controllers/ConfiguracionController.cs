using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Mappers;

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


        /// Obtiene la configuración actual del sistema (días de préstamo, etc.)

        [HttpGet]
        public async Task<IActionResult> ObtenerConfiguracion()
        {
            try
            {
                var result = await _service.ObtenerConfiguracionAsync();

                if (!result.Success || result.Data == null)
                    return NotFound(new { Success = false, Message = result.Message });

                return Ok(new
                {
                    Success = true,
                    Message = "Configuración cargada correctamente.",
                    Data = result.Data.ToDTO()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }


        /// Actualiza la duración máxima del préstamo en días.

        [HttpPut("duracion/{dias:int}")]
        public async Task<IActionResult> ActualizarDuracionPrestamo(int dias)
        {
            try
            {
                if (dias <= 0)
                    return BadRequest(new { Success = false, Message = "Los días deben ser mayores que cero." });

                var result = await _service.ActualizarDuracionPrestamoDiasAsync(dias);

                if (!result.Success)
                    return BadRequest(new { Success = false, Message = result.Message });

                return Ok(new
                {
                    Success = true,
                    Message = "Duración del préstamo actualizada correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }
    }
}
