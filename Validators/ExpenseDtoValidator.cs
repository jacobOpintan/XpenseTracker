using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using XpenseTracker.Dtos;




namespace XpenseTracker.Validators
{
    public class ExpenseDtoValidator : AbstractValidator<ExpenseDto>
    {
        public ExpenseDtoValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(150).WithMessage("Title must be under 150 characters");

        RuleFor(e => e.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0");

        RuleFor(e => e.Date)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Date cannot be in the future");

        RuleFor(e => e.CategoryId)
            .GreaterThan(0).WithMessage("Valid category is required");
    }

    }
}