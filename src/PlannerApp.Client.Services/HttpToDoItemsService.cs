using PlannerApp.Client.Services.Exceptions;
using PlannerApp.Client.Services.Interfaces;
using PlannerApp.Shared.Models;
using PlannerApp.Shared.Responses;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class HttpToDoItemsService : IToDoItemsService
{
    private readonly HttpClient _client;
    public HttpToDoItemsService(HttpClient client)
    {
        _client = client;
    }

    public async Task<ApiResponse<ToDoItemDetail>> CreateAsync(string description, string planId)
    {       
        var response = await _client.PostAsJsonAsync("/api/v2/ToDos", new
        {
            Description = description,
            PlanId = planId
        });
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ToDoItemDetail>>();
            return result;
        }
        else
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new ApiException(errorResponse, response.StatusCode);
        }
    }

    public async Task DeleteAsync(string id)
    {
        var response = await _client.DeleteAsync($"/api/v2/ToDos/{id}");
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new ApiException(errorResponse, response.StatusCode);
        }
    }

    public async Task<ApiResponse<ToDoItemDetail>> EditAsync(string id, string newDescription, string planId)
    {
        var response = await _client.PutAsJsonAsync("/api/v2/ToDos", new
        {
            Description = newDescription,
            PlanId = planId,
            Id = id
        });
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ToDoItemDetail>>();
            return result;
        }
        else
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new ApiException(errorResponse, response.StatusCode);
        }
    }

    public async Task ToggleAsync(string Id)
    {
        var response = await _client.PutAsJsonAsync<object>($"/api/v2/ToDos/Toggle/{Id}", null);
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new ApiException(errorResponse, response.StatusCode);
        }
    }
}