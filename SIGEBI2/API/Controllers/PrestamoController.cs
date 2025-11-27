using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Mappers;
using SIGEBI.Application.DTOs;

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
        public async Task<ActionResult<PrestamoDTO>> RegistrarPrestamo([FromBody] PrestamoRequest request)
        {
            if (request == null)
                return BadRequest("Solicitud inválida.");

            if (request.UsuarioId <= 0)
                return BadRequest("Debe indicar un usuario válido.");

            if (request.LibrosIds == null || !request.LibrosIds.Any())
                return BadRequest("Debe seleccionar al menos un libro.");

            var result = await _prestamoService.RegistrarPrestamoAsync(request.UsuarioId, request.LibrosIds);

            if (!result.Success || result.Data == null)
                return BadRequest(result.Message);

            return Ok(result.Data.ToDTO());
        }

        [HttpPut("{prestamoId}/devolucion")]
        public async Task<ActionResult<string>> RegistrarDevolucion(int prestamoId, [FromBody] List<int> librosIds)
        {
            if (prestamoId <= 0)
                return BadRequest("El préstamo no es válido.");

            if (librosIds == null || !librosIds.Any())
                return BadRequest("Debe especificar los libros a devolver.");

            var result = await _prestamoService.RegistrarDevolucionAsync(prestamoId, librosIds);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok("Devolución registrada correctamente.");
        }

        // Préstamos activos por usuario -> devuelve directamente List<PrestamoDTO>
        [HttpGet("activos/{usuarioId}")]
        public async Task<ActionResult<List<PrestamoDTO>>> ObtenerActivosPorUsuario(int usuarioId)
        {
            if (usuarioId <= 0)
                return BadRequest("Usuario inválido.");

            var result = await _prestamoService.ObtenerPrestamosActivosPorUsuarioAsync(usuarioId);

            if (!result.Success || result.Data == null)
                return Ok(new List<PrestamoDTO>());

            var lista = result.Data.Select(p => p.ToDTO()).ToList();
            return Ok(lista);
        }

        // Préstamos vencidos -> devuelve directamente List<PrestamoDTO>
        [HttpGet("vencidos")]
        public async Task<ActionResult<List<PrestamoDTO>>> ObtenerVencidos()
        {
            var result = await _prestamoService.ObtenerPrestamosVencidosAsync();

            if (!result.Success || result.Data == null)
                return Ok(new List<PrestamoDTO>());

            var lista = result.Data.Select(p => p.ToDTO()).ToList();
            return Ok(lista);
        }
    }

    public class PrestamoRequest
    {
        public int UsuarioId { get; set; }
        public List<int> LibrosIds { get; set; } = new();
    }
}
