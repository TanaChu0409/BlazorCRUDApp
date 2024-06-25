using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCRUDApp.Api.Entities;

[Table("SubCategory")]
public class SubCategoryEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Comment("子型錄關連用序列號")]
    public Guid Uuid { get; set; }

    [Comment("主型錄序列號")]
    public Guid CategoryUid { get; set; }

    [Required]
    [MaxLength(500)]
    [Unicode]
    public string Name { get; set; } = default!;

    [MaxLength(1000)]
    [Unicode]
    public string? Description { get; set; }

    [Required]
    public DateTime LastUpdateDate { get; set; }

    public CategoryEntity Category { get; set; } = new CategoryEntity();

    public static void Settings(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SubCategoryEntity>()
                    .Property(x => x.Uuid)
                    .HasDefaultValueSql("NEWID()");

        modelBuilder.Entity<SubCategoryEntity>()
                    .Property(x => x.LastUpdateDate)
                    .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<SubCategoryEntity>()
                    .HasOne(x => x.Category)
                    .WithMany(x => x.SubCategories)
                    .HasPrincipalKey(x => x.Uuid)
                    .HasForeignKey(x => x.CategoryUid);
    }
}