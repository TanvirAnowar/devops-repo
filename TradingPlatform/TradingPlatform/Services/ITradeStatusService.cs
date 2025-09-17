using System.Collections.Generic;
using System.Threading.Tasks;

public interface ITradeStatusService
{
    Task<List<TradeStatus>> GetAllAsync();
    Task<TradeStatus?> GetByIdAsync(int id);
    Task<TradeStatus?> GetByStatusKeyAsync(string statusKey);
    Task<TradeStatus> CreateAsync(TradeStatus status);
    Task<bool> UpdateAsync(TradeStatus status);
    Task<bool> DeleteAsync(int id);
}