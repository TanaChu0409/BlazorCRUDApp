using BlazorCRUDApp.Models.Dtos;

namespace BlazorCRUDApp.Services;

public interface IHttpRequestService
{
    Task<TResult?> CallHttpRequestAsync<TBody, TResult>(HttpRequestDto<TBody> httpRequestDto)
            where TResult : class;

    Task<bool> CallHttpRequestForCheckAsync(HttpRequestDto httpRequestDto);
}