using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace PlannerApp.Shared.Models
{
    public class PlanDetail : PlanSummary
    {
        // List of the ToDos
        public IFormFile CoverFile { get; set; }

        public List<ToDoItemDetail> ToDoItems { get; set; }
    }

}
