using System.Net.Http;
using Application.DTOs;
using Shared;
using UI2.Models.Auth;
using UI2.Services;

namespace UI2.Adapters
{
    public class AuthAdapter
    {
        private readonly ApiClient _apiClient;

        public AuthAdapter(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<LoginResultModel> LoginAsync(LoginRequestModel request)
        {
            var response = await _apiClient.SendAsync<OperationResult<AuthResponseDTO>>(HttpMethod.Post,
                                                                                       "api/auth/login",
                                                                                       new AuthRequestDTO
                                                                                       {
                                                                                           Email = request.Email,
                                                                                           Password = request.Password
                                                                                       },
                                                                                       requiresAuth: false);

            if (!response.Success || response.Data == null)
            {
                var mensajeApi = string.IsNullOrWhiteSpace(response.Message)
                    ? "No fue posible iniciar sesión."
                    : response.Message;
                return LoginResultModel.Failure(mensajeApi);
            }

            var resultado = response.Data;
            if (!resultado.Success || resultado.Data == null)
            {
                return LoginResultModel.Failure(resultado.Message ?? "No fue posible iniciar sesión.");
            }

            return LoginResultModel.Successful(resultado.Data);
        }
    }
}
