namespace FoodprojectApi.Models
{
    public class FileModel
    {
        public string FileName { get; set; }
        public IFormFile file { get; set; }
        public string Text { get; set; } // Added Text property
    }
}
