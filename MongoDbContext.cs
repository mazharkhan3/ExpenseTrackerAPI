using ExpenseTrackerAPI.Data;
using MongoDB.Driver;

namespace ExpenseTrackerAPI;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IMongoDatabase database)
    {
        _database = database;
    }

    public IMongoCollection<Category> Categories => _database.GetCollection<Category>("categories");
}