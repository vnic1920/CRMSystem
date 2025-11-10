using CRMSystem.Models;
using CRMSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Services
{
    public class AufgabeService : IAufgabeService
    {
        private readonly ApplicationDbContext _context;

        public AufgabeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Aufgabe>> GetAufgabenAsync()
        {
            return await _context.Aufgaben
                .Include(a => a.Kunde)
                .OrderByDescending(a => a.Fälligkeitsdatum)
                .ToListAsync();
        }

        public async Task<Aufgabe?> GetAufgabeByIdAsync(int id)
        {
            return await _context.Aufgaben
                .Include(a => a.Kunde)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAufgabeAsync(Aufgabe aufgabe)
        {
            aufgabe.Erstellungsdatum = DateTime.Now;
            _context.Aufgaben.Add(aufgabe);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAufgabeAsync(Aufgabe aufgabe)
        {
            var existingAufgabe = await _context.Aufgaben.FindAsync(aufgabe.Id);
            if (existingAufgabe != null)
            {
                existingAufgabe.Titel = aufgabe.Titel;
                existingAufgabe.Beschreibung = aufgabe.Beschreibung;
                existingAufgabe.Fälligkeitsdatum = aufgabe.Fälligkeitsdatum;
                existingAufgabe.Priorität = aufgabe.Priorität;
                existingAufgabe.Status = aufgabe.Status;
                existingAufgabe.KundeId = aufgabe.KundeId;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAufgabeAsync(int id)
        {
            var aufgabe = await _context.Aufgaben.FindAsync(id);
            if (aufgabe != null)
            {
                _context.Aufgaben.Remove(aufgabe);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Aufgabe>> GetHeuteFaelligAsync()
        {
            return await _context.Aufgaben
                .Where(a => a.Fälligkeitsdatum.Date == DateTime.Today && a.Status != "Erledigt")
                .ToListAsync();
        }
    }
}