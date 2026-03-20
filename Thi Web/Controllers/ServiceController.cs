using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechShop.Data;
using TechShop.Models;

namespace TechShop.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ServiceController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Booking()
        {
            var model = new ServiceTicket();

            // Nếu khách đã đăng nhập, tự động điền tên và số điện thoại
            if (User.Identity!.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    model.UserId = user.Id;
                    // Lấy user.PhoneNumber hoặc thuộc tính FullName nếu có
                    model.CustomerName = user.UserName?.Split('@')[0] ?? "";
                    model.PhoneNumber = user.PhoneNumber ?? "";
                }
            }

            // Ngày đặt lịch mặc định là ngày mai
            model.BookingDate = DateTime.Now.AddDays(1);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Booking(ServiceTicket model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem khách có đăng nhập không để gán UserId
                if (User.Identity!.IsAuthenticated && string.IsNullOrEmpty(model.UserId))
                {
                    var user = await _userManager.GetUserAsync(User);
                    model.UserId = user?.Id;
                }

                model.Status = "Pending"; // Trạng thái chờ xử lý

                _context.ServiceTickets.Add(model);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Đặt lịch thành công! Nhân viên sẽ liên hệ với bạn sớm nhất.";
                return RedirectToAction("BookingSuccess");
            }

            return View(model);
        }

        public IActionResult BookingSuccess()
        {
            return View();
        }
    }
}