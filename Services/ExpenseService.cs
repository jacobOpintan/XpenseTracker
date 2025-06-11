using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using XpenseTracker.Data;
using XpenseTracker.Dtos;

namespace XpenseTracker.Services
{
    public class ExpenseService : IExpenseService
    {

        //instantiatting the dtatbase object
        private readonly ApplicationDbContext _context;

        //constructor to inject the database context
        public ExpenseService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<ExpenseSummaryDto>> GetCategorySummaryAsync(string userId)
        {
            return await _context.Expenses
                                .Where(e => e.UserId == userId)
                                .GroupBy(e => e.Category.Name)
                                .Select(g => new ExpenseSummaryDto
                                {
                                    Group = g.Key,
                                    TotalAmount = g.Sum(e => e.Amount)
                                })
                                .ToListAsync();
        }

        public async Task<List<ExpenseSummaryDto>> GetMonthlySummaryAsync(string userId)
        {
              return await _context.Expenses
                                    .Where(e => e.UserId == userId)
                                    .GroupBy(e => e.Date.ToString("yyyy-MM"))
                                     .Select(g => new ExpenseSummaryDto
                                    {
                                        Group = g.Key,
                                        TotalAmount = g.Sum(e => e.Amount)
                                    })
                                    .ToListAsync();
        }

        public async Task<decimal> GetTotalExpensesAsync(string userId)
        {
             return await _context.Expenses
            .Where(e => e.UserId == userId)
            .SumAsync(e => e.Amount);
        }
    }
}