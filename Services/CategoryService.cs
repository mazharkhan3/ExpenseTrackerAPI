using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExpenseTrackerAPI.Data;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace ExpenseTrackerAPI.Services;

public class CategoryService
{
    private readonly IMongoCollection<Category> _entities;

    public CategoryService(IMongoDatabase database)
    {
        _entities = database.GetCollection<Category>("categories");
    }

    public async Task<Category?> GetCategory(string id)
    {
        return await _entities.Find(u => u.Id == id).FirstOrDefaultAsync();
    }
}