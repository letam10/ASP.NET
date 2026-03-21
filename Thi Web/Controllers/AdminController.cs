using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechShop.Models;
using TechShop.Data;

namespace TechShop.Controllers
{
    // ================================================================
    // ADMIN PRODUCT CONTROLLER
    // ================================================================
    [Authorize(Roles = "Admin")]
    public class AdminProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminProductController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            Order? order = await _context.Orders.FindAsync(id);
            if (order != null) { order.Status = status; await _context.SaveChangesAsync(); }
            TempData["Success"] = "Cập nhật trạng thái thành công!";
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

    }
}
