using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
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

        [HttpGet("generar-hash/{password}")]
        public IActionResult GenerarHash(string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            return Ok(new { Password = password, Hash = hash });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequestDTO request)
        {
            var result = await _usuarioService.AutenticarAsync(request.Email, request.Password);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }
    }

}
