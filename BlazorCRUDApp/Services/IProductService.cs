using BlazorCRUDApp.Models.Dtos;

namespace BlazorCRUDApp.Services;

public interface IProductService
{
    Task<ProductDto> GetProductAsync(int id);

    Task<List<ProductDto>> GetProductsAsync();

    Task<List<ProductDto>> GetProductsAsync(ProductQueryDto queryDto);

    Task AddAsync(ProductDto productDto);

    Task UpdateAsync(ProductDto productDto);

    Task DeleteAsync(int id);
}