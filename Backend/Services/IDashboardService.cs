using NileCareAPI.DTOs;

namespace NileCareAPI.Services;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardDataAsync();
}


