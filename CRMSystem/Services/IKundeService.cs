using CRMSystem.Models;

namespace CRMSystem.Services
{
    public interface IKundeService
    {
        Task<List<Kunde>> GetKundenAsync();
        Task<Kunde?> GetKundeByIdAsync(int id);
        Task AddKundeAsync(Kunde kunde);
        Task UpdateKundeAsync(Kunde kunde);
        Task DeleteKundeAsync(int id);
        Task<string> ExportKundenToCsvAsync();
        Task<List<string>> ImportKundenFromCsvAsync(string csvData); // 👈 НОВИЙ МЕТОД
    }
}