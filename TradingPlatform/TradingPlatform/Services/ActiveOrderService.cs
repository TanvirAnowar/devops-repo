using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingPlatform.Models.DbModels;
using static TradingPlatform.Models.ApiModels.OrderRequest;

namespace TradingPlatform.Services
{
    public class ActiveOrderService : IActiveOrderService
    {
        private readonly TradingPlatformDbContext _dbContext;

        public ActiveOrderService(TradingPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all active orders
        /// </summary>
        /// <returns>List of active orders</returns>
        public async Task<List<ActiveOrder>> GetAllActiveOrdersAsync()
        {
            var id = "77";
            var x = await _dbContext.ActiveOrders.ToListAsync();

         


            var result = await _dbContext.ActiveOrders
                .Where(o => o.IsOrderActive == true)
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// Gets an active order by ID
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>The active order if found, null otherwise</returns>
        public async Task<ActiveOrder?> GetActiveOrderByIdAsync(string id)
        {
            return await _dbContext.ActiveOrders
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        /// <summary>
        /// Adds a new order
        /// </summary>
        /// <param name="order">The order to add</param>
        /// <returns>The added order</returns>
        public async Task<ActiveOrder> AddOrderAsync(ActiveOrder order)
        {
            if (string.IsNullOrEmpty(order.Id))
            {
                throw new ArgumentException("Order ID cannot be empty", nameof(order));
            }

            order.OrderDateTime = DateTime.UtcNow;
            order.IsOrderActive = true;

            await _dbContext.ActiveOrders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            
            return order;
        }

        /// <summary>
        /// Updates an existing order
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <returns>The updated order</returns>
        public async Task<ActiveOrder> UpdateOrderAsync(ActiveOrder order)
        {
            var existingOrder = await _dbContext.ActiveOrders
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Order with ID {order.Id} not found");
            }

            // Update fields
            existingOrder.Unites = order.Unites;
            existingOrder.EntryPrice = order.EntryPrice;
            existingOrder.StopLossPrice = order.StopLossPrice;
            existingOrder.TakeProfitPrice = order.TakeProfitPrice;
            existingOrder.OrderJsonObject = order.OrderJsonObject;
            existingOrder.IsOrderActive = order.IsOrderActive;

            _dbContext.ActiveOrders.Update(existingOrder);
            await _dbContext.SaveChangesAsync();
            
            return existingOrder;
        }

        /// <summary>
        /// Deactivates an order without deleting it
        /// </summary>
        /// <param name="id">The order ID</param>
        /// <returns>True if successful, false otherwise</returns>
        public async Task<bool> DeactivateOrderAsync(string id)
        {
            var order = await _dbContext.ActiveOrders
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return false;
            }

            order.IsOrderActive = false;
            _dbContext.ActiveOrders.Update(order);
            await _dbContext.SaveChangesAsync();
            
            return true;
        }

        /// <summary>
        /// Permanently deletes an order
        /// </summary>
        /// <param name="id">The order ID</param>
        /// <returns>True if successful, false otherwise</returns>
        public async Task<bool> DeleteOrderAsync(string id)
        {
            var order = await _dbContext.ActiveOrders
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return false;
            }

            _dbContext.ActiveOrders.Remove(order);
            await _dbContext.SaveChangesAsync();
            
            return true;
        }

        /// <summary>
        /// Updates only the IsOrderActive field for an order with the specified ID
        /// </summary>
        /// <param name="id">The order ID</param>
        /// <returns>True if the order was updated, false if the order was not found</returns>
        public async Task<bool> UpdateActiveOrderToFalseAsync(string id)
        {
            var order = await _dbContext.ActiveOrders
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return false;
            }

            // Update only the IsOrderActive field
            order.IsOrderActive = false;
            _dbContext.ActiveOrders.Update(order);
            await _dbContext.SaveChangesAsync();
            
            return true;
        }
    }
}