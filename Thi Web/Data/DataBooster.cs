using Microsoft.EntityFrameworkCore;
using TechShop.Models;

namespace TechShop.Data
{
    public static class DataBooster
    {
        public static async Task Boost(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var products = await context.Products
                .Include(p => p.Category)
                .Include(p => p.Specifications)
                .ToListAsync();

            if (!products.Any()) return;

            foreach (var p in products)
            {
                // 1. Update Description if short or null
                if (string.IsNullOrEmpty(p.Description) || p.Description.Length < 50)
                {
                    p.Description = GenerateDescription(p);
                }

                // 2. Add Specifications if missing
                if (!p.Specifications.Any())
                {
                    var specs = GenerateSpecs(p);
                    foreach (var s in specs)
                    {
                        context.ProductSpecifications.Add(new ProductSpecification
                        {
                            ProductId = p.Id,
                            SpecName = s.Key,
                            SpecValue = s.Value
                        });
                    }
                }
            }

            await context.SaveChangesAsync();
        }

        private static string GenerateDescription(Product p)
        {
            string catName = p.Category?.Name ?? "Sản phẩm";
            return $@"Mẫu {p.Name} là dòng {catName} thế hệ mới nhất tại TechShop. 
Sở hữu thiết kế hiện đại, tinh tế cùng hiệu năng đột phá, sản phẩm này đáp ứng hoàn hảo mọi nhu cầu từ học tập, làm việc văn phòng đến thiết kế đồ họa chuyên nghiệp và giải trí đỉnh cao. 
Được chế tác từ vật liệu cao cấp, {p.Name} không chỉ mang lại độ bền vượt trội mà còn khẳng định đẳng cấp của người sở hữu. 
Hệ thống tản nhiệt tối ưu giúp máy luôn duy trì nhiệt độ ổn định ngay cả khi xử lý các tác vụ nặng nhất. 
Sản phẩm đang được phân phối chính hãng tại TechShop với chính sách bảo hành ưu việt và nhiều quà tặng hấp dẫn đi kèm.";
        }

        private static Dictionary<string, string> GenerateSpecs(Product p)
        {
            var specs = new Dictionary<string, string>();
            string cat = p.Category?.Name ?? "";

            if (cat.Contains("Laptop"))
            {
                specs.Add("CPU", "Intel Core i7-13700H (Up to 5.0GHz)");
                specs.Add("RAM", "16GB DDR5 4800MHz");
                specs.Add("SSD", "512GB NVMe PCIe Gen4");
                specs.Add("Màn hình", "15.6 inch FHD (1920x1080) 144Hz");
                specs.Add("Pin", "4-cell, 60Whr");
                specs.Add("Trọng lượng", "1.86 kg");
            }
            else if (cat.Contains("Chuột") || cat.Contains("Bàn phím"))
            {
                specs.Add("Kết nối", "Wireless / Wired (USB-C)");
                specs.Add("LED", "RGB 16.8 triệu màu");
                specs.Add("Cảm biến/Switch", "Premium Optical / Mechanical");
                specs.Add("Pin", "Lên đến 70 giờ liên tục");
            }
            else if (cat.Contains("Màn hình"))
            {
                specs.Add("Kích thước", "27 inch");
                specs.Add("Tấm nền", "IPS / OLED");
                specs.Add("Độ phân giải", "2K QHD (2560x1440)");
                specs.Add("Tần số quét", "165Hz");
            }
            else
            {
                specs.Add("Thương hiệu", "Chính hãng");
                specs.Add("Xuất xứ", "Nhập khẩu");
                specs.Add("Bảo hành", "12 tháng");
                specs.Add("Tình trạng", "Mới 100%");
            }

            return specs;
        }
    }
}
