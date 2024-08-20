using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ExpenseTrackerAPI.Data;

public class Category
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? Title { get; set; }
}