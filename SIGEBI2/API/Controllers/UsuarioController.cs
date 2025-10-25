using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Entities;
using System.Security.Claims;

namespace SIGEBI.API.Controllers
{
    [Authorize] // Todos los métodos requieren autenticación
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // Método auxiliar para obtener el usuario autenticado
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

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await _usuarioService.ObtenerTodosAsync(ObtenerUsuarioActual());
            return Ok(result);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Usuario usuario)
        {
            var result = await _usuarioService.CrearAsync(usuario, ObtenerUsuarioActual());
            return Ok(result);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] Usuario usuario)
        {
            var result = await _usuarioService.ActualizarAsync(usuario, ObtenerUsuarioActual());
            return Ok(result);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPatch("{id}/activar")]
        public async Task<IActionResult> Activar(int id)
        {
            var result = await _usuarioService.ActivarAsync(id, ObtenerUsuarioActual());
            return Ok(result);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var result = await _usuarioService.DesactivarAsync(id, ObtenerUsuarioActual());
            return Ok(result);
        }
    }
}
