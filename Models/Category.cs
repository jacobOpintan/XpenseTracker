using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XpenseTracker.Data
{
    public class Category
    {
        public int id { get; set; }
        public string Name { get; set; }

        public Icollection<Expense> Expenses { get; set; }
    }
}