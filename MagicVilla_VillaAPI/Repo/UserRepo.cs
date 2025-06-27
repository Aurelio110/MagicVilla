using FluentAssertions.Common;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Repo.IRepo;
using MagicVilla_VillaAPI.Users;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_VillaAPI.Repo
{
    public class UserRepo : IUserRepo
    {
        public string secretKey;
        private readonly ApplicationDbContext _db;
        public UserRepo(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string userName)
        {
            var user = _db.RegisteredUsers.FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower());
            if(user == null)
            {
                return true;
            }

            
            return false;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var user = _db.RegisteredUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequest.User.ToLower() && u.Password == loginRequest.Password);
            if (user == null)
            {
                return new LoginResponse()
                {
                    Token = "",
                    User = null
                };
            }
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var encodedSecretKey = Encoding.ASCII.GetBytes(secretKey);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                ]),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponse loginResponse = new LoginResponse
            {
                Token = tokenHandler.WriteToken(token),
                User = user
            };
            return await Task.FromResult(loginResponse);
        }

        public async Task<RegisterUser> Register(RegisterUserRequest registerUserRequest)
        {
            RegisterUser user = new RegisterUser
            {
                UserName = registerUserRequest.UserName,
                Password = registerUserRequest.Password,
                ConfirmPassword = registerUserRequest.ConfirmPassword,
                Role = registerUserRequest.Role
            };
            _db.RegisteredUsers.Add(user);
            await _db.SaveChangesAsync();
            user.Password = "";
            return await Task.FromResult(user);
        }
    }
}
