using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using PlannerApp;
using PlannerApp.Shared;
using PlannerApp.Components;
using MudBlazor;
using Blazored.FluentValidation;
using PlannerApp.Pages.Authentication;
using PlannerApp.Client.Services.Interfaces;
using PlannerApp.Client.Services.Exceptions;
using PlannerApp.Shared.Models;

namespace PlannerApp.Components
{
    public partial class CreateToDoItemForm
    {
        [Inject] public IToDoItemsService ToDoItemsService  { get; set; }
        [Parameter] public string PlanId { get; set; }
        [Parameter] public EventCallback<ToDoItemDetail> OnToDoItemAdded { get; set; }
        [CascadingParameter] public Error Error { get; set; }
        private string _description { get; set; }
        private bool _isBusy = false;
        private string _errorMessage = string.Empty;

        private async Task AddToDoItemAsync()
        {            
            _errorMessage = string.Empty;
            try
            {
                if(string.IsNullOrWhiteSpace(_description))
                {
                    _errorMessage = "Description is Required";
                    return;
                }
                //Call the API to add the item
                _isBusy = true;
                var result = await ToDoItemsService.CreateAsync(_description, PlanId);
                _description = string.Empty;

                //Notify the parent about the newly added item
                await OnToDoItemAdded.InvokeAsync(result.Value);
            }
            catch (ApiException e)
            {
                _errorMessage = e.Message;
            }
            catch (Exception ex)
            {
                Error.HandleError(ex);
            }
            _isBusy = false;
            
        }
    }
}