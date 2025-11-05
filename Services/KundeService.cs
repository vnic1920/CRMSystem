using CRMSystem.Models;
using System.Text;

namespace CRMSystem.Services
{
    public class KundeService : IKundeService
    {
        private List<Kunde> _kunden = new List<Kunde>();
        private int _nextId = 1;

        public Task<List<Kunde>> GetKundenAsync()
        {
            return Task.FromResult(_kunden);
        }

        public Task<Kunde?> GetKundeByIdAsync(int id)
        {
            var kunde = _kunden.FirstOrDefault(k => k.Id == id);
            return Task.FromResult(kunde);
        }

        public Task AddKundeAsync(Kunde kunde)
        {
            kunde.Id = _nextId++;
            _kunden.Add(kunde);
            return Task.CompletedTask;
        }

        public Task UpdateKundeAsync(Kunde kunde)
        {
            var existingKunde = _kunden.FirstOrDefault(k => k.Id == kunde.Id);
            if (existingKunde != null)
            {
                existingKunde.Name = kunde.Name;
                existingKunde.Email = kunde.Email;
                existingKunde.Telefon = kunde.Telefon;
                existingKunde.Firma = kunde.Firma;
                existingKunde.Adresse = kunde.Adresse;
                existingKunde.Notizen = kunde.Notizen;
            }
            return Task.CompletedTask;
        }

        public Task DeleteKundeAsync(int id)
        {
            var kunde = _kunden.FirstOrDefault(k => k.Id == id);
            if (kunde != null)
            {
                _kunden.Remove(kunde);
            }
            return Task.CompletedTask;
        }

        public Task<string> ExportKundenToCsvAsync()
        {
            var csv = new StringBuilder();
            csv.AppendLine("ID;Name;Email;Telefon;Firma;Adresse;Notizen;Erstellungsdatum");

            foreach (var kunde in _kunden)
            {
                csv.AppendLine($"{kunde.Id};{EscapeCsv(kunde.Name)};{EscapeCsv(kunde.Email)};{EscapeCsv(kunde.Telefon)};{EscapeCsv(kunde.Firma)};{EscapeCsv(kunde.Adresse)};{EscapeCsv(kunde.Notizen)};{kunde.Erstellungsdatum:yyyy-MM-dd HH:mm:ss}");
            }

            return Task.FromResult(csv.ToString());
        }

        private string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            if (value.Contains(";") || value.Contains("\"") || value.Contains("\n"))
                return $"\"{value.Replace("\"", "\"\"")}\"";
            return value;
        }
    }
}