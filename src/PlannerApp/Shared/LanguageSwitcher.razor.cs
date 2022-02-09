using Microsoft.AspNetCore.Components;
using AKSoftware.Localization.MultiLanguages;
using Blazored.LocalStorage;
using System.Globalization;

namespace PlannerApp.Shared
{
    public partial class LanguageSwitcher
    {
        [Inject] public ILanguageContainerService Language { get; set; }   
        [Inject] public ILocalStorageService LocalStorage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if(await LocalStorage.ContainKeyAsync("language"))
            {
                string cultureCode = await LocalStorage.GetItemAsStringAsync("language");
                Language.SetLanguage(CultureInfo.GetCultureInfo(cultureCode));
            }
            Console.WriteLine($"OnInitializedAsync for LanguageSwither : {Language.CurrentCulture}");
        }

        private async Task ChangeLanguageAsync(string cultureCode)
        {
            Language.SetLanguage(CultureInfo.GetCultureInfo(cultureCode));
            await LocalStorage.SetItemAsStringAsync("language",cultureCode);
            //StateHasChanged();
            Console.WriteLine($"ChangeLanguageAsync : {cultureCode}");
        }
    }
}