namespace BlazorCRUDApp.Models.Dtos;

public class CategoryDto
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public Guid Uuid { get; set; }

    public string? Description { get; set; }
}