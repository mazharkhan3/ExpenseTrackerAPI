using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerAPI.Controllers;

[Authorize]
[ApiController]
[Route("expenses")]
public class ExpenseController : ControllerBase
{
    private readonly ExpenseService _expenseService;
    private readonly UserService _userService;

    public ExpenseController(ExpenseService expenseService, UserService userService)
    {
        _expenseService = expenseService;
        _userService = userService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateExpense([FromBody] Expense expense)
    {
        var userId = _userService.GetUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        expense.UserId = userId;
        
        return Ok(await _expenseService.CreateExpense(expense));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(string id, [FromBody] Expense expense)
    {
        var expenseDb = await _expenseService.GetExpense(id);

        if (expenseDb == null)
        {
            return NotFound("Expense not found");
        }
        
        expenseDb.Amount = expense.Amount;
        expenseDb.Title = expense.Title;
        expenseDb.Description = expense.Description;
        expenseDb.CategoryId = expense.CategoryId;
        
        await _expenseService.UpdateExpense(id, expenseDb);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllExpenses()
    {
        Console.WriteLine("UserID " + _userService.GetUserId());
        return Ok(await _expenseService.GetExpenses());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExpense(string id)
    {
        var expense = await _expenseService.GetExpense(id);

        if (expense == null)
        {
            return NotFound("Expense not found");
        }

        return Ok(expense);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(string id)
    {
        var expense = await _expenseService.GetExpense(id);

        if (expense == null)
        {
            return NotFound("Expense not found");
        }

        await _expenseService.DeleteExpense(id);
        
        return Ok("Expense Deleted");
    }
}