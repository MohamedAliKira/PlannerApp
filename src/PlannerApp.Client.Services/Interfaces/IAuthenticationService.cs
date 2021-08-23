using PlannerApp.Shared.Models;
using PlannerApp.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Client.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ApiResponse> RegisterUserAsync(RegisterRequest model);
    }
}
