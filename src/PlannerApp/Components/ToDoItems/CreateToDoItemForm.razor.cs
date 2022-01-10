using Microsoft.AspNetCore.Components;
using PlannerApp.Shared;
using PlannerApp.Client.Services.Interfaces;
using PlannerApp.Client.Services.Exceptions;
using PlannerApp.Shared.Models;
using AKSoftware.Localization.MultiLanguages;
using AKSoftware.Localization.MultiLanguages.Blazor;

namespace PlannerApp.Components
{
    public partial class CreateToDoItemForm
    {
        [Inject] public IToDoItemsService ToDoItemsService  { get; set; }
        [Parameter] public string PlanId { get; set; }
        [Parameter] public EventCallback<ToDoItemDetail> OnToDoItemAdded { get; set; }
        [CascadingParameter] public Error Error { get; set; }
        [Inject] public ILanguageContainerService Language { get; set; }
        private string _description { get; set; }
        private bool _isBusy = false;
        private string _errorMessage = string.Empty;

        protected override void OnInitialized()
        {
            Language.InitLocalizedComponent(this);
        }
        private async Task AddToDoItemAsync()
        {            
            _errorMessage = string.Empty;
            try
            {
                if(string.IsNullOrWhiteSpace(_description))
                {
                    _errorMessage = Language["DescriptionRequired"];
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