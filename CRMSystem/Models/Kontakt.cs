using System.ComponentModel.DataAnnotations;

namespace CRMSystem.Models
{
    public class Kontakt
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefon { get; set; }
        public string? Position { get; set; }
        public string? Notizen { get; set; }
        public DateTime Erstellungsdatum { get; set; } = DateTime.Now;
        public int KundeId { get; set; }
        public Kunde Kunde { get; set; } = null!;
    }
}