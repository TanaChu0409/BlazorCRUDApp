using BlazorCRUDApp.Models.Enums;

namespace BlazorCRUDApp.Models.Dtos;
public record HttpRequestDto<T>(string Url, HTTPMethods HTTPMethods, Dictionary<string, string> Header, T Body, string? ContentType);
public record HttpRequestDto(string Url, HTTPMethods HTTPMethods, Dictionary<string, string> Header, string? ContentType);