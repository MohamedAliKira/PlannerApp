using PlannerApp.Shared.Models;
using PlannerApp.Shared.Responses;
using System.Threading.Tasks;

namespace PlannerApp.Client.Services.Interfaces
{
    public interface IToDoItemsService
    {        
        Task<ApiResponse<ToDoItemDetail>> CreateAsync(string description, string planId);
        Task<ApiResponse<ToDoItemDetail>> EditAsync(string Id, string newDescription, string planId);
        Task ToggleAsync(string Id);
        Task DeleteAsync(string id);
    }
}
