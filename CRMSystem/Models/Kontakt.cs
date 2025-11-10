using System.ComponentModel.DataAnnotations;

namespace CRMSystem.Models
{
    public class Kontakt
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

        [StringLength(100, ErrorMessage = "Position darf nicht länger als 100 Zeichen sein")]
        public string? Position { get; set; }

        [StringLength(500, ErrorMessage = "Notizen dürfen nicht länger als 500 Zeichen sein")]
        public string? Notizen { get; set; }

        public DateTime Erstellungsdatum { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Kunde ist erforderlich")]
        public int KundeId { get; set; }
        public Kunde Kunde { get; set; } = null!;
    }
}