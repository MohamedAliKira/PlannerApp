using AKSoftware.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using PlannerApp.Client.Services.Exceptions;
using PlannerApp.Client.Services.Interfaces;
using PlannerApp.Shared;
using PlannerApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.Components
{
    public partial class PlanList
    {
        [Inject] public IPlansService PlanService { get; set; }
        [Inject] public NavigationManager Navigation { get; set; }
        [Inject] public IDialogService DialogService { get; set; }
        [CascadingParameter] public Error Error { get; set; }

        //private bool _isBusy;
        private string _errorMessage = string.Empty;
        private int _pageNumber = 1;
        private int _pageSize = 12;
        private int _totalPages = 1;
        private List<PlanSummary> _plans = new();

        private async Task<PageList<PlanSummary>> GetPlansAsync(string query = "", int pageNumber = 1, int pageSize = 12)
        {
            //_isBusy = true;
            try
            {
                var result = await PlanService.GetPlansAsync(query, pageNumber, pageSize);
                _plans = result.Value.Records.ToList();
                _pageNumber = result.Value.Page;
                _pageSize = result.Value.PageSize;
                _totalPages = result.Value.TotalPages;
                return result.Value;
            }
            catch(ApiException e)
            {
                _errorMessage = e.ApiErrorResponse.Message;
            }
            catch (Exception ex)
            {
                Error.HandleError(ex);
            }
            //_isBusy = false;
            return null;
        }

        #region ViewToggler
        private bool _isCardsViewEnabled = true;
        private void setCardView() => _isCardsViewEnabled = true;
        private void setTableView() => _isCardsViewEnabled = false;
        #endregion

        #region Edit
        private void EditPlan(PlanSummary plan)
        {
            Navigation.NavigateTo($"/plans/form/{plan.Id}");
        }
        #endregion

        #region Delete
        private async Task DeletePlanAsync(PlanSummary plan)
        {
            var parameters = new DialogParameters();
            parameters.Add("ContentText", $"Do you really want to delete these plan '{plan.Title}'?");
            parameters.Add("ButtonText", "Delete");
            parameters.Add("Color", Color.Error);

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var dialog = DialogService.Show<ConfirmationDialog>("Delete", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                try
                {
                    await PlanService.DeleteAsync(plan.Id);

                    //Send a message about the deleted plan
                    MessagingCenter.Send(this, "plan_deleted", plan);
                }
                catch(ApiException ex)
                {
                    _errorMessage = ex.Message;
                    throw;
                }
                catch (Exception e)
                {
                    _errorMessage = e.Message;
                    throw;
                }
                
            }
        }
        #endregion

        #region View
        private void ViewPlan(PlanSummary plan)
        {
            var parameters = new DialogParameters();
            parameters.Add("PlanId", plan.Id);

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };

            var dialog = DialogService.Show<PlanDetailsDialog>("Details", parameters, options);
        }
        #endregion
    }
}
