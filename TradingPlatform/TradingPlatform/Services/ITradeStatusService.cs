using System.Collections.Generic;
using System.Threading.Tasks;
using TradingPlatform.Models;

public interface ITradeStatusService
{
    Task<List<TradeStatusModel>> GetAllAsync();
    Task<TradeStatusModel?> GetByIdAsync(int id);
    Task<TradeStatusModel?> GetByStatusKeyAsync(string statusKey);
    Task<TradeStatusModel> CreateAsync(TradeStatusModel status);
    Task<bool> UpdateAsync(TradeStatusModel status);
    Task<bool> DeleteAsync(int id);    
}