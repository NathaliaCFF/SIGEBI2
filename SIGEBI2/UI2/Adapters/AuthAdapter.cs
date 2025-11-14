using Application.DTOs;
using SIGEBI.Application.Interfaces;
using UI2.Models.Auth;

namespace UI2.Adapters
{
    public class AuthAdapter
    {
        private readonly IUsuarioService _usuarioService;

        public AuthAdapter(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public async Task<LoginResultModel> LoginAsync(LoginRequestModel request)
        {
            var resultado = await _usuarioService.AutenticarAsync(request.Email, request.Password);

            if (!resultado.Success || resultado.Data == null)
            {
                return LoginResultModel.Failure(resultado.Message ?? "No fue posible iniciar sesión.");
            }

            return LoginResultModel.Successful(resultado.Data);
        }
    }
}