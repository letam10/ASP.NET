namespace TechShop.Models
{
    public class ProductVariantGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Màu sắc, RAM, Ổ cứng...
        public List<ProductVariantOption> Options { get; set; } = new();
    }
}