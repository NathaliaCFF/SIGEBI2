using System.Net.Http.Json;
using Application.DTOs;
using UI2.Models.Auth;
using UI2.Models.Common;

namespace UI2.Adapters
{
    public class AuthAdapter
    {
        private readonly HttpClient _http;

        public AuthAdapter(HttpClient http)
        {
            _http = http;
        }

        public async Task<LoginResultModel> LoginAsync(AuthRequestDTO request)
        {
            var response = await _http.PostAsJsonAsync("auth/login", request);

            if (!response.IsSuccessStatusCode)
                return LoginResultModel.Failure("Credenciales inválidas.");

            var data = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();

            return LoginResultModel.Successful(data!);
        }
    }
}
