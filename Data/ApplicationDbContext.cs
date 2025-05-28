using Microsoft.EntityFrameworkCore;



namespace XpenseTracker.Data;

public class ApplicationDbContext : DbContext
{
 public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Category>().HasMany(c => c.Expenses).WithOne(e => e.Category).HasForeignKey(e => e.CategoryId);
    }
}