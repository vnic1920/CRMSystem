using CRMSystem.Models;
using CRMSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Services
{
    public class KontaktService : IKontaktService
    {
        private readonly ApplicationDbContext _context;

        public KontaktService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Kontakt>> GetKontakteAsync()
        {
            try
            {
                return await _context.Kontakte
                    .Include(k => k.Kunde)
                    .OrderByDescending(k => k.Erstellungsdatum)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Laden der Kontakte", ex);
            }
        }

        public async Task<List<Kontakt>> GetKontakteByKundeIdAsync(int kundeId)
        {
            try
            {
                return await _context.Kontakte
                    .Where(k => k.KundeId == kundeId)
                    .Include(k => k.Kunde)
                    .OrderBy(k => k.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Fehler beim Laden der Kontakte für Kunde {kundeId}", ex);
            }
        }

        public async Task<Kontakt?> GetKontaktByIdAsync(int id)
        {
            try
            {
                return await _context.Kontakte
                    .Include(k => k.Kunde)
                    .FirstOrDefaultAsync(k => k.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Fehler beim Laden des Kontakts mit ID {id}", ex);
            }
        }

        public async Task AddKontaktAsync(Kontakt kontakt)
        {
            try
            {
                kontakt.Erstellungsdatum = DateTime.Now;
                _context.Kontakte.Add(kontakt);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Speichern des Kontakts", ex);
            }
        }

        public async Task UpdateKontaktAsync(Kontakt kontakt)
        {
            try
            {
                var existingKontakt = await _context.Kontakte.FindAsync(kontakt.Id);
                if (existingKontakt != null)
                {
                    existingKontakt.Name = kontakt.Name;
                    existingKontakt.Email = kontakt.Email;
                    existingKontakt.Telefon = kontakt.Telefon;
                    existingKontakt.Position = kontakt.Position;
                    existingKontakt.Notizen = kontakt.Notizen;
                    existingKontakt.KundeId = kontakt.KundeId;

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Aktualisieren des Kontakts", ex);
            }
        }

        public async Task DeleteKontaktAsync(int id)
        {
            try
            {
                var kontakt = await _context.Kontakte.FindAsync(id);
                if (kontakt != null)
                {
                    _context.Kontakte.Remove(kontakt);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Löschen des Kontakts", ex);
            }
        }

        public async Task<int> GetKontakteCountAsync()
        {
            try
            {
                return await _context.Kontakte.CountAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Zählen der Kontakte", ex);
            }
        }

        public async Task<List<Kontakt>> SearchKontakteAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return await GetKontakteAsync();

                var term = searchTerm.ToLower();
                return await _context.Kontakte
                    .Include(k => k.Kunde)
                    .Where(k =>
                        k.Name.ToLower().Contains(term) ||
                        (k.Email != null && k.Email.ToLower().Contains(term)) ||
                        (k.Position != null && k.Position.ToLower().Contains(term)) ||
                        (k.Kunde != null && k.Kunde.Name.ToLower().Contains(term))
                    )
                    .OrderByDescending(k => k.Erstellungsdatum)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler bei der Kontaktsuche", ex);
            }
        }
    }
}