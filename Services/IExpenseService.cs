using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XpenseTracker.Dtos;

namespace XpenseTracker.Services
{
    public interface IExpenseService
    {
        Task<List<ExpenseSummaryDto>> GetMonthlySummaryAsync(string userId);
        Task<List<ExpenseSummaryDto>> GetCategorySummaryAsync(string userId);
        Task<decimal> GetTotalExpensesAsync(string userId);
    }
}