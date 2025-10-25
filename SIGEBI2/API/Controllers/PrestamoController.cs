using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Mappers;
using SIGEBI.Application.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIGEBI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamoController : ControllerBase
    {
        private readonly IPrestamoService _prestamoService;

        public PrestamoController(IPrestamoService prestamoService)
        {
            _prestamoService = prestamoService;
        }


        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarPrestamo([FromBody] PrestamoRequest request)
        {
            var result = await _prestamoService.RegistrarPrestamoAsync(request.UsuarioId, request.LibrosIds);

            if (!result.Success || result.Data == null)
                return BadRequest(result);

            var prestamoDto = result.Data.ToDTO();
            return Ok(prestamoDto);
        }

        [HttpPut("{prestamoId}/devolucion")]
        public async Task<IActionResult> RegistrarDevolucion(int prestamoId, [FromBody] List<int> librosIds)
        {
            var result = await _prestamoService.RegistrarDevolucionAsync(prestamoId, librosIds);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpGet("activos/{usuarioId}")]
        public async Task<IActionResult> ObtenerActivosPorUsuario(int usuarioId)
        {
            var result = await _prestamoService.ObtenerPrestamosActivosPorUsuarioAsync(usuarioId);

            if (!result.Success || result.Data == null)
                return NotFound(result.Message);

            
            var prestamosDto = result.Data.Select(p => p.ToDTO()).ToList();
            return Ok(prestamosDto);
        }

        [HttpGet("vencidos")]
        public async Task<IActionResult> ObtenerVencidos()
        {
            var result = await _prestamoService.ObtenerPrestamosVencidosAsync();

            if (!result.Success || result.Data == null)
                return NotFound(result.Message);

            var vencidosDto = result.Data.Select(p => p.ToDTO()).ToList();
            return Ok(vencidosDto);
        }
    }

    
    public class PrestamoRequest
    {
        public int UsuarioId { get; set; }
        public List<int> LibrosIds { get; set; } = new();
    }
}

