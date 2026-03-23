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
        [Authorize]
        public async Task<IActionResult> SpinWheel()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            if (user.LoyaltyPoints < 50)
                return Json(new { success = false, message = "Bạn không đủ điểm để quay." });

            var segments = new[]
            {
            new { Label = "Trượt rồi", Reward = 0m, Weight = 45.0 },
            new { Label = "Chúc may mắn lần sau", Reward = 0m, Weight = 45.0 },
            new { Label = "Voucher 50K", Reward = 50000m, Weight = 2.5 },
            new { Label = "Voucher 100K", Reward = 100000m, Weight = 2.5 },
            new { Label = "Voucher 500K", Reward = 500000m, Weight = 2.5 },
            new { Label = "Voucher Freeship", Reward = 0m, Weight = 2.5 }
            };

            user.LoyaltyPoints -= 50;

            int winningIndex = GetWeightedIndex(segments.Select(x => x.Weight).ToArray());
            var prize = segments[winningIndex];

            if (prize.Reward > 0)
            {
                user.CommissionBalance += prize.Reward;
                _context.CommissionLogs.Add(new CommissionLog
                {
                    UserId = user.Id,
                    Amount = prize.Reward,
                    Description = "Trúng thưởng vòng quay",
                    CreatedAt = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                winningIndex,
                winningLabel = prize.Label,
                reward = prize.Reward,
                currentPoints = user.LoyaltyPoints
            });
        }

        private int GetWeightedIndex(double[] weights)
        {
            var total = weights.Sum();
            var random = Random.Shared.NextDouble() * total;
            double cumulative = 0;

            for (int i = 0; i < weights.Length; i++)
            {
                cumulative += weights[i];
                if (random <= cumulative)
                    return i;
            }

            return weights.Length - 1;
        }

    }
}
