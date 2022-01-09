using AKSoftware.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using PlannerApp.Client.Services.Interfaces;
using PlannerApp.Shared;
using PlannerApp.Shared.Models;
using System.Threading.Tasks;

namespace PlannerApp.Components
{
    public partial class PlansTable
    {
        [Inject] public IPlansService PlansService { get; set; }
        [Parameter] public EventCallback<PlanSummary> OnViewClicked { get; set; }
        [Parameter] public EventCallback<PlanSummary> OnEditClicked { get; set; }
        [Parameter] public EventCallback<PlanSummary> OnDeleteClicked { get; set; }
        [CascadingParameter] public Error Error { get; set; }

        private string _query = string.Empty;
        private MudTable<PlanSummary> _table;

        protected override void OnInitialized()
        {
            MessagingCenter.Subscribe<PlanList, PlanSummary>(this, "plan_deleted", async (sender, args) =>
              {
                  await _table.ReloadServerData();
                  StateHasChanged();
              });
        }
        private async Task<TableData<PlanSummary>> ServerReloadAsync(TableState state)
        {
            try
            {
                var result = await PlansService.GetPlansAsync(_query, state.Page, state.PageSize);
                return new TableData<PlanSummary>
                {
                    Items = result.Value.Records,
                    TotalItems = result.Value.ItemsCount
                };
            }
            catch (Exception ex)
            {
                Error.HandleError(ex);
            }
            return new TableData<PlanSummary>
            {
                Items = new List<PlanSummary>(),
                TotalItems = 0
            };
        }

        private void OnSearch(string query)
        {
            _query = query;
            _table.ReloadServerData();
        }
    }
}
