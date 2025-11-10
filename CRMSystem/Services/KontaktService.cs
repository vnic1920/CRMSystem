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
            return await _context.Kontakte
                .Include(k => k.Kunde)
                .OrderByDescending(k => k.Erstellungsdatum)
                .ToListAsync();
        }

        public async Task<List<Kontakt>> GetKontakteByKundeIdAsync(int kundeId)
        {
            return await _context.Kontakte
                .Where(k => k.KundeId == kundeId)
                .Include(k => k.Kunde)
                .ToListAsync();
        }

        public async Task<Kontakt?> GetKontaktByIdAsync(int id)
        {
            return await _context.Kontakte
                .Include(k => k.Kunde)
                .FirstOrDefaultAsync(k => k.Id == id);
        }

        public async Task AddKontaktAsync(Kontakt kontakt)
        {
            kontakt.Erstellungsdatum = DateTime.Now;
            _context.Kontakte.Add(kontakt);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateKontaktAsync(Kontakt kontakt)
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

        public async Task DeleteKontaktAsync(int id)
        {
            var kontakt = await _context.Kontakte.FindAsync(id);
            if (kontakt != null)
            {
                _context.Kontakte.Remove(kontakt);
                await _context.SaveChangesAsync();
            }
        }
    }
}