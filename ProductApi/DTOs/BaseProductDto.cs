using System.ComponentModel.DataAnnotations;

namespace ProductApi.DTOs
{
    public class BaseProductDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be zero or more.")]
        public int StockAvailable { get; set; }

        [Required]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
    }
}
