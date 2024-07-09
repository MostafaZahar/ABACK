using System.ComponentModel.DataAnnotations;

namespace AssurAmiBackEnd.Core.Entity.Authentification
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public RoleEnnum Role { get; set; }
    }
}
