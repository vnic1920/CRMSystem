using System.ComponentModel.DataAnnotations;

namespace CRMSystem.Models
{
    public class Kunde
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name ist erforderlich")]
        [StringLength(100, ErrorMessage = "Name darf nicht länger als 100 Zeichen sein")]
        public string Name { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Ungültige E-Mail-Adresse")]
        [StringLength(100, ErrorMessage = "E-Mail darf nicht länger als 100 Zeichen sein")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Ungültige Telefonnummer")]
        [StringLength(50, ErrorMessage = "Telefon darf nicht länger als 50 Zeichen sein")]
        public string? Telefon { get; set; }

        [StringLength(100, ErrorMessage = "Firma darf nicht länger als 100 Zeichen sein")]
        public string? Firma { get; set; }

        [StringLength(200, ErrorMessage = "Adresse darf nicht länger als 200 Zeichen sein")]
        public string? Adresse { get; set; }

        [StringLength(1000, ErrorMessage = "Notizen dürfen nicht länger als 1000 Zeichen sein")]
        public string? Notizen { get; set; }

        public DateTime Erstellungsdatum { get; set; } = DateTime.Now;

        // Navigation property for Kontakte
        public virtual ICollection<Kontakt> Kontakte { get; set; } = [];
    }
}