using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XpenseTracker.Dtos
{
    public record ExpenseSummaryDto
    {
        public string Group { get; set; }
        public decimal TotalAmount {get; set; }
    }
}