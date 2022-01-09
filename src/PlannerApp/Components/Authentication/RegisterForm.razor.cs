using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PlannerApp.Client.Services.Exceptions;
using PlannerApp.Client.Services.Interfaces;
using PlannerApp.Shared;
using PlannerApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlannerApp.Components
{
    public partial class RegisterForm : ComponentBase
    {
        [Inject] public IAuthenticationService AuthenticationService { get; set; }
        [Inject] public NavigationManager Navigation { get; set; }
        [CascadingParameter] public Error Error { get; set; }

        private RegisterRequest _model = new();
        private bool _isBusy = false;
        private string _errorMessage = string.Empty;

        private async Task RegisterUserAsync()
        {
            _isBusy = true;
            _errorMessage = string.Empty;

            try
            {                
                await AuthenticationService.RegisterUserAsync(_model);
                Navigation.NavigateTo("/");
            }
            catch (ApiException ex)
            {
                // Handle the errors of API
                _errorMessage = ex.ApiErrorResponse.Message;
            }
            catch (Exception e)
            {
                Error.HandleError(e);
            }

            _isBusy = false;
        }
        private void RedirectToLogin() => Navigation.NavigateTo("/");
    }
}
