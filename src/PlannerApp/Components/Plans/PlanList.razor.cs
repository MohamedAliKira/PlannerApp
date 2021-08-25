﻿using Microsoft.AspNetCore.Components;
using PlannerApp.Client.Services.Exceptions;
using PlannerApp.Client.Services.Interfaces;
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

        private bool _isBusy = false;
        private string _errorMessage = string.Empty;
        private int _pageNumber = 1;
        private int _pageSize = 12;
        private int _totalPages = 1;
        private List<PlanSummary> _plans = new();

        private async Task<PageList<PlanSummary>> GetPlansAsync(string query = "", int pageNumber = 1, int pageSize = 12)
        {
            _isBusy = true;
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
                _errorMessage = ex.Message;
            }
            _isBusy = false;
            return null;
        }
    }
}