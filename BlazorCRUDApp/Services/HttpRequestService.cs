using BlazorCRUDApp.Models.Dtos;
using BlazorCRUDApp.Models.Enums;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace BlazorCRUDApp.Services;

public class HttpRequestService : IHttpRequestService
{
    private const string JsonMIMEWithUTF8Type = "application/json";
    private readonly HttpClient _httpClient;

    public HttpRequestService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TResult?> CallHttpRequestAsync<TBody, TResult>(HttpRequestDto<TBody> httpRequestDto)
        where TResult : class
    {
        if (string.IsNullOrWhiteSpace(httpRequestDto.ContentType))
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMIMEWithUTF8Type));
        }
        else
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(httpRequestDto.ContentType));
        }

        if (httpRequestDto.Header != null)
        {
            foreach (var item in httpRequestDto.Header)
            {
                _httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
        }

        HttpResponseMessage? response = null;
        HttpContent _body;
        switch (httpRequestDto.HTTPMethods)
        {
            case HTTPMethods.GET:
                response = await _httpClient.GetAsync(httpRequestDto.Url);
                response.EnsureSuccessStatusCode();
                break;

            case HTTPMethods.PUT:
                response = await _httpClient.PutAsJsonAsync(httpRequestDto.Url, httpRequestDto.Body);
                response.EnsureSuccessStatusCode();
                break;

            case HTTPMethods.POST:
                response = await _httpClient.PostAsJsonAsync(httpRequestDto.Url, httpRequestDto.Body);
                response.EnsureSuccessStatusCode();
                break;

            case HTTPMethods.PATCH:
                var requestBodyJson = JsonSerializer.Serialize(httpRequestDto.Body);
                _body = new StringContent(requestBodyJson, Encoding.UTF8);
                response = await _httpClient.PatchAsync(httpRequestDto.Url, _body);
                response.EnsureSuccessStatusCode();
                break;

            case HTTPMethods.DELETE:
                response = await _httpClient.DeleteAsync(httpRequestDto.Url);
                response.EnsureSuccessStatusCode();
                break;
        }

        if (response != null)
        {
            var responseObject = await response.Content.ReadFromJsonAsync<TResult>();
            return responseObject;
        }
        else
        {
            return default;
        }
    }

    public async Task<bool> CallHttpRequestForCheckAsync(HttpRequestDto httpRequestDto)
    {
        if (string.IsNullOrWhiteSpace(httpRequestDto.ContentType))
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMIMEWithUTF8Type));
        }
        else
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(httpRequestDto.ContentType));
        }

        if (httpRequestDto.Header != null)
        {
            foreach (var item in httpRequestDto.Header)
            {
                _httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
        }

        var response = await _httpClient.GetAsync(httpRequestDto.Url);
        response.EnsureSuccessStatusCode();

        var isChecked = await response.Content.ReadAsStringAsync() == "true";
        return isChecked;
    }
}