namespace CRMSystem.Models
{
    public class Kunde
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Firma { get; set; }
        public string Adresse { get; set; }
        public string Notizen { get; set; }
        public DateTime Erstellungsdatum { get; set; } = DateTime.Now;
    }
}