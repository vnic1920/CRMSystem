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
            try
            {
                return await _context.Kunden
                    .Include(k => k.Kontakte)
                    .OrderByDescending(k => k.Erstellungsdatum)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Laden der Kunden", ex);
            }
        }

        public async Task<Kunde?> GetKundeByIdAsync(int id)
        {
            try
            {
                return await _context.Kunden
                    .Include(k => k.Kontakte)
                    .FirstOrDefaultAsync(k => k.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Fehler beim Laden des Kunden mit ID {id}", ex);
            }
        }

        public async Task AddKundeAsync(Kunde kunde)
        {
            try
            {
                kunde.Erstellungsdatum = DateTime.Now;
                _context.Kunden.Add(kunde);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Speichern des Kunden", ex);
            }
        }

        public async Task UpdateKundeAsync(Kunde kunde)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Aktualisieren des Kunden", ex);
            }
        }

        public async Task DeleteKundeAsync(int id)
        {
            try
            {
                var kunde = await _context.Kunden.FindAsync(id);
                if (kunde != null)
                {
                    _context.Kunden.Remove(kunde);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Löschen des Kunden", ex);
            }
        }

        public async Task<string> ExportKundenToCsvAsync()
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Export der Kunden", ex);
            }
        }

        // 👇 НОВИЙ МЕТОД ДЛЯ ІМПОРТУ
        public async Task<List<string>> ImportKundenFromCsvAsync(string csvData)
        {
            var results = new List<string>();
            var lines = csvData.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

            if (lines.Length < 2)
            {
                results.Add("❌ CSV-Datei ist leer oder ungültig");
                return results;
            }

            int successCount = 0;
            int errorCount = 0;

            for (int i = 1; i < lines.Length; i++) // Skip header
            {
                try
                {
                    var fields = ParseCsvLine(lines[i]);

                    if (fields.Length >= 2) // At least Name
                    {
                        var kunde = new Kunde
                        {
                            Name = fields[1]?.Trim() ?? "",
                            Email = fields.Length > 2 ? fields[2]?.Trim() : null,
                            Telefon = fields.Length > 3 ? fields[3]?.Trim() : null,
                            Firma = fields.Length > 4 ? fields[4]?.Trim() : null,
                            Adresse = fields.Length > 5 ? fields[5]?.Trim() : null,
                            Notizen = fields.Length > 6 ? fields[6]?.Trim() : null,
                            Erstellungsdatum = DateTime.Now
                        };

                        if (!string.IsNullOrEmpty(kunde.Name))
                        {
                            _context.Kunden.Add(kunde);
                            successCount++;
                            results.Add($"✅ {kunde.Name} importiert");
                        }
                        else
                        {
                            errorCount++;
                            results.Add($"❌ Zeile {i + 1}: Name fehlt");
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorCount++;
                    results.Add($"❌ Fehler in Zeile {i + 1}: {ex.Message}");
                }
            }

            if (successCount > 0)
            {
                await _context.SaveChangesAsync();
            }

            results.Add($"");
            results.Add($"📊 Zusammenfassung: {successCount} erfolgreich, {errorCount} Fehler");

            return results;
        }

        private string[] ParseCsvLine(string line)
        {
            var result = new List<string>();
            var inQuotes = false;
            var currentField = "";

            foreach (var c in line)
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ';' && !inQuotes)
                {
                    result.Add(currentField);
                    currentField = "";
                }
                else
                {
                    currentField += c;
                }
            }

            result.Add(currentField);
            return result.ToArray();
        }

        private static string EscapeCsv(string? value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            if (value.Contains(';') || value.Contains('"') || value.Contains('\n'))
                return $"\"{value.Replace("\"", "\"\"")}\"";
            return value;
        }
    }
}