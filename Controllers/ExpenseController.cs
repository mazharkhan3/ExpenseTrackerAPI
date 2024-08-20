using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerAPI.Controllers;

[ApiController]
[Route("expenses")]
public class ExpenseController : ControllerBase
{
    private readonly ExpenseService _expenseService;

    public ExpenseController(ExpenseService expenseService)
    {
        _expenseService = expenseService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateExpense([FromBody] Expense expense)
    {
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
        
        await _expenseService.UpdateExpense(id, expense);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllExpenses()
    {
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