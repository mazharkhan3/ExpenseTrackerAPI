using ExpenseTrackerAPI.Data;
using MongoDB.Driver;

namespace ExpenseTrackerAPI.Services;

public class ExpenseService
{
    private readonly IMongoCollection<Expense> _entities;

    public ExpenseService(IMongoDatabase database)
    {
        _entities = database.GetCollection<Expense>("expenses");
    }
    
    public async Task<List<Expense>> GetExpenses()
    {
        return await _entities.Find(_ => true).ToListAsync();
    }
    
    public async Task<Expense?> GetExpense(string id)
    {
        return await _entities.Find(e => e.Id == id).FirstOrDefaultAsync();
    }
    
    public async Task<Expense> CreateExpense(Expense expense)
    {
        await _entities.InsertOneAsync(expense);
        return expense;
    }
    
    public async Task UpdateExpense(string id, Expense expense)
    {
        await _entities.ReplaceOneAsync(e => e.Id == id, expense);
    }
    
    public async Task DeleteExpense(string id)
    {
        await _entities.DeleteOneAsync(e => e.Id == id);
    }
}