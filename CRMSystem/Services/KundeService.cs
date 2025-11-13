using CRMSystem.Models;
using CRMSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CRMSystem.Services
{
    public class KundeService : IKundeService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<KundeService> _logger;

        public KundeService(ApplicationDbContext context, ILogger<KundeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Kunde>> GetKundenAsync()
        {
            try
            {
                _logger.LogInformation("Getting all customers");
                var kunden = await _context.Kunden
                    .Include(k => k.Kontakte)
                    .OrderByDescending(k => k.Erstellungsdatum)
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} customers", kunden.Count);
                return kunden;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customers");
                throw new Exception("Fehler beim Laden der Kunden: " + ex.Message);
            }
        }

        public async Task<Kunde?> GetKundeByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Getting customer by ID: {Id}", id);
                return await _context.Kunden
                    .Include(k => k.Kontakte)
                    .FirstOrDefaultAsync(k => k.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer by ID: {Id}", id);
                throw new Exception($"Fehler beim Laden des Kunden mit ID {id}: " + ex.Message);
            }
        }

        public async Task AddKundeAsync(Kunde kunde)
        {
            try
            {
                _logger.LogInformation("Adding new customer: {Name}", kunde.Name);

                // Validate required fields
                if (string.IsNullOrWhiteSpace(kunde.Name))
                {
                    throw new Exception("Kundenname ist erforderlich");
                }

                kunde.Erstellungsdatum = DateTime.Now;
                _context.Kunden.Add(kunde);

                var result = await _context.SaveChangesAsync();
                _logger.LogInformation("Customer added successfully. Save changes result: {Result}", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding customer: {Name}", kunde.Name);
                throw new Exception("Fehler beim Speichern des Kunden: " + ex.Message);
            }
        }

        public async Task UpdateKundeAsync(Kunde kunde)
        {
            try
            {
                _logger.LogInformation("Updating customer ID: {Id}", kunde.Id);

                var existingKunde = await _context.Kunden.FindAsync(kunde.Id);
                if (existingKunde == null)
                {
                    _logger.LogWarning("Customer not found for update: {Id}", kunde.Id);
                    throw new Exception($"Kunde mit ID {kunde.Id} nicht gefunden");
                }

                // Update properties
                existingKunde.Name = kunde.Name;
                existingKunde.Email = kunde.Email;
                existingKunde.Telefon = kunde.Telefon;
                existingKunde.Firma = kunde.Firma;
                existingKunde.Adresse = kunde.Adresse;
                existingKunde.Notizen = kunde.Notizen;

                var result = await _context.SaveChangesAsync();
                _logger.LogInformation("Customer updated successfully. Save changes result: {Result}", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer ID: {Id}", kunde.Id);
                throw new Exception("Fehler beim Aktualisieren des Kunden: " + ex.Message);
            }
        }

        public async Task DeleteKundeAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting customer ID: {Id}", id);

                var kunde = await _context.Kunden.FindAsync(id);
                if (kunde == null)
                {
                    _logger.LogWarning("Customer not found for deletion: {Id}", id);
                    throw new Exception($"Kunde mit ID {id} nicht gefunden");
                }

                _context.Kunden.Remove(kunde);
                var result = await _context.SaveChangesAsync();
                _logger.LogInformation("Customer deleted successfully. Save changes result: {Result}", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer ID: {Id}", id);
                throw new Exception("Fehler beim Löschen des Kunden: " + ex.Message);
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
                _logger.LogError(ex, "Error exporting customers to CSV");
                throw new Exception("Fehler beim Export der Kunden: " + ex.Message);
            }
        }

        public async Task<List<string>> ImportKundenFromCsvAsync(string csvData)
        {
            var results = new List<string>();

            try
            {
                var lines = csvData.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

                if (lines.Length < 2)
                {
                    results.Add("❌ CSV-Datei ist leer oder ungültig");
                    return results;
                }

                int successCount = 0;
                int errorCount = 0;

                for (int i = 1; i < lines.Length; i++)
                {
                    try
                    {
                        var fields = ParseCsvLine(lines[i]);

                        if (fields.Length >= 2)
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing customers from CSV");
                results.Add($"❌ Import fehlgeschlagen: {ex.Message}");
            }

            return results;
        }

        private static string[] ParseCsvLine(string line)
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