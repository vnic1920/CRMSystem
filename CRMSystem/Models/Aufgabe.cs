using System.ComponentModel.DataAnnotations;

namespace CRMSystem.Models
{
    public class Aufgabe
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Titel ist erforderlich")]
        [StringLength(200, ErrorMessage = "Titel darf nicht länger als 200 Zeichen sein")]
        public string Titel { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Beschreibung darf nicht länger als 1000 Zeichen sein")]
        public string? Beschreibung { get; set; }

        [Required(ErrorMessage = "Fälligkeitsdatum ist erforderlich")]
        public DateTime Fälligkeitsdatum { get; set; }

        public string Priorität { get; set; } = "Mittel";
        public string Status { get; set; } = "Offen";

        public DateTime Erstellungsdatum { get; set; } = DateTime.Now;
        public DateTime? Abschlussdatum { get; set; }

        public int? KundeId { get; set; }
        public Kunde? Kunde { get; set; }

        public string? ZugewiesenAnUserId { get; set; }
        public string ErstelltVonUserId { get; set; } = string.Empty;
    }
}