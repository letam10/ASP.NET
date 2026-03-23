using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechShop.Data;
using TechShop.Models;

namespace TechShop.Controllers
{
    // ================================================================
    // ADMIN PRODUCT CONTROLLER
    // ================================================================
    [Authorize(Roles = "Admin, Staff")]
    public class AdminProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminProductController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index(int? categoryId)
        {
            var q = _context.Products.Include(p => p.Category).AsQueryable();
            if (categoryId.HasValue)
                q = q.Where(p => p.CategoryId == categoryId.Value);

            var products = await q.ToListAsync();

            ViewBag.Categories = await _context.Categories
                .Include(c => c.Products)
                .OrderBy(c => c.Name)
                .ToListAsync();

            ViewBag.SelectedCategory = categoryId;
            return View("~/Views/Admin/Product/Index.cshtml", products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View("~/Views/Admin/Product/Create.cshtml", new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model)
        {
            ModelState.Remove("Category");
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                _context.Products.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View("~/Views/Admin/Product/Create.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Product? product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View("~/Views/Admin/Product/Edit.cshtml", product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product model)
        {
            if (id != model.Id) return NotFound();
            ModelState.Remove("Category");
            if (ModelState.IsValid)
            {
                _context.Products.Update(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View("~/Views/Admin/Product/Edit.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Product? product = await _context.Products.FindAsync(id);
            if (product != null) { _context.Products.Remove(product); await _context.SaveChangesAsync(); }
            TempData["Success"] = "Đã xóa sản phẩm.";
            return RedirectToAction(nameof(Index));
        }
    }

    // ================================================================
    // ADMIN CATEGORY CONTROLLER
    // ================================================================
    [Authorize(Roles = "Admin,Staff")]
    public class AdminCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminCategoryController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.Include(c => c.Products).ToListAsync();
            return View("~/Views/Admin/Category/Index.cshtml", categories);
        }

        [HttpGet]
        public IActionResult Create()
            => View("~/Views/Admin/Category/Create.cshtml", new Category());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            ModelState.Remove("Products");
            if (ModelState.IsValid)
            {
                _context.Categories.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thêm danh mục thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Admin/Category/Create.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Category? category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View("~/Views/Admin/Category/Edit.cshtml", category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category model)
        {
            if (id != model.Id) return NotFound();
            ModelState.Remove("Products");
            if (ModelState.IsValid)
            {
                _context.Categories.Update(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật danh mục thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Admin/Category/Edit.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Category? category = await _context.Categories.FindAsync(id);
            if (category != null) { _context.Categories.Remove(category); await _context.SaveChangesAsync(); }
            TempData["Success"] = "Đã xóa danh mục.";
            return RedirectToAction(nameof(Index));
        }
    }

    // ================================================================
    // ADMIN ORDER CONTROLLER
    // ================================================================
    [Authorize(Roles = "Admin,Staff")]
    public class AdminOrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminOrderController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return View("~/Views/Admin/Order/Index.cshtml", orders);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Order? order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return View("~/Views/Admin/Order/Detail.cshtml", order);
        }

        private int CalculateEarnedPoints(decimal totalAmount)
        {
            // ví dụ: 1 điểm / 100.000
            return (int)Math.Floor(totalAmount / 100_000m);
        }
        private string GetTierByPoints(int points)
        {
            if (points >= 5000) return "Diamond";
            if (points >= 2000) return "Gold";
            if (points >= 500) return "Silver";
            return "Bronze";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            var oldStatus = order.Status;
            order.Status = status;
            if (order.User != null)
            {
                // Nếu chuyển sang Completed và chưa award -> cộng
                if (status == "Completed" && !order.LoyaltyPointsAwarded)
                {
                    int earned = CalculateEarnedPoints(order.TotalAmount);
                    int oldPoints = order.User.LoyaltyPoints;
                    string oldTier = order.User.MembershipTier;
                    order.User.LoyaltyPoints += earned;
                    order.User.MembershipTier = GetTierByPoints(order.User.LoyaltyPoints);
                    order.LoyaltyPointsAwarded = true;

                    // TODO: nếu bạn đã thêm SendMembershipUpgradeEmailAsync
                    // if (oldTier != order.User.MembershipTier && !string.IsNullOrEmpty(order.User.Email))
                    //     await _emailService.SendMembershipUpgradeEmailAsync(...);

                    TempData["Success"] = $"Đã cập nhật trạng thái + cộng {earned} điểm (từ {oldPoints} ➜ {order.User.LoyaltyPoints}).";
                }
                // Nếu chuyển sang Cancelled mà đã award -> trừ
                if (status == "Cancelled" && order.LoyaltyPointsAwarded)
                {
                    int earned = CalculateEarnedPoints(order.TotalAmount);
                    int oldPoints = order.User.LoyaltyPoints;

                    order.User.LoyaltyPoints = Math.Max(0, order.User.LoyaltyPoints - earned);
                    order.User.MembershipTier = GetTierByPoints(order.User.LoyaltyPoints);
                    order.LoyaltyPointsAwarded = false;

                    TempData["Success"] = $"Đã hủy đơn và trừ lại {earned} điểm (từ {oldPoints} ➜ {order.User.LoyaltyPoints}).";
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Detail), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPOS(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null && order.Status == "Pending")
            {
                order.Status = "Processing"; // Hoặc "Paid" tùy bạn đặt tên
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã xác nhận thanh toán POS thành công!";
            }
            return RedirectToAction("Detail", new { id = orderId });
        }
    }

    // ================================================================
    // ADMIN USER CONTROLLER
    // ================================================================
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminUserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userRoles = new Dictionary<string, IList<string>>();
            foreach (var user in users)
                userRoles[user.Id] = await _userManager.GetRolesAsync(user);
            ViewBag.UserRoles = userRoles;
            return View("~/Views/Admin/User/Index.cshtml", users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(id);
            if (user != null) await _userManager.DeleteAsync(user);
            TempData["Success"] = "Đã xóa người dùng.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AssignRole(string userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            ViewBag.User = user;
            ViewBag.AllRoles = _roleManager.Roles.ToList();
            ViewBag.UserRoles = await _userManager.GetRolesAsync(user);
            return View("~/Views/Admin/User/AssignRole.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(string userId, string roleName, bool assign)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                bool isInRole = await _userManager.IsInRoleAsync(user, roleName);
                if (assign && !isInRole)
                    await _userManager.AddToRoleAsync(user, roleName);
                else if (!assign && isInRole)
                    await _userManager.RemoveFromRoleAsync(user, roleName);
            }
            TempData["Success"] = "Cập nhật quyền thành công!";
            return RedirectToAction(nameof(AssignRole), new { userId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Thêm bảo mật chống CSRF
        public async Task<IActionResult> UpdateLoyalty(string userId, int points, string tier)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.LoyaltyPoints = points;
                user.MembershipTier = tier;
                await _userManager.UpdateAsync(user);
                TempData["Success"] = $"Đã cập nhật thành viên {user.UserName} thành thẻ {tier} với {points} điểm.";
            }
            return RedirectToAction(nameof(Index));
        }
    }

    // ================================================================
    // ADMIN ROLE CONTROLLER
    // ================================================================
    [Authorize(Roles = "Admin")]
    public class AdminRoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminRoleController(RoleManager<IdentityRole> roleManager) => _roleManager = roleManager;

        public IActionResult Index()
            => View("~/Views/Admin/Roles/Index.cshtml", _roleManager.Roles.ToList());

        [HttpGet]
        public IActionResult Create()
            => View("~/Views/Admin/Roles/Create.cshtml");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string roleName)
        {
            if (!string.IsNullOrWhiteSpace(roleName) && !await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
                TempData["Success"] = "Tạo role thành công!";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole? role = await _roleManager.FindByIdAsync(id);
            if (role != null) await _roleManager.DeleteAsync(role);
            TempData["Success"] = "Đã xóa role.";
            return RedirectToAction(nameof(Index));
        }
        // ================================================================
        // ADMIN SUPPORT CONTROLLER
        // ================================================================
        [Authorize(Roles = "Admin,Staff")]
        public class AdminSupportController : Controller
        {
            public IActionResult Index()
            {
                ViewBag.TawkDirectChatUrl = "https://tawk.to/chat/69bac14ebb7f0b1c337b2b54/1jk0o67bd";
                ViewBag.TawkDashboardUrl = "https://dashboard.tawk.to/";
                return View("~/Views/Admin/Support/Index.cshtml");
            }
        }

        [Authorize(Roles = "Admin")]
        public class AdminCouponController : Controller
        {
            private readonly ApplicationDbContext _context;
            public AdminCouponController(ApplicationDbContext context) => _context = context;
            public async Task<IActionResult> Index()
                => View(await _context.Coupons.OrderByDescending(x => x.Id).ToListAsync());
            [HttpGet]
            public IActionResult Create() => View(new Coupon());
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(Coupon model)
            {
                if (!ModelState.IsValid) return View(model);
                model.Code = model.Code.Trim().ToUpperInvariant();
                _context.Coupons.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã tạo mã giảm giá.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
