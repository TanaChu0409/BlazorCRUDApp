using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCRUDApp.Api.Entities
{
    [Table("Product")]
    public class ProductEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid? CategoryGuid { get; set; }

        [Required]
        [MaxLength(500)]
        [Unicode(true)]
        public string Name { get; set; } = default!;

        [Required]
        [Precision(10, 0)]
        public decimal Amonut { get; set; }

        [Required]
        [Precision(10, 0)]
        public decimal Price { get; set; }

        [Required]
        public DateTime LastUpdateDate { get; set; }

        public CategoryEntity Category { get; set; } = default!;

        public static void Settings(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductEntity>()
                .Property(x => x.LastUpdateDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<ProductEntity>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasPrincipalKey(x => x.Uuid)
                .HasForeignKey(x => x.CategoryGuid);
        }
    }
}