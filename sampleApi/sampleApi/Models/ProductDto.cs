using System.ComponentModel.DataAnnotations;

namespace sampleApi.Models
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; } = "";
        [Required]
        public int Age { get; set; }
    }
}
