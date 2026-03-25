using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechShop.Data;
using TechShop.Models;
using TechShop.ViewModels.Admin;

namespace TechShop.Controllers
{
    // ================================================================
    // ADMIN PRODUCT CONTROLLER
    // ================================================================
    [Authorize(Roles = "Admin, Staff")]
    [Route("Admin/Product/{action=Index}")]
    public class AdminProductController : Controller
    {
        private readonly ApplicationDbContext _context;
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
    [Route("Admin/Category/{action=Index}")]
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
    [Route("Admin/Order/{action=Index}")]
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
    [Route("Admin/User/{action=Index}")]
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
    [Route("Admin/Role/{action=Index}")]
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
    // ================================================================
    // ADMIN DASHBOARD CONTROLLER
    // ================================================================
    [Authorize(Roles = "Admin,Staff")]
    [Route("Admin/Dashboard/{action=Index}")]
    public class AdminDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var completedOrders = _context.Orders.Where(o => o.Status == "Completed");

            var totalRevenue = await completedOrders.SumAsync(o => (decimal?)o.TotalAmount) ?? 0m;
            var totalOrders = await _context.Orders.CountAsync();
            var totalProducts = await _context.Products.CountAsync();

            var topProducts = await _context.OrderDetails
                .Include(od => od.Order)
                .Include(od => od.Product)
                .Where(od => od.Order != null && od.Order.Status == "Completed" && od.Product != null)
                .GroupBy(od => od.Product!.Name)
                .Select(g => new TopProductItemViewModel
                {
                    ProductName = g.Key,
                    SoldQuantity = g.Sum(x => x.Quantity),
                    Revenue = g.Sum(x => x.Quantity * x.UnitPrice)
                })
                .OrderByDescending(x => x.SoldQuantity)
                .Take(5)
                .ToListAsync();

            var salesRatio = topProducts.Select(x => new SalesRatioItemViewModel
            {
                ProductName = x.ProductName,
                SoldQuantity = x.SoldQuantity
            }).ToList();

            var vm = new AdminDashboardViewModel
            {
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                TotalProducts = totalProducts,
                TopProducts = topProducts,
                SalesRatio = salesRatio
            };

            return View("~/Views/Admin/Dashboard/Index.cshtml", vm);
        }
    }

    // ================================================================
    // ADMIN VARIANT CONTROLLER
    // ================================================================
    [Authorize(Roles = "Admin")]
    [Route("Admin/Variant/{action=Index}")]
    public class AdminVariantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminVariantController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var groups = await _context.ProductVariantGroups
                .Include(g => g.Category)
                .Include(g => g.Options)
                .OrderBy(g => g.Category!.Name)
                .ThenBy(g => g.SortOrder)
                .ToListAsync();

            return View("~/Views/Admin/Variant/Index.cshtml", groups);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            return View("~/Views/Admin/Variant/Create.cshtml", new AdminVariantCatalogViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminVariantCatalogViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
                return View("~/Views/Admin/Variant/Create.cshtml", model);
            }

            await UpsertGroupAsync(model.CategoryId, "Màu Sắc", model.Colors, 1);
            await UpsertGroupAsync(model.CategoryId, "Dung Lượng", model.Capacities, 2);
            await UpsertGroupAsync(model.CategoryId, "Nâng Cấp", model.Upgrades, 3);

            TempData["Success"] = "Đã lưu bộ biến thể theo danh mục.";
            return RedirectToAction(nameof(Index));
        }

        private async Task UpsertGroupAsync(int categoryId, string groupName, string? rawValues, int sortOrder)
        {
            var group = await _context.ProductVariantGroups
                .Include(g => g.Options)
                .FirstOrDefaultAsync(g => g.CategoryId == categoryId && g.Name == groupName);

            if (group == null)
            {
                group = new ProductVariantGroup
                {
                    CategoryId = categoryId,
                    Name = groupName,
                    SortOrder = sortOrder,
                    IsActive = true
                };
                _context.ProductVariantGroups.Add(group);
                await _context.SaveChangesAsync();
            }

            var values = (rawValues ?? "")
                .Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            // xóa option cũ của group rồi add lại theo dữ liệu admin nhập
            var oldOptions = await _context.ProductVariantOptions
                .Where(o => o.ProductVariantGroupId == group.Id)
                .ToListAsync();

            if (oldOptions.Any())
                _context.ProductVariantOptions.RemoveRange(oldOptions);

            await _context.SaveChangesAsync();

            var newOptions = values.Select((v, index) => new ProductVariantOption
            {
                ProductVariantGroupId = group.Id,
                Value = v,
                SortOrder = index + 1,
                ColorHex = groupName == "Màu Sắc" ? GuessColorHex(v) : null,
                IsActive = true
            }).ToList();

            if (newOptions.Any())
            {
                _context.ProductVariantOptions.AddRange(newOptions);
                await _context.SaveChangesAsync();
            }
        }

        private string? GuessColorHex(string value)
        {
            var key = value.Trim().ToLowerInvariant();
            return key switch
            {
                "đen" => "#111111",
                "trắng" => "#f8fafc",
                "đỏ" => "#ef4444",
                "xanh" => "#3b82f6",
                "xanh lá" => "#22c55e",
                "vàng" => "#eab308",
                "bạc" => "#cbd5e1",
                "hồng" => "#ec4899",
                _ => null
            };
        }
    }
    //[Authorize(Roles = "Admin")]
    //[Route("Admin/Variant/{action=Index}")]
    //public class AdminVariantController : Controller
    //{
    //    private readonly ApplicationDbContext _context;

    //    public AdminVariantController(ApplicationDbContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<IActionResult> Index()
    //    {
    //        var variants = await _context.ProductVariants
    //            .Include(v => v.Product)
    //            .Include(v => v.Values)
    //                .ThenInclude(vv => vv.ProductVariantOption)
    //                    .ThenInclude(o => o!.ProductVariantGroup)
    //            .OrderByDescending(v => v.Id)
    //            .ToListAsync();

    //        return View("~/Views/Admin/Variant/Index.cshtml", variants);
    //    }

    //    [HttpGet]
    //    public async Task<IActionResult> Create()
    //    {
    //        await EnsureVariantCatalogSeededAsync();
    //        ViewBag.Products = await _context.Products.Where(p => p.IsActive).OrderBy(p => p.Name).ToListAsync();
    //        ViewBag.Options = await _context.ProductVariantOptions
    //            .Include(o => o.ProductVariantGroup)
    //            .OrderBy(o => o.ProductVariantGroup!.Name)
    //            .ThenBy(o => o.Value)
    //            .ToListAsync();

    //        ViewBag.OptionCategoryMap = await BuildOptionCategoryMapAsync();

    //        return View("~/Views/Admin/Variant/Create.cshtml", new ProductVariant());
    //    }

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create(ProductVariant model, List<int> selectedOptionIds)
    //    {
    //        ModelState.Remove("Product");
    //        ModelState.Remove("Values");

    //        selectedOptionIds = selectedOptionIds?.Distinct().ToList() ?? new List<int>();
    //        if (!selectedOptionIds.Any())
    //            ModelState.AddModelError("", "Vui lòng chọn ít nhất 1 thuộc tính biến thể.");

    //        if (!ModelState.IsValid)
    //        {
    //            await EnsureVariantCatalogSeededAsync();
    //            ViewBag.Products = await _context.Products.Where(p => p.IsActive).OrderBy(p => p.Name).ToListAsync();
    //            ViewBag.Options = await _context.ProductVariantOptions.Include(o => o.ProductVariantGroup).ToListAsync();
    //            ViewBag.OptionCategoryMap = await BuildOptionCategoryMapAsync();
    //            return View("~/Views/Admin/Variant/Create.cshtml", model);
    //        }

    //        var duplicateGroupIds = await _context.ProductVariantOptions
    //            .Where(o => selectedOptionIds.Contains(o.Id))
    //            .GroupBy(o => o.ProductVariantGroupId)
    //            .Where(g => g.Count() > 1)
    //            .Select(g => g.Key)
    //            .ToListAsync();
    //        if (duplicateGroupIds.Any())
    //        {
    //            ModelState.AddModelError("", "Mỗi nhóm thuộc tính chỉ được chọn 1 giá trị.");
    //            await EnsureVariantCatalogSeededAsync();
    //            ViewBag.Products = await _context.Products.Where(p => p.IsActive).OrderBy(p => p.Name).ToListAsync();
    //            ViewBag.Options = await _context.ProductVariantOptions.Include(o => o.ProductVariantGroup).ToListAsync();
    //            ViewBag.OptionCategoryMap = await BuildOptionCategoryMapAsync();
    //            return View("~/Views/Admin/Variant/Create.cshtml", model);
    //        }

    //        model.Values = selectedOptionIds.Select(x => new ProductVariantValue
    //        {
    //            ProductVariantOptionId = x
    //        }).ToList();

    //        _context.ProductVariants.Add(model);
    //        await _context.SaveChangesAsync();

    //        TempData["Success"] = "Đã thêm biến thể sản phẩm.";
    //        return RedirectToAction(nameof(Index));
    //    }

    //    private async Task EnsureVariantCatalogSeededAsync()
    //    {
    //        var blueprints = new Dictionary<string, string[]>
    //        {
    //            ["Màu sắc"] = new[] { "Đen", "Trắng", "Bạc", "Xanh", "Đỏ" },
    //            ["RAM"] = new[] { "8GB", "16GB", "32GB", "64GB" },
    //            ["SSD"] = new[] { "512GB", "1TB", "2TB", "4TB" },
    //            ["Phiên bản"] = new[] { "Standard", "Plus", "Pro", "Pro Max" },
    //            ["Hiệu năng"] = new[] { "Cơ bản", "Nâng cao", "Cao cấp" },
    //            ["Kết nối"] = new[] { "Wired", "Wireless", "Bluetooth" }
    //        };

    //        foreach (var entry in blueprints)
    //        {
    //            var group = await _context.ProductVariantGroups
    //                .Include(g => g.Options)
    //                .FirstOrDefaultAsync(g => g.Name == entry.Key);

    //            if (group == null)
    //            {
    //                group = new ProductVariantGroup { Name = entry.Key };
    //                _context.ProductVariantGroups.Add(group);
    //                await _context.SaveChangesAsync();
    //            }

    //            var existingValues = group.Options.Select(x => x.Value).ToHashSet(StringComparer.OrdinalIgnoreCase);
    //            var missingValues = entry.Value
    //                .Where(v => !existingValues.Contains(v))
    //                .Select(v => new ProductVariantOption
    //                {
    //                    ProductVariantGroupId = group.Id,
    //                    Value = v
    //                })
    //                .ToList();

    //            if (missingValues.Any())
    //            {
    //                _context.ProductVariantOptions.AddRange(missingValues);
    //                await _context.SaveChangesAsync();
    //            }
    //        }
    //    }

    //    private async Task<Dictionary<int, List<int>>> BuildOptionCategoryMapAsync()
    //    {
    //        var products = await _context.Products.Include(p => p.Category).AsNoTracking().ToListAsync();
    //        var categories = products
    //            .Select(p => p.Category)
    //            .Where(c => c != null)
    //            .DistinctBy(c => c!.Id)
    //            .Select(c => c!)
    //            .ToList();

    //        var allCategoryIds = categories.Select(c => c.Id).ToList();
    //        var laptopIds = categories.Where(c => c.Name.Contains("Laptop", StringComparison.OrdinalIgnoreCase)).Select(c => c.Id).ToList();
    //        var pcPartIds = categories.Where(c => c.Name.Contains("Linh kiện", StringComparison.OrdinalIgnoreCase) || c.Name.Contains("Ổ cứng", StringComparison.OrdinalIgnoreCase)).Select(c => c.Id).ToList();
    //        var peripheralIds = categories.Where(c =>
    //                c.Name.Contains("Màn hình", StringComparison.OrdinalIgnoreCase) ||
    //                c.Name.Contains("Chuột", StringComparison.OrdinalIgnoreCase) ||
    //                c.Name.Contains("Bàn phím", StringComparison.OrdinalIgnoreCase) ||
    //                c.Name.Contains("Ghế", StringComparison.OrdinalIgnoreCase) ||
    //                c.Name.Contains("Tai nghe", StringComparison.OrdinalIgnoreCase) ||
    //                c.Name.Contains("Điện thoại", StringComparison.OrdinalIgnoreCase))
    //            .Select(c => c.Id)
    //            .ToList();

    //        var groupMap = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase)
    //        {
    //            ["Màu sắc"] = allCategoryIds,
    //            ["RAM"] = laptopIds,
    //            ["SSD"] = laptopIds.Concat(pcPartIds).Distinct().ToList(),
    //            ["Phiên bản"] = peripheralIds,
    //            ["Hiệu năng"] = pcPartIds,
    //            ["Kết nối"] = peripheralIds
    //        };

    //        var options = await _context.ProductVariantOptions.Include(o => o.ProductVariantGroup).ToListAsync();
    //        var result = new Dictionary<int, List<int>>();
    //        foreach (var option in options)
    //        {
    //            var groupName = option.ProductVariantGroup?.Name ?? "";
    //            result[option.Id] = groupMap.TryGetValue(groupName, out var ids) && ids.Any()
    //                ? ids
    //                : allCategoryIds;
    //        }

    //        return result;
    //    }
    //}
}