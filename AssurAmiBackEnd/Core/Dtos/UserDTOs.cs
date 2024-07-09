using AssurAmiBackEnd.Core.Entity.Authentification;
using System.ComponentModel.DataAnnotations;

namespace AssurAmiBackEnd.Core.Dtos
{
    public class UserDTOs
    {
        [Required]
        
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        //[System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public RoleEnnum roleEnnum { get; set; }
    }
}
