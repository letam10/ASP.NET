using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechShop.Data;
using TechShop.Models;

namespace TechShop.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public GameController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> LuckyWheel()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> SpinWheel()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false, message = "Lỗi xác thực." });

            int costPerSpin = 50; // Tốn 50 điểm cho 1 lần quay

            if (user.LoyaltyPoints < costPerSpin)
            {
                return Json(new { success = false, message = "Bạn không đủ điểm để quay! Mua thêm hàng để tích điểm nhé." });
            }

            // Trừ điểm ngay lập tức
            user.LoyaltyPoints -= costPerSpin;

            // Random phần thưởng
            var random = new Random();
            int chance = random.Next(1, 101); // Random từ 1-100%

            string prize = "";
            int segmentIndex = 0; // Vị trí trên CSS Wheel
            decimal commissionBonus = 0;

            // Thiết lập tỷ lệ trúng thưởng
            if (chance <= 5)
            {
                prize = "Voucher 500k"; segmentIndex = 1; commissionBonus = 500000;
            }
            else if (chance <= 20)
            {
                prize = "100 Điểm"; segmentIndex = 3; user.LoyaltyPoints += 100;
            }
            else if (chance <= 45)
            {
                prize = "Voucher 50k"; segmentIndex = 5; commissionBonus = 50000;
            }
            else
            {
                // 55% trượt
                prize = "Chúc bạn may mắn lần sau";
                segmentIndex = random.Next(0, 2) == 0 ? 0 : 4; // Ô trượt số 0 hoặc 4
            }

            // Nếu trúng tiền, cộng thẳng vào quỹ Hoa hồng để dùng cho đơn sau
            if (commissionBonus > 0)
            {
                user.CommissionBalance += commissionBonus;
                _context.CommissionLogs.Add(new CommissionLog
                {
                    UserId = user.Id,
                    Amount = commissionBonus,
                    Description = $"Trúng vòng quay may mắn: {prize}"
                });
            }

            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                prize = prize,
                segment = segmentIndex,
                remainingPoints = user.LoyaltyPoints
            });
        }
    }
}