using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCRUDApp.Api.Entities;

[Table("Category")]
public class CategoryEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Comment("型錄關連用序列號")]
    public Guid Uuid { get; set; }

    [Required]
    [MaxLength(500)]
    [Unicode]
    public string Name { get; set; } = default!;

    [MaxLength(1000)]
    [Unicode]
    public string? Description { get; set; }

    [Required]
    public DateTime LastUpdateDate { get; set; }

    public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();

    public ICollection<SubCategoryEntity> SubCategories { get; set; } = new List<SubCategoryEntity>();

    public static void Settings(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoryEntity>()
                .Property(x => x.Uuid)
                .HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<CategoryEntity>()
                .Property(x => x.LastUpdateDate)
                .HasDefaultValueSql("GETDATE()");
    }
}