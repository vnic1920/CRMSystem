using CRMSystem.Models;
using CRMSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CRMSystem.Services
{
    public class KundeService : IKundeService
    {
        private readonly ApplicationDbContext _context;

        public KundeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Kunde>> GetKundenAsync()
        {
            return await _context.Kunden
                .OrderByDescending(k => k.Erstellungsdatum)
                .ToListAsync();
        }

        public async Task<Kunde?> GetKundeByIdAsync(int id)
        {
            return await _context.Kunden.FindAsync(id);
        }

        public async Task AddKundeAsync(Kunde kunde)
        {
            kunde.Erstellungsdatum = DateTime.Now;
            _context.Kunden.Add(kunde);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateKundeAsync(Kunde kunde)
        {
            var existingKunde = await _context.Kunden.FindAsync(kunde.Id);
            if (existingKunde != null)
            {
                existingKunde.Name = kunde.Name;
                existingKunde.Email = kunde.Email;
                existingKunde.Telefon = kunde.Telefon;
                existingKunde.Firma = kunde.Firma;
                existingKunde.Adresse = kunde.Adresse;
                existingKunde.Notizen = kunde.Notizen;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteKundeAsync(int id)
        {
            var kunde = await _context.Kunden.FindAsync(id);
            if (kunde != null)
            {
                _context.Kunden.Remove(kunde);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> ExportKundenToCsvAsync()
        {
            var kunden = await GetKundenAsync();
            var csv = new StringBuilder();
            csv.AppendLine("ID;Name;Email;Telefon;Firma;Adresse;Notizen;Erstellungsdatum");

            foreach (var kunde in kunden)
            {
                csv.AppendLine($"{kunde.Id};{EscapeCsv(kunde.Name)};{EscapeCsv(kunde.Email)};{EscapeCsv(kunde.Telefon)};{EscapeCsv(kunde.Firma)};{EscapeCsv(kunde.Adresse)};{EscapeCsv(kunde.Notizen)};{kunde.Erstellungsdatum:yyyy-MM-dd HH:mm:ss}");
            }

            return csv.ToString();
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