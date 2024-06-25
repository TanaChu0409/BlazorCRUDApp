using BlazorCRUDApp.Models.Dtos;

namespace BlazorCRUDApp.Services;

public interface ISubCategoryService
{
    Task<SubCategoryDto> GetSubCategoryAsync(int id);

    Task<List<SubCategoryDto>> GetAllSubCategoriesAsync();

    Task<List<SubCategoryDto>> GetSubCategoriesAsync(SubCategoryQueryDto queryDto);

    Task AddAsync(SubCategoryDto subCategoryDto);

    Task UpdateAsync(SubCategoryDto subCategoryDto);

    Task DeleteAsync(int id);
}