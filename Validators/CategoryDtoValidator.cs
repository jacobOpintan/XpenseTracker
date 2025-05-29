using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using XpenseTracker.Dtos;


namespace XpenseTracker.Validators;

public class CategoryDtoValidator : AbstractValidator<CategoryDto>
{
    public CategoryDtoValidator()
    {
        RuleFor(c => c.Name)
                            .NotEmpty().WithMessage("Category Name is required")
                            .MaximumLength(100).WithMessage("Category Name can not be more 100 characters");
    }

    
}