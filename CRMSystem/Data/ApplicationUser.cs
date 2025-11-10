using Microsoft.AspNetCore.Identity;

namespace CRMSystem.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? Vorname { get; set; }
        public string? Nachname { get; set; }
        public DateTime Erstellungsdatum { get; set; } = DateTime.Now;
    }
}