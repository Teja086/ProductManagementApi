namespace ProductApi.DTOs
{
    public class BaseProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int StockAvailable { get; set; }
        public decimal Price { get; set; }
    }
}
