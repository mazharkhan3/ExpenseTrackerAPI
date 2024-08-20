using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Models;
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
    private readonly CategoryService _categoryService;

    public ExpenseController(ExpenseService expenseService, UserService userService, CategoryService categoryService)
    {
        _expenseService = expenseService;
        _userService = userService;
        _categoryService = categoryService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateExpense([FromBody] Expense expense)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("All fields are required");
        }
        
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
        if (!ModelState.IsValid)
        {
            return BadRequest("All fields are required");
        }
        
        if (id != expense.Id)
        {
            return BadRequest("Expense id is not correct");
        }
        
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
        var expensesListDto = new List<GetAllExpenseDTO>();
        
        var expenses = await _expenseService.GetExpenses();

        foreach (var expense in expenses)
        {
            var expenseDto = new GetAllExpenseDTO()
            {
                Id = expense.Id,
                Title = expense.Title,
                Description = expense.Description,
                Amount = expense.Amount,
                Date = expense.Date,
            };
            
            var user = await _userService.GetUser(expense.UserId!);
            var category = await _categoryService.GetCategory(expense.CategoryId!);
            
            if (user != null)
            {
                expenseDto.User = new GetUserDTO()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                };
            }
            
            if (category != null)
            {
                expenseDto.Category = new GetCategoryDTO()
                {
                    Id = category.Id,
                    Title = category.Title,
                };
            }
            
            expensesListDto.Add(expenseDto);
        }
        
        return Ok(expensesListDto);
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