using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace AssurAmiBackEnd.Core.Entity
{
    public class Client
    {
        public long Id { get; set; }
        public string? Matricule { get; set; }
        public string? Name { get; set; }
        public string? Prenom { get; set; }
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateNaissance { get; set; }
        public string? Sexe { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateFeet { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateSortie { get; set; }
        public string? code1 { get; set; }
        public string? code2 { get; set; }
        public long GestionnaireId { get; set; }
        public Gestionnaire? Gestionnaire { get; set; } = null!;
        public long AssureurId { get; set; }
        public Assureur? Assureur { get; set; } = null!;
        public bool? IsConsumed { get; set; } // Modifié pour accepter des valeurs nulles

    }
}
