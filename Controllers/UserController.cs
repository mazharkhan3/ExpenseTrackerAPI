using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerAPI.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] User user)
    {   
        return Ok(await _userService.Register(user));
    }
    
    [HttpPost("login")]
    public IActionResult LoginUser([FromQuery] string email, string password)
    {
        return Ok( _userService.Login(email, password));
    }
}