using System.Net.Http.Json;
using PortalGalaxy.Common.Response;
using PortalGalaxy.WebApp.Proxy.Interfaces;

namespace PortalGalaxy.WebApp.Proxy.Services;

public class CrudRestHelperBase<TRequest, TResponse> : RestBase, ICrudRestHelper<TRequest, TResponse>
    where TRequest : class
    where TResponse : class
{
    protected CrudRestHelperBase(string baseUrl,
        HttpClient httpClient)
        : base(baseUrl, httpClient)
    {
    }

    public virtual async Task<ICollection<TResponse>> ListAsync()
    {
        var response = await HttpClient.GetFromJsonAsync<BaseResponse<ICollection<TResponse>>>($"{BaseUrl}");
        if (response is { Success: true, Data: not null })
            return response.Data;

        throw new InvalidOperationException(response!.ErrorMessage);
    }

    public async Task<TRequest> FindByIdAsync(int id)
    {
        var response = await HttpClient.GetFromJsonAsync<BaseResponse<TRequest>>($"{BaseUrl}/{id}");
        if (response is { Data: not null, Success: true })
            return response.Data;

        throw new InvalidOperationException($"Error en la solicitud {response!.ErrorMessage}");
    }

    public async Task CreateAsync<TOutputResponse>(TRequest request) where TOutputResponse : BaseResponse
    {
        var response = await SendAsync<TRequest, TOutputResponse>(request, HttpMethod.Post, string.Empty);

        if (response is { Success: true })
            return;

        throw new InvalidOperationException(response.ErrorMessage);
    }

    public async Task UpdateAsync(int id, TRequest request)
    {
        var response = await SendAsync<TRequest, BaseResponse>(request, HttpMethod.Put, $"{id}");
        if (response is { Success: true })
            return;
        throw new InvalidOperationException(response.ErrorMessage);
    }

    public async Task DeleteAsync(int id)
    {
        var response = await HttpClient.DeleteAsync($"{BaseUrl}/{id}");
        response.EnsureSuccessStatusCode();
    }
}