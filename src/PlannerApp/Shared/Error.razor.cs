using Microsoft.AspNetCore.Components;
using MudBlazor;
using AKSoftware.Localization.MultiLanguages;
using AKSoftware.Localization.MultiLanguages.Blazor;

namespace PlannerApp.Shared
{
    public partial class Error
    {
        [Inject] public ISnackbar Snackbar { get; set; }
        
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Inject] public ILanguageContainerService Language { get; set; }

        protected override void OnInitialized()
        {
            Language.InitLocalizedComponent(this);
        }
        public void HandleError(Exception ex)
        {
            Snackbar.Add(Language["GeneralError"], Severity.Error);

            // TODO: Log the error, send the server, to application,
            Console.WriteLine($"{ex.Message} - {DateTime.Now}");
        }
    }
}