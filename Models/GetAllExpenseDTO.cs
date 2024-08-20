namespace ExpenseTrackerAPI.Models;

public class GetAllExpenseDTO
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime? Date { get; set; }
    public GetUserDTO? User { get; set; }
    public GetCategoryDTO Category { get; set; }
}