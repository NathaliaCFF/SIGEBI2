using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public AuthController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }


        /// Utilidad para generar un hash usando BCrypt.

        [HttpGet("generar-hash/{password}")]
        public IActionResult GenerarHash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return BadRequest(new { Success = false, Message = "La contraseña no puede estar vacía." });

            string hash = BCrypt.Net.BCrypt.HashPassword(password);

            return Ok(new
            {
                Success = true,
                Password = password,
                Hash = hash
            });
        }

        /// Autentica un usuario y genera un token (flujo login).

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequestDTO request)
        {
            if (request == null)
                return BadRequest(new { Success = false, Message = "La solicitud es inválida." });

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { Success = false, Message = "Email y password son obligatorios." });

            var result = await _usuarioService.AutenticarAsync(request.Email, request.Password);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }
    }
}
