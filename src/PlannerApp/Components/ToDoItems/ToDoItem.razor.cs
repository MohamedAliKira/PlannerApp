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
using PlannerApp.Shared.Models;
using PlannerApp.Client.Services.Interfaces;
using PlannerApp.Client.Services.Exceptions;

namespace PlannerApp.Components
{
    public partial class ToDoItem
    {
        [Inject] public IToDoItemsService ToDoItemsService { get; set; }
        [Parameter] public ToDoItemDetail Item { get; set; }
        [Parameter] public EventCallback<ToDoItemDetail> OnItemDeleted { get; set; }
        private bool _isChecked = true;
        private bool _isBusy = false;
        private bool _isEditMode = false;
        private string _description = string.Empty;
        private void ToggleEditMode(bool isCancel)
        {
            if (_isEditMode)
            {
                _isEditMode = false;
                _description = isCancel ? Item.Description : _description;
            }
            else
            {
                _isEditMode = true;
                _description = Item.Description;
            }
        }

        private async Task RemoveItemAsync()
        {
            try
            {               
                //Call the API to add the item
                _isBusy = true;
                await ToDoItemsService.DeleteAsync(Item.Id);
                
                //Notify the parent about the newly added item
                await OnItemDeleted.InvokeAsync(Item);
            }
            catch (ApiException e)
            {
                
            }
            catch (Exception ex)
            {
                //TODO: Handler error globaly
                
            }
            _isBusy = false;
        }
    }
}