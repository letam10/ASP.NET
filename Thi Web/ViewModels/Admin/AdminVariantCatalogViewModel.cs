using System.ComponentModel.DataAnnotations;

namespace TechShop.ViewModels.Admin
{
    public class AdminVariantCatalogViewModel
    {
        [Required]
        public int CategoryId { get; set; }

        [Display(Name = "Màu Sắc")]
        public string? Colors { get; set; } // Xanh/Đỏ/Vàng/Đen

        [Display(Name = "Dung Lượng")]
        public string? Capacities { get; set; } // 256GB/512GB/1TB

        [Display(Name = "Nâng Cấp")]
        public string? Upgrades { get; set; } // Plus/Pro/Pro Max
    }
}