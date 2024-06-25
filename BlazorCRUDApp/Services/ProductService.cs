using BlazorCRUDApp.Models.Dtos;
using System.Net.Http.Json;

namespace BlazorCRUDApp.Services;

public class ProductService : IProductService
{
    private const string RequestUri = "api/product";

    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task AddAsync(ProductDto productDto)
    {
        var response = await _httpClient.PostAsJsonAsync(RequestUri, productDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{RequestUri}/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<ProductDto> GetProductAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{RequestUri}/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ProductDto>() ?? new ProductDto();
    }

    public async Task<List<ProductDto>> GetProductsAsync(ProductQueryDto queryDto)
    {
        var response = await _httpClient.GetAsync($"{RequestUri}/name?name={queryDto.Name}&categoryGuid={queryDto.CategoryGuid}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<ProductDto>>() ?? new List<ProductDto>();
    }

    public async Task<List<ProductDto>> GetProductsAsync()
    {
        var response = await _httpClient.GetAsync(RequestUri);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<ProductDto>>() ?? new List<ProductDto>();
    }

    public async Task UpdateAsync(ProductDto productDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"{RequestUri}/{productDto.Id}", productDto);
        response.EnsureSuccessStatusCode();
    }
}