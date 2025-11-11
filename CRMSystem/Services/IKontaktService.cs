using CRMSystem.Models;

namespace CRMSystem.Services
{
    public interface IKontaktService
    {
        Task<List<Kontakt>> GetKontakteAsync();
        Task<List<Kontakt>> GetKontakteByKundeIdAsync(int kundeId);
        Task<Kontakt?> GetKontaktByIdAsync(int id);
        Task AddKontaktAsync(Kontakt kontakt);
        Task UpdateKontaktAsync(Kontakt kontakt);
        Task DeleteKontaktAsync(int id);
        Task<int> GetKontakteCountAsync();
        Task<List<Kontakt>> SearchKontakteAsync(string searchTerm);
    }
}