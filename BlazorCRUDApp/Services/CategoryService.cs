using BlazorCRUDApp.Models.Dtos;
using System.Net.Http.Json;

namespace BlazorCRUDApp.Services;

public class CategoryService : ICategoryService
{
    private const string RequestUrl = "api/category";
    private readonly HttpClient _httpClient;

    public CategoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task AddAsync(CategoryDto categoryDto)
    {
        var response = await _httpClient.PostAsJsonAsync(RequestUrl, categoryDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{RequestUrl}/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<CategoryDto>> GetAllCategoriesAsync()
    {
        var response = await _httpClient.GetAsync(RequestUrl);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<CategoryDto>>() ?? new List<CategoryDto>();
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync(CategoryQueryDto queryDto)
    {
        var response = await _httpClient.GetAsync($"{RequestUrl}/name?name={queryDto.Name}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<CategoryDto>>() ?? new List<CategoryDto>();
    }

    public async Task<CategoryDto> GetCategoryAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{RequestUrl}/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CategoryDto>() ?? new CategoryDto();
    }

    public async Task UpdateAsync(CategoryDto categoryDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"{RequestUrl}/{categoryDto.Id}", categoryDto);
        response.EnsureSuccessStatusCode();
    }
}