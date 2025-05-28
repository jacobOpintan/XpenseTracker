using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XpenseTracker.Data
{
    public class Expense
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Expense description cannot exceed 200 characters.")]
        [Display(Name = "Expense Description")]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }


    }
}