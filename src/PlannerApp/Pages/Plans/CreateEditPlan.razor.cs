using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace PlannerApp.Pages
{
    public partial class CreateEditPlan
    {
        [Parameter] public string Id { get; set; }

        private List<BreadcrumbItem> _breadcrumbItems = new()
        {
            new BreadcrumbItem("Home", "/index"),
            new BreadcrumbItem("Plans", "/plans"),
            new BreadcrumbItem("Create", "/plans/form", true)
        };
    }
}
