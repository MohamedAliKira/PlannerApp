using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace PlannerApp.Shared
{
    public partial class MainLayout
    {
        [Inject] ILocalStorageService LocalStorage { get; set; }

        bool _drawerOpen = true;
        
        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }
        protected async override Task OnInitializedAsync()
        {
            if (await LocalStorage.ContainKeyAsync("theme"))
                _themeName = await LocalStorage.GetItemAsStringAsync("theme");
                
            _currentTheme = _themeName == "light" ? _lightTheme : _darkTheme; //_darkTheme;
        }

        MudTheme _currentTheme = null;

        MudTheme _darkTheme = new MudTheme
        {
            Palette = new Palette
            {
                Black = "#27282f",
                Background = "#32343d",
                BackgroundGrey = "#27282f",
                Surface = "#575967",
                TextPrimary = "#ffffff",
                TextSecondary = "rgba(255,255,255, 0.50)",
                DrawerBackground = "#000000",
                DrawerText = "#ffffff",
                Primary = "#007CD1",
                AppbarBackground = "#181818",
                AppbarText = "#ffffff",
                ActionDisabled = "rgba(255,255,255, 0.30)",
                TextDisabled = "rgba(255,255,255, 0.30)"
            }
        };

        MudTheme _lightTheme = new MudTheme();

        private string _themeName = "light";
        
        private async Task ToggleThemeAsync()
        {
            if(_themeName == "light")
            {
                _currentTheme = _darkTheme;
                _themeName = "dark";
            }
            else
            {
                _currentTheme = _lightTheme;
                _themeName = "light";                
            }

            await LocalStorage.SetItemAsStringAsync("theme", _themeName);
        }
    }
}