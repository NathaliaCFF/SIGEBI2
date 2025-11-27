using UI2.Models.Auth;

namespace UI2.Services.Interfaces
{
    public interface IAuthApiService
    {
        Task<LoginResultModel> LoginAsync(LoginRequestModel request);
    }
}
