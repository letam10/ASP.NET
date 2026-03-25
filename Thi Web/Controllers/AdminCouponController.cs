using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechShop.Data;
using TechShop.Models;

namespace TechShop.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin/Coupon/{action=Index}")]
    public class AdminCouponController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminCouponController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var coupons = await _context.Coupons.OrderByDescending(x => x.Id).ToListAsync();
            return View("~/Views/Admin/Coupon/Index.cshtml", coupons);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("~/Views/Admin/Coupon/Create.cshtml", new Coupon());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Coupon model)
        {
            if (!ModelState.IsValid) return View("~/Views/Admin/Coupon/Create.cshtml", model);

            model.Code = model.Code.Trim().ToUpperInvariant();
            _context.Coupons.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã tạo mã giảm giá.";
            return RedirectToAction(nameof(Index));
        }
    }
}
