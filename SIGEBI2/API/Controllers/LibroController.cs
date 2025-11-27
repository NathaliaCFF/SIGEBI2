using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Mappers;

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


        // Crear libro

        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromBody] LibroDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { Success = false, Message = "El cuerpo de la solicitud es inválido." });

                var libro = dto.ToEntity();
                var result = await _libroService.CrearAsync(libro);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(new
                {
                    Success = true,
                    Message = "Libro registrado correctamente.",
                    Data = result.Data!.ToDTO()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }


        // Actualizar libro

        [HttpPut("actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] LibroDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { Success = false, Message = "El cuerpo de la solicitud es inválido." });

                var libro = dto.ToEntity();
                var result = await _libroService.ActualizarAsync(id, libro);

                if (!result.Success)
                    return NotFound(result);

                return Ok(new
                {
                    Success = true,
                    Message = "Libro actualizado correctamente.",
                    Data = result.Data!.ToDTO()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }


        // Buscar libros

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string criterio)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(criterio))
                    return BadRequest(new { Success = false, Message = "Debe especificar un criterio de búsqueda." });

                var result = await _libroService.BuscarAsync(criterio);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(new
                {
                    Success = true,
                    Data = result.Data!.Select(l => l.ToDTO()).ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }


        // Consultar disponibilidad

        [HttpGet("{id:int}/disponibilidad")]
        public async Task<IActionResult> ConsultarDisponibilidad(int id)
        {
            try
            {
                var result = await _libroService.EstaDisponibleAsync(id);
                return Ok(new
                {
                    Success = result.Success,
                    Message = result.Message,
                    Disponible = result.Data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }

        // Desactivar libro

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Desactivar(int id)
        {
            try
            {
                var result = await _libroService.DesactivarAsync(id);

                if (!result.Success)
                    return NotFound(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }

        [HttpPut("{id:int}/activar")]
        public async Task<IActionResult> Activar(int id)
        {
            var result = await _libroService.ActivarAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(new
            {
                Success = true,
                Message = "Libro activado correctamente."
            });
        }


        // Obtener libro por Id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var result = await _libroService.ObtenerPorIdAsync(id);

                if (!result.Success || result.Data == null)
                    return NotFound(result);

                return Ok(new
                {
                    Success = true,
                    Data = result.Data.ToDTO()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }


        // Listar todos los libros
        [HttpGet("listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var result = await _libroService.ListarAsync();

                if (!result.Success)
                    return BadRequest(result);

                return Ok(new
                {
                    Success = true,
                    Data = result.Data!.Select(l => l.ToDTO()).ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }
    }
}
