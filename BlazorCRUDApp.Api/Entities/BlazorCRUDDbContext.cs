using Microsoft.EntityFrameworkCore;

namespace BlazorCRUDApp.Api.Entities;

public class BlazorCRUDDbContext : DbContext
{
    private const string Collation = "Chinese_Taiwan_Stroke_CS_AS";

    public BlazorCRUDDbContext(DbContextOptions<BlazorCRUDDbContext> options)
        : base(options)
    {
    }

    public DbSet<ProductEntity> Product { get; set; }

    public DbSet<CategoryEntity> Category { get; set; }

    public DbSet<SubCategoryEntity> SubCategory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation(Collation);

        ProductEntity.Settings(modelBuilder);
        CategoryEntity.Settings(modelBuilder);
        SubCategoryEntity.Settings(modelBuilder);
    }
}