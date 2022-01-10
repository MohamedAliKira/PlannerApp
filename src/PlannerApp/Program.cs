using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;
using PlannerApp.Client.Services;
using PlannerApp;
using AKSoftware.Localization.MultiLanguages;
using System.Reflection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("PlannerApp.Api", client =>
{
    client.BaseAddress = new Uri("https://plannerapp-api.azurewebsites.net/");
}).AddHttpMessageHandler<AuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetService<IHttpClientFactory>().CreateClient("PlannerApp.Api"));
builder.Services.AddTransient<AuthorizationMessageHandler>();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthentificationStateProvider>();
builder.Services.AddHttpClientServices();
builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly());

await builder.Build().RunAsync();

