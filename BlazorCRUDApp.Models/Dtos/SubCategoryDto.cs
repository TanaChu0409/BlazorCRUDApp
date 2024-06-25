using System.ComponentModel.DataAnnotations;

namespace BlazorCRUDApp.Models.Dtos;

public class SubCategoryDto
{
    [Editable(false)]
    public int Id { get; set; }

    [Editable(false)]
    public Guid? Uuid { get; set; }

    [Editable(false)]
    public Guid? CategoryUid { get; set; }

    [Editable(false)]
    public string? CategoryName { get; set; }

    [Required]
    [StringLength(500)]
    public string? Name { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [Editable(false)]
    public DateTime? LastUpdateDate { get; set; }
}