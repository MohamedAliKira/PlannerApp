using Microsoft.AspNetCore.Components;
using MudBlazor;
using PlannerApp.Client.Services.Interfaces;
using PlannerApp.Shared.Models;
using PlannerApp.Client.Services.Exceptions;

namespace PlannerApp.Components
{
    public partial class PlanDetailsDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Inject] public IPlansService PlansService { get; set; }
        [Parameter] public string PlanId { get; set; }

        private PlanDetail _plan;
        private bool _isBusy;
        private string _errorMessage = string.Empty;

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
            }
            catch(ApiException e)
            {
                // TODO
            }
            catch (Exception ex)
            {
                // Log this error
            }
            _isBusy = false;
        }
    }
}