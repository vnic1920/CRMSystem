using CRMSystem.Models;

namespace CRMSystem.Services
{
    public interface IAufgabeService
    {
        Task<List<Aufgabe>> GetAufgabenAsync();
        Task<Aufgabe?> GetAufgabeByIdAsync(int id);
        Task AddAufgabeAsync(Aufgabe aufgabe);
        Task UpdateAufgabeAsync(Aufgabe aufgabe);
        Task DeleteAufgabeAsync(int id);
        Task<List<Aufgabe>> GetHeuteFaelligAsync();
    }
}