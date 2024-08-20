using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ExpenseTrackerAPI.Data;

public class Expense
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string? UserId { get; set; }
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string? CategoryId { get; set; }
    
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}