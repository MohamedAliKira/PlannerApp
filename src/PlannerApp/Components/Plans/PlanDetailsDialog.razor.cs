using Microsoft.AspNetCore.Components;
using MudBlazor;
using PlannerApp.Client.Services.Interfaces;
using PlannerApp.Shared.Models;
using PlannerApp.Client.Services.Exceptions;
using PlannerApp.Shared;

namespace PlannerApp.Components
{
    public partial class PlanDetailsDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Inject] public IPlansService PlansService { get; set; }
        [Parameter] public string PlanId { get; set; }
        [CascadingParameter] public Error Error { get; set; }

        private PlanDetail _plan;
        private bool _isBusy;
        private string _errorMessage = string.Empty;
        private List<ToDoItemDetail> _items = new();

        private void Close()
        {
            MudDialog.Cancel();
        }

        protected override void OnParametersSet()
        {
            if(PlanId == null)
                throw new ArgumentNullException(nameof(PlanId));
            base.OnParametersSet();
        }

        protected override async Task OnInitializedAsync()
        {
            await FetchPlanAsync();
        }
        private async Task FetchPlanAsync()
        {
            _isBusy = true;
            try
            {
                var result = await PlansService.GetByIdAsync(PlanId);
                _plan = result.Value;
                _items = _plan.ToDoItems;
                StateHasChanged();
            }
            catch(ApiException e)
            {
                // TODO
                _errorMessage = e.Message;
            }
            catch (Exception ex)
            {
                Error.HandleError(ex);
            }
            _isBusy = false;
        }

        private void OnToDoItemAddedCallback(ToDoItemDetail toDoItem)
        {
            _items.Add(toDoItem);
        }
        private void OnItemDeletedCallback(ToDoItemDetail toDoItem)
        {
            _items.Remove(toDoItem);
        }

        private void OnItemEditedCallback(ToDoItemDetail toDoItem)
        {
            var editedItem = _items.SingleOrDefault(i => i.Id == toDoItem.Id);
            editedItem.Description = toDoItem.Description;
            editedItem.IsDone = toDoItem.IsDone;
        }

    }
}