using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public interface ITransportRepository
{
    Task<List<Transport>> GetAllAsync();
    Task<Transport?> GetByIdAsync(int id);
    Task<List<Transport>> GetAccessibleAsync();
}


