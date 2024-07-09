using System.ComponentModel.DataAnnotations;

namespace AssurAmiBackEnd.Core.Entity.Authentification
{
    public class LoginModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "User Name is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
