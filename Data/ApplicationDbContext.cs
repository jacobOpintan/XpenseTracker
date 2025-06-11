using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XpenseTracker.Models;



namespace XpenseTracker.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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
        modelBuilder.Entity<Expense>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .IsRequired();
    }
}