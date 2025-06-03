using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using XpenseTracker.Data;
using XpenseTracker.Dtos;

namespace XpenseTracker.Controllers;

[Route("api/category")]
[ApiController]
public class CategoryController : Controller
{
    private readonly ILogger<CategoryController> _logger;
    private readonly ApplicationDbContext _context;

    public CategoryController(ILogger<CategoryController> logger, ApplicationDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    //Get : api/category
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        _logger.LogInformation("Request made to retrieve all categories");
        return await _context.Categories.ToListAsync();
    }
    // Get : api/category/ id get category by id
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategoryById(int id,CategoryDto categoryDto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return category;

    }

    //Post : api/category 
    public async Task<ActionResult<Category>> CreateCategory([FromBody]CategoryDto categoryDto)
    {
        var category = new Category { Name =categoryDto.Name};

        if (category == null)
        {
            return BadRequest("Category cannot be Empty");
        }
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
    }

    // put : api/category/id
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, Category category)
    {
        if (id != category.Id)
        {
            return BadRequest("Category Id mismatch");
        }

        _context.Entry(category).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Categories.Any(e => e.Id == id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    //Delete : api/category/id delete a category based on id
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }






}