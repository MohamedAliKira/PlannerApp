using System.IO;

namespace PlannerApp.Shared.Models
{
    public class FormFile
    {
        public FormFile(Stream stream, string fileName)
        {
            FileName = fileName;
            FileStream = stream;
        }
        public Stream FileStream{ get; set; }
        public string FileName { get; set; }
    }
}
