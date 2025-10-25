using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.DTOs;
using SIGEBI.Application.Mappers; // 🔹 Importante: usar el mapper
using SIGEBI.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIGEBI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibroController : ControllerBase
    {
        private readonly ILibroService _libroService;

        public LibroController(ILibroService libroService)
        {
            _libroService = libroService;
        }

        // ============================================================
        // CU-01 Registrar libro
        // ============================================================
        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromBody] LibroDTO dto)
        {
            var libro = dto.ToEntity(); // 🔹 Usa el mapper en lugar de construir a mano
            var result = await _libroService.CrearAsync(libro);
            return result.Success
                ? Ok(result.Data!.ToDTO())
                : BadRequest(result);
        }

        // ============================================================
        // CU-02 Editar libro
        // ============================================================
        [HttpPut("actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] LibroDTO dto)
        {
            var libro = dto.ToEntity(); // 🔹 Conversión con mapper
            var result = await _libroService.ActualizarAsync(id, libro);
            return result.Success
                ? Ok(result.Data!.ToDTO())
                : NotFound(result);
        }

        // ============================================================
        // CU-03 Buscar libro
        // ============================================================
        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string criterio)
        {
            var result = await _libroService.BuscarAsync(criterio);
            if (!result.Success) return BadRequest(result);

            var lista = result.Data!.Select(l => l.ToDTO()).ToList(); // 🔹 Usa mapper
            return Ok(lista);
        }

        // ============================================================
        // CU-04 Consultar disponibilidad
        // ============================================================
        [HttpGet("{id:int}/disponibilidad")]
        public async Task<IActionResult> ConsultarDisponibilidad(int id)
        {
            var result = await _libroService.EstaDisponibleAsync(id);
            return Ok(result);
        }

        // ============================================================
        // Desactivar libro
        // ============================================================
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var result = await _libroService.DesactivarAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        // ============================================================
        // Obtener libro por Id
        // ============================================================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var result = await _libroService.ObtenerPorIdAsync(id);
            return result.Success && result.Data != null
                ? Ok(result.Data.ToDTO()) // 🔹 Usa mapper
                : NotFound(result);
        }

        // ============================================================
        // Listar todos los libros
        // ============================================================
        [HttpGet("listar")]
        public async Task<IActionResult> Listar()
        {
            var result = await _libroService.ListarAsync();
            if (!result.Success) return BadRequest(result);

            var lista = result.Data!.Select(l => l.ToDTO()).ToList(); // 🔹 Usa mapper
            return Ok(lista);
        }
    }
}
