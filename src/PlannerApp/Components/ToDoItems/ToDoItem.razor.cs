using Microsoft.AspNetCore.Components;
using PlannerApp.Shared.Models;
using PlannerApp.Client.Services.Interfaces;
using PlannerApp.Client.Services.Exceptions;
using PlannerApp.Shared;
using AKSoftware.Localization.MultiLanguages;
using AKSoftware.Localization.MultiLanguages.Blazor;

namespace PlannerApp.Components
{
    public partial class ToDoItem
    {
        [Inject] public IToDoItemsService ToDoItemsService { get; set; }
        [Parameter] public ToDoItemDetail Item { get; set; }
        [Parameter] public EventCallback<ToDoItemDetail> OnItemDeleted { get; set; }
        [Parameter] public EventCallback<ToDoItemDetail> OnItemEdited { get; set; }
        [CascadingParameter] public Error Error { get; set; }
        [Inject] public ILanguageContainerService Language { get; set; }

        private bool _isChecked = true;
        private bool _isBusy = false;
        private bool _isEditMode = false;
        private string _description = string.Empty;
        private string _errorMessage = string.Empty;
        private string _descriptionStyle => $"cursor:pointer; {(_isChecked ? "text-decoration:line-through" : "")}";
        protected override void OnInitialized()
        {
            Language.InitLocalizedComponent(this);
            _isChecked = Item.IsDone;
        }
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
                
                //Notify the parent about the deleted item
                await OnItemDeleted.InvokeAsync(Item);
            }
            catch (ApiException e)
            {
                _errorMessage = e.Message;
            }
            catch (Exception ex)
            {
                //TODO: Handler error globaly
                Error.HandleError(ex);
            }
            _isBusy = false;
        }
        private async Task EditItemAsync()
        {
            _errorMessage = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(_description))
                {
                    _errorMessage = Language["DescriptionRequired"];
                    return;
                }
                //Call the API to add the item
                _isBusy = true;
                var result = await ToDoItemsService.EditAsync(Item.Id, _description, Item.PlanId);
                ToggleEditMode(false);

                //Notify the parent about the newly edited item
                await OnItemEdited.InvokeAsync(result.Value);
                
            }
            catch (ApiException e)
            {
                _errorMessage = e.Message;
            }
            catch (Exception ex)
            {
                //TODO: Handler error globaly
                Error.HandleError(ex);
            }
            _isBusy = false;
        }
        private async Task ToggleItemAsync(bool value)
        {
            _errorMessage = string.Empty;
            try
            {                
                //Call the API to add the item
                _isBusy = true;
                await ToDoItemsService.ToggleAsync(Item.Id);                
                Item.IsDone = !Item.IsDone;
                _isChecked = Item.IsDone;

                //Notify the parent about the newly edited item
                await OnItemEdited.InvokeAsync(Item);

            }
            catch (ApiException e)
            {
                _errorMessage = e.Message;
            }
            catch (Exception ex)
            {
                //TODO: Handler error globaly
               Error.HandleError(ex);
            }
            _isBusy = false;
        }
    }
}