using BlazorCRUDApp.Models.Dtos;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net.Http.Json;

namespace BlazorCRUDApp.Services;

public class SubCategoryService : ISubCategoryService
{
    private const string RequestUrl = "api/sub-category";
    private readonly HttpClient _httpClient;

    public SubCategoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task AddAsync(SubCategoryDto subCategoryDto)
    {
        var response = await _httpClient.PostAsJsonAsync(RequestUrl, subCategoryDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{RequestUrl}/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<SubCategoryDto>> GetAllSubCategoriesAsync()
    {
        var response = await _httpClient.GetAsync(RequestUrl);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<SubCategoryDto>>() ?? new List<SubCategoryDto>();
    }

    public async Task<List<SubCategoryDto>> GetSubCategoriesAsync(SubCategoryQueryDto queryDto)
    {
        var queryBuilder = new QueryBuilder
        {
            { nameof(queryDto.Name), queryDto.Name },
            { nameof(queryDto.CategoryUid), queryDto.CategoryUid.ToString() },
        };
        var url = new Uri(new Uri($"{RequestUrl}/query"), queryBuilder.ToString());
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<SubCategoryDto>>() ?? new List<SubCategoryDto>();
    }

    public async Task<SubCategoryDto> GetSubCategoryAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{RequestUrl}/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<SubCategoryDto>() ?? new SubCategoryDto();
    }

    public async Task UpdateAsync(SubCategoryDto subCategoryDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"{RequestUrl}/{subCategoryDto.Id}", subCategoryDto);
        response.EnsureSuccessStatusCode();
    }
}