using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Entities;
using System.Security.Claims;

namespace SIGEBI.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }


        // Método auxiliar para obtener usuario autenticado

        private Usuario ObtenerUsuarioActual()
        {
            return new Usuario
            {
                Nombre = User.FindFirst(ClaimTypes.Name)?.Value ?? "Desconocido",
                Email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "",
                Rol = User.FindFirst(ClaimTypes.Role)?.Value ?? "Usuario",
                Activo = true
            };
        }


        // Obtener todos los usuarios (solo Administrador)

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var result = await _usuarioService.ObtenerTodosAsync(ObtenerUsuarioActual());

                return Ok(new
                {
                    Success = result.Success,
                    Message = result.Message,
                    Data = result.Data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }


        // Crear usuario

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Usuario usuario)
        {
            try
            {
                if (usuario == null)
                    return BadRequest(new { Success = false, Message = "El cuerpo de la solicitud es inválido." });

                var result = await _usuarioService.CrearAsync(usuario, ObtenerUsuarioActual());

                return Ok(new
                {
                    Success = result.Success,
                    Message = result.Message,
                    Data = result.Data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }


        // Actualizar usuario

        [Authorize(Roles = "Administrador")]
        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] Usuario usuario)
        {
            try
            {
                if (usuario == null)
                    return BadRequest(new { Success = false, Message = "El cuerpo de la solicitud es inválido." });

                var result = await _usuarioService.ActualizarAsync(usuario, ObtenerUsuarioActual());

                return Ok(new
                {
                    Success = result.Success,
                    Message = result.Message,
                    Data = result.Data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }


        // Activar usuario

        [Authorize(Roles = "Administrador")]
        [HttpPatch("{id}/activar")]
        public async Task<IActionResult> Activar(int id)
        {
            try
            {
                var result = await _usuarioService.ActivarAsync(id, ObtenerUsuarioActual());

                return Ok(new
                {
                    Success = result.Success,
                    Message = result.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }


        // Desactivar usuario

        [Authorize(Roles = "Administrador")]
        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> Desactivar(int id)
        {
            try
            {
                var result = await _usuarioService.DesactivarAsync(id, ObtenerUsuarioActual());

                return Ok(new
                {
                    Success = result.Success,
                    Message = result.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }
    }
}
