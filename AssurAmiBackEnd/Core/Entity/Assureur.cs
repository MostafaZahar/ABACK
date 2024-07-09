using System.ComponentModel.DataAnnotations.Schema;

namespace AssurAmiBackEnd.Core.Entity
{
    public class Assureur
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? CodeAssureur { get; set; }

        public string? Libelet { get; set; }
        public ICollection<Client> Clients { get; set; } = new List<Client>();
    }
}
