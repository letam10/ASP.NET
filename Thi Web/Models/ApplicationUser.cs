using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string Address { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        // 1. Hệ thống giới thiệu (Referral)
        public string? ReferralCode { get; set; } // Mã giới thiệu riêng của user này
        public string? ReferredByCode { get; set; } // Người đã giới thiệu user này (nhập lúc đăng ký)

        // 2. Hoa hồng tích lũy (Commission)
        [Column(TypeName = "decimal(18,2)")]
        public decimal CommissionBalance { get; set; } = 0; // Tiền hoa hồng hiện có

        // 3. Hệ thống Thẻ thành viên (Loyalty)
        public int LoyaltyPoints { get; set; } = 0; // Điểm tích lũy khi mua hàng
        public string MembershipTier { get; set; } = "Bronze"; // Bronze, Silver, Gold, Diamond
    }
}
