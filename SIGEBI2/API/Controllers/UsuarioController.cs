using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Usuario;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Mappers;
using SIGEBI.Domain.Entities;

namespace SIGEBI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var usuarioActual = new Usuario
            {
                Nombre = User.FindFirst("nombre")?.Value ?? "Desconocido",
                Email = User.Identity?.Name ?? "",
                Rol = User.FindFirst("rol")?.Value ?? "Usuario",
                Activo = true
            };

            var result = await _usuarioService.ObtenerTodosAsync(usuarioActual);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Usuario usuario)
        {
            var usuarioActual = new Usuario
            {
                Nombre = User.FindFirst("nombre")?.Value ?? "Desconocido",
                Email = User.Identity?.Name ?? "",
                Rol = User.FindFirst("rol")?.Value ?? "Usuario",
                Activo = true
            };

            var result = await _usuarioService.CrearAsync(usuario, usuarioActual);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] Usuario usuario)
        {
            var usuarioActual = new Usuario
            {
                Nombre = User.FindFirst("nombre")?.Value ?? "Desconocido",
                Email = User.Identity?.Name ?? "",
                Rol = User.FindFirst("rol")?.Value ?? "Usuario",
                Activo = true
            };

            var result = await _usuarioService.ActualizarAsync(usuario, usuarioActual);
            return Ok(result);
        }

        [HttpPatch("{id}/activar")]
        public async Task<IActionResult> Activar(int id)
        {
            var usuarioActual = new Usuario
            {
                Nombre = User.FindFirst("nombre")?.Value ?? "Desconocido",
                Email = User.Identity?.Name ?? "",
                Rol = User.FindFirst("rol")?.Value ?? "Usuario",
                Activo = true
            };

            var result = await _usuarioService.ActivarAsync(id, usuarioActual);
            return Ok(result);
        }

        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var usuarioActual = new Usuario
            {
                Nombre = User.FindFirst("nombre")?.Value ?? "Desconocido",
                Email = User.Identity?.Name ?? "",
                Rol = User.FindFirst("rol")?.Value ?? "Usuario",
                Activo = true
            };

            var result = await _usuarioService.DesactivarAsync(id, usuarioActual);
            return Ok(result);
        }
    }

}
