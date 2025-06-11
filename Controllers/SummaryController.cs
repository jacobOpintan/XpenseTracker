using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using XpenseTracker.Models;
using XpenseTracker.Services;

namespace XpenseTracker.Controllers
{
[Route("api/summary")]
[ApiController]
[Authorize]
public class SummaryController : ControllerBase
{
    private readonly IExpenseService _expenseService;
    private readonly UserManager<ApplicationUser> _userManager;

    public SummaryController(IExpenseService expenseService, UserManager<ApplicationUser> userManager)
    {
        _expenseService = expenseService;
        _userManager = userManager;
    }

    private async Task<string> GetUserIdAsync() =>
        (await _userManager.GetUserAsync(User))?.Id!;

    [HttpGet("monthly")]
    public async Task<IActionResult> GetMonthlySummary()
    {
        var userId = await GetUserIdAsync();
        var summary = await _expenseService.GetMonthlySummaryAsync(userId);
        return Ok(summary);
    }

    [HttpGet("expense-categories")]
    public async Task<IActionResult> GetCategorySummary()
    {
        var userId = await GetUserIdAsync();
        var summary = await _expenseService.GetCategorySummaryAsync(userId);
        return Ok(summary);
    }

    [HttpGet("total-expenses")]
    public async Task<IActionResult> GetTotalExpenses()
    {
        var userId = await GetUserIdAsync();
        var total = await _expenseService.GetTotalExpensesAsync(userId);
        return Ok(new { total });
    }
}
}