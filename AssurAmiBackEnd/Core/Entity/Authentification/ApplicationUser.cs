using Microsoft.AspNetCore.Identity;

namespace AssurAmiBackEnd.Core.Entity.Authentification
{
    public class ApplicationUser : IdentityUser
    {  
        public Boolean ActivateCompte {  get; set; }
        public ICollection<FileStatus>? FileStatuses { get; set; }
    }
}
