using BlazorCRUDApp.Models.Dtos;

namespace BlazorCRUDApp.Services;

public interface ICategoryService
{
    Task<CategoryDto> GetCategoryAsync(int id);

    Task<List<CategoryDto>> GetAllCategoriesAsync();

    Task<List<CategoryDto>> GetCategoriesAsync(CategoryQueryDto queryDto);

    Task AddAsync(CategoryDto categoryDto);

    Task UpdateAsync(CategoryDto categoryDto);

    Task DeleteAsync(int id);
}