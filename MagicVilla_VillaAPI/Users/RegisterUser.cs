using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Users
{
    public class RegisterUser
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Role { get; set; }

    }
}
