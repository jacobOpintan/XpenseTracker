using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XpenseTracker.Data
{
    public class Expense
    {
        public int id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}