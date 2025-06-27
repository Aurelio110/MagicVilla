namespace MagicVilla_VillaAPI.Users
{
    public class LoginResponse
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; internal set; }
        public RegisterUser User { get; internal set; }
        public object Token { get; set; }
    }
}
