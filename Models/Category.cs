using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace XpenseTracker.Data
{
    public class Category
    {

        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        public ICollection<Expense> Expenses { get; set; }
    }
}