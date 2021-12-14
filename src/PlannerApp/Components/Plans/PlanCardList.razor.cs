using AKSoftware.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using PlannerApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.Components
{
    public partial class PlanCardList
    {
        [Parameter] public Func<string, int, int, Task<PageList<PlanSummary>>> FetchPlans { get; set; }
        [Inject] public NavigationManager Navigation { get; set; }

        [Parameter] public EventCallback<PlanSummary> OnViewClicked { get; set; }
        [Parameter] public EventCallback<PlanSummary> OnEditClicked { get; set; }
        [Parameter] public EventCallback<PlanSummary> OnDeleteClicked { get; set; }
        private bool _isBusy{ get; set; }
        private string _query = string.Empty;
        private int _pageNumber = 1;
        private int _pageSize = 12;
        private PageList<PlanSummary> _result = new();

        protected override void OnInitialized()
        {
            MessagingCenter.Subscribe<PlanList, PlanSummary>(this, "plan_deleted", async (sender, args) =>
            {
                await GetPlansAsync(_pageNumber);
                StateHasChanged();
            });
        }

        protected async override Task OnInitializedAsync()
        {
            await GetPlansAsync();
        } 

        private async Task GetPlansAsync(int pageNumber = 1)
        {
            _isBusy = true;
            _pageNumber = pageNumber;            
            _result = await FetchPlans?.Invoke(_query, _pageNumber, _pageSize);
            _isBusy = false;
        }

        
    } 
}
