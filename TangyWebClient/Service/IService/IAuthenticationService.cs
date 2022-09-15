using Tangy.Models;

namespace TangyWebClient.Service.IService
{
    public interface IAuthenticationService
    {
        Task<SignupResponseDto> RegisterUser(SignupRequestDto request);
        Task<SignInResponseDto> Login(SignInRequestDto request);
        Task Logout();
    }
}
