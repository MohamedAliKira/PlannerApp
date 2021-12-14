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