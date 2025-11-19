using Application.DTOs;

namespace UI2.Models.Auth
{
    public class LoginResultModel
    {
        private LoginResultModel() { }

        public bool Success { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public AuthResponseDTO? Data { get; private set; }

        public static LoginResultModel Successful(AuthResponseDTO data)
        {
            return new LoginResultModel
            {
                Success = true,
                Data = data
            };
        }

        public static LoginResultModel Failure(string message)
        {
            return new LoginResultModel
            {
                Success = false,
                Message = message
            };
        }
    }
}
