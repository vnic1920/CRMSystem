using System.ComponentModel.DataAnnotations;

namespace CRMSystem.Models
{
    public class Aufgabe
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Titel ist erforderlich")]
        public string Titel { get; set; } = string.Empty;

        public string? Beschreibung { get; set; }

        [Required(ErrorMessage = "Fälligkeitsdatum ist erforderlich")]
        public DateTime Fälligkeitsdatum { get; set; }

        public string Priorität { get; set; } = "Mittel"; // Niedrig, Mittel, Hoch
        public string Status { get; set; } = "Offen"; // Offen, In Bearbeitung, Erledigt

        public DateTime Erstellungsdatum { get; set; } = DateTime.Now;
        public DateTime? Abschlussdatum { get; set; }

        // Foreign keys
        public int? KundeId { get; set; }
        public Kunde? Kunde { get; set; }

        public string? ZugewiesenAnUserId { get; set; }

        public string ErstelltVonUserId { get; set; } = string.Empty;
    }
}