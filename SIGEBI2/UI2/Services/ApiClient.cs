using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace UI2.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly SessionService _sessionService;
        private readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public ApiClient(HttpClient httpClient, SessionService sessionService)
        {
            _httpClient = httpClient;
            _sessionService = sessionService;
        }

        public async Task<ApiResponse<TResponse>> SendAsync<TResponse>(HttpMethod method,
                                                                       string endpoint,
                                                                       object? body = null,
                                                                       bool requiresAuth = true,
                                                                       CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(method, endpoint);

            if (body != null)
            {
                var json = JsonSerializer.Serialize(body, _serializerOptions);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            if (requiresAuth && !TryAttachAuthorization(request))
            {
                return ApiResponse<TResponse>.Fail("No hay una sesión activa para comunicarse con la API.");
            }

            var response = await _httpClient.SendAsync(request, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return ApiResponse<TResponse>.Fail(ExtraerMensajeError(responseContent));
            }

            if (string.IsNullOrWhiteSpace(responseContent))
            {
                return ApiResponse<TResponse>.Ok(default);
            }

            try
            {
                var data = JsonSerializer.Deserialize<TResponse>(responseContent, _serializerOptions);
                if (data == null)
                {
                    return ApiResponse<TResponse>.Fail("La API devolvió una respuesta vacía.");
                }

                return ApiResponse<TResponse>.Ok(data);
            }
            catch (JsonException)
            {
                return ApiResponse<TResponse>.Fail("No se pudo interpretar la respuesta del servidor.");
            }
        }

        private bool TryAttachAuthorization(HttpRequestMessage request)
        {
            var token = _sessionService.AuthInfo?.Token;
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return true;
        }

        private static string ExtraerMensajeError(string contenido)
        {
            if (string.IsNullOrWhiteSpace(contenido))
            {
                return "La API devolvió un error inesperado.";
            }

            try
            {
                using var document = JsonDocument.Parse(contenido);
                if (document.RootElement.ValueKind == JsonValueKind.Object)
                {
                    if (document.RootElement.TryGetProperty("message", out var messageElement))
                    {
                        var mensaje = messageElement.GetString();
                        if (!string.IsNullOrWhiteSpace(mensaje))
                        {
                            return mensaje!;
                        }
                    }

                    if (document.RootElement.TryGetProperty("title", out var titleElement))
                    {
                        var mensaje = titleElement.GetString();
                        if (!string.IsNullOrWhiteSpace(mensaje))
                        {
                            return mensaje!;
                        }
                    }
                }
            }
            catch (JsonException)
            {
                // Ignorar errores de parseo y devolver el contenido crudo
            }

            return contenido;
        }
    }

    public class ApiResponse<T>
    {
        private ApiResponse(bool success, T? data, string message)
        {
            Success = success;
            Data = data;
            Message = message;
        }

        public bool Success { get; }
        public string Message { get; }
        public T? Data { get; }

        public static ApiResponse<T> Ok(T? data, string message = "") => new(true, data, message);
        public static ApiResponse<T> Fail(string message) => new(false, default, message);
    }
}
