using Application.DTOs;
using Shared;
using UI2.Models.Auth;
using UI2.Services.Interfaces;

namespace UI2.Services.Implementations
{
    public class AuthApiService : IAuthApiService
    {
        private readonly ApiClient _apiClient;

        public AuthApiService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<LoginResultModel> LoginAsync(LoginRequestModel request)
        {
            var response = await _apiClient.SendAsync<OperationResult<AuthResponseDTO>>(
                HttpMethod.Post,
                "/api/auth/login",
                new AuthRequestDTO
                {
                    Email = request.Email,
                    Password = request.Password
                },
                requiresAuth: false
            );

            if (!response.Success || response.Data == null)
            {
                var mensaje = string.IsNullOrWhiteSpace(response.Message)
                    ? "No fue posible iniciar sesión."
                    : response.Message;

                return LoginResultModel.Failure(mensaje);
            }

            var resultado = response.Data;
            if (!resultado.Success || resultado.Data == null)
            {
                return LoginResultModel.Failure(
                    resultado.Message ?? "No fue posible iniciar sesión."
                );
            }

            return LoginResultModel.Successful(resultado.Data);
        }
    }
}
