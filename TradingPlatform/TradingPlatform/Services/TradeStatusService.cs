using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TradingPlatform.Models.DbModels;

public class TradeStatusService : ITradeStatusService
{
    private readonly TradingPlatformDbContext _context;

    public TradeStatusService(TradingPlatformDbContext context)
    {
        _context = context;
    }

    public async Task<List<TradeStatusModel>> GetAllAsync()
    {
        return await _context.TradeStatuses.ToListAsync();
    }

    public async Task<TradeStatusModel?> GetByIdAsync(int id)
    {
        return await _context.TradeStatuses.FindAsync(id);
    }

    public async Task<TradeStatusModel?> GetByStatusKeyAsync(string statusKey)
    {
        return await _context.TradeStatuses.FirstOrDefaultAsync(i => i.StatusKey.Equals(statusKey));
    }

    public async Task<TradeStatusModel> CreateAsync(TradeStatusModel status)
    {
        status.UpdatedTime = DateTime.UtcNow;
        _context.TradeStatuses.Add(status);
        await _context.SaveChangesAsync();
        return status;
    }

    public async Task<bool> UpdateAsync(TradeStatusModel status)
    {
        var existing = await _context.TradeStatuses.FindAsync(status.Id);
        if (existing == null) return false;
        existing.StatusKey = status.StatusKey;
        existing.StatusValue = status.StatusValue;
        existing.UpdatedTime = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var status = await _context.TradeStatuses.FindAsync(id);
        if (status == null) return false;
        _context.TradeStatuses.Remove(status);
        await _context.SaveChangesAsync();
        return true;
    }
}