using System.Collections.Generic;
using System.Threading.Tasks;
using TradingPlatform.Models.DbModels;

namespace TradingPlatform.Services
{
    public interface IActiveOrderService
    {
        Task<List<ActiveOrder>> GetAllActiveOrdersAsync();
        Task<ActiveOrder?> GetActiveOrderByIdAsync(string id);
        Task<ActiveOrder> AddOrderAsync(ActiveOrder order);
        Task<ActiveOrder> UpdateOrderAsync(ActiveOrder order);
        Task<bool> DeactivateOrderAsync(string id);
        Task<bool> DeleteOrderAsync(string id);
    }
}