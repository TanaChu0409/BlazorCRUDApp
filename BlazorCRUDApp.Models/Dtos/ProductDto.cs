using System.ComponentModel.DataAnnotations;

namespace BlazorCRUDApp.Models.Dtos;

public class ProductDto
{
    [Display(Name = "Product Id")]
    public int Id { get; set; }

    [Display(Name = "Product Name")]
    public string? Name { get; set; }

    [Display(Name = "Product Amount")]
    public decimal? Amount { get; set; }

    [Display(Name = "Product Price")]
    public decimal? Price { get; set; }

    public Guid? CategoryGuid { get; set; }

    public string? CategoryName { get; set; }
}