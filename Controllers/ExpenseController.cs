using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XpenseTracker.Data;
using XpenseTracker.Dtos;
using XpenseTracker.Models;


namespace XpenseTracker.Controllers
{
    [ApiController]
    [Route("api/expense")]
    public class ExpenseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public ExpenseController(ApplicationDbContext context , UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // Get the current user's ID
        private async Task<string> GetUserIdAsync()
        {
            var user = await userManager.GetUserAsync(User);
            return user?.Id!;
        }

       

        [Route("/expense")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetAllExpenses()
        {
            var expense = await _context.Expenses.ToListAsync();
            if (expense == null || !expense.Any())
            {
                return NotFound("No expenses found.");
            }
            return Ok(expense);
        }

        //getting expenses by Id 
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult<Expense>> GetExpenseById(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense == null)
            {
                return NotFound($"Expense with ID {id} not found.");
            }
            return Ok(expense);
        }

        [Route("create")]
        [HttpPost]
        public async Task<ActionResult<ExpenseDto>> CreateExpense([FromBody] ExpenseDto expenseDto)
        {
            if (expenseDto == null)
            {
                return BadRequest("Expense data cannot be null.");
            }
            var userId = await GetUserIdAsync();

            var expense = new Expense
            {
                Amount = expenseDto.Amount,
                Description = expenseDto.Description,
                Date = expenseDto.Date,
                CategoryId = expenseDto.CategoryId,
                UserId=userId
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExpenseById), new { id = expense.Id }, expense);
        }

        




        
    }
}