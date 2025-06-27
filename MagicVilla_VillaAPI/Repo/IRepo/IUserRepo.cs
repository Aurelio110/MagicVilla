using MagicVilla_VillaAPI.Users;

namespace MagicVilla_VillaAPI.Repo.IRepo
{
    public interface IUserRepo
    {
        bool IsUniqueUser(string userName);

        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task<RegisterUser> Register(RegisterUserRequest registerUserRequest);
    }
}
