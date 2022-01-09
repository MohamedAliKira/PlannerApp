using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PlannerApp.Client.Services.Exceptions;
using PlannerApp.Client.Services.Interfaces;
using PlannerApp.Shared;
using PlannerApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.Components
{
    public partial class PlanForm
    {
        [Inject] public IPlansService PlansService { get; set; }
        [Inject] public NavigationManager Navigation { get; set; }
        [Parameter] public string Id { get; set; }
        [CascadingParameter] public Error Error { get; set; }

        private bool _isEditMode => Id != null;
        private PlanDetail _model = new();
        private bool _isBusy = false;
        private Stream _stream = null;
        private string _fileName = string.Empty;
        private string _errorMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            if (_isEditMode)
                await FetchPlanByIdAsync();
        }

        private async Task SubmitFormAsync()
        {
            _isBusy = true;
            try
            {
                FormFile formFile = null;
                if (_stream != null)
                    formFile = new FormFile(_stream, _fileName);

                if(_isEditMode)
                    await PlansService.EditAsync(_model, formFile);
                else 
                    await PlansService.CreateAsync(_model, formFile);
                
                //success
                Navigation.NavigateTo("/plans");
            }
            catch (ApiException e)
            {
                _errorMessage = e.ApiErrorResponse.Message;
            }
            catch (Exception ex)
            {
                Error.HandleError(ex);  
            }
            _isBusy = false;
        }

        private async Task FetchPlanByIdAsync()
        {
            _isBusy = true;
            try
            {
                var result = await PlansService.GetByIdAsync(Id);
                _model = result.Value;
            }
            catch (ApiException ex)
            {
                _errorMessage = ex.ApiErrorResponse.Message;
            }
            catch (Exception e)
            {
                //TODO : Log the error
                _errorMessage = e.Message;
            }
            _isBusy = false;
        }

        private async Task OnChooseFileAsync(InputFileChangeEventArgs e)
        {
            _errorMessage = string.Empty;
            var file = e.File;
            if (file != null)
            {
                if (file.Size > 2097152)
                {
                    _errorMessage = "The file must be equal or less than 2 MB";
                    return;
                }
                string[] allowedExtension = new[] { ".jpg", ".png", ".bmp", ".svg" };
                string extension = Path.GetExtension(file.Name).ToLower();
                if (!allowedExtension.Contains(extension))
                {
                    _errorMessage = "Please choose a valid image file";
                    return;
                }
                using (var stream = file.OpenReadStream(2097152))
                {
                    var buffer = new byte[file.Size];
                    await stream.ReadAsync(buffer, 0, (int)file.Size);
                    _stream = new MemoryStream(buffer);
                    _stream.Position = 0;
                    _fileName = file.Name;
                }
            }

        }
    }
}
