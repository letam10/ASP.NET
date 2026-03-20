using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TechShop.Data;
using TechShop.Models;

namespace TechShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartService _cartService;

        public ProductController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ICartService cartService)
        {
            _context = context;
            _userManager = userManager;
            _cartService = cartService;
        }

        // ===== INDEX =====
        public async Task<IActionResult> Index(
            int? category,
            string? search,
            string? priceRange,
            string? sortOrder,
            int page = 1)
        {
            int pageSize = 12;
            if (page < 1) page = 1;

            var query = _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .AsQueryable();

            // Lọc theo CategoryId
            if (category.HasValue)
                query = query.Where(p => p.CategoryId == category.Value);

            // Lọc theo search
            if (!string.IsNullOrEmpty(search))
                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    (p.Description != null && p.Description.Contains(search)));

            // Lọc theo giá
            if (!string.IsNullOrEmpty(priceRange))
            {
                query = priceRange switch
                {
                    "under5" => query.Where(p => (p.DiscountPrice ?? p.Price) < 5_000_000),
                    "5to10" => query.Where(p => (p.DiscountPrice ?? p.Price) >= 5_000_000 && (p.DiscountPrice ?? p.Price) <= 10_000_000),
                    "10to15" => query.Where(p => (p.DiscountPrice ?? p.Price) > 10_000_000 && (p.DiscountPrice ?? p.Price) <= 15_000_000),
                    "over15" => query.Where(p => (p.DiscountPrice ?? p.Price) > 15_000_000),
                    _ => query
                };
            }

            // Sắp xếp
            query = sortOrder switch
            {
                "price_asc" => query.OrderBy(p => p.DiscountPrice ?? p.Price),
                "price_desc" => query.OrderByDescending(p => p.DiscountPrice ?? p.Price),
                "name_asc" => query.OrderBy(p => p.Name),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            // Phân trang
            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            if (totalPages < 1) totalPages = 1;
            if (page > totalPages) page = totalPages;

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Lấy categories từ DB cho sidebar
            var dbCategories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            ViewBag.Categories = dbCategories;
            ViewBag.Category = category;
            ViewBag.Search = search ?? "";
            ViewBag.SortOrder = sortOrder ?? "";
            ViewBag.PriceRange = priceRange ?? "";
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;

            return View(products);
        }

        // ===== DETAIL =====
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Specifications)
                .Include(p => p.WarrantyPackages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            ViewBag.Related = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == product.CategoryId && p.Id != id && p.IsActive)
                .Take(4)
                .ToListAsync();

            return View(product);
        }

        // ===== ADD TO CART =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(
            int productId,
            int quantity = 1,
            int warrantyId = 0,
            bool isTradeIn = false)
        {
            var product = await _context.Products
                .Include(p => p.WarrantyPackages)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null) return NotFound();

            decimal finalPrice = product.DiscountPrice ?? product.Price;
            string extraOptions = "";

            // Gói bảo hành
            if (warrantyId > 0)
            {
                var warranty = product.WarrantyPackages.FirstOrDefault(w => w.Id == warrantyId);
                if (warranty != null)
                {
                    finalPrice += warranty.AdditionalPrice;
                    extraOptions += $" [+ {warranty.PackageName}]";
                }
            }

            // Thu cũ đổi mới
            if (isTradeIn && product.IsTradeInEligible)
            {
                decimal tradeInDiscount = (product.MaxTradeInValue ?? 0) * 0.3m;
                finalPrice -= tradeInDiscount;
                extraOptions += " [Thu cũ đổi mới]";
            }

            _cartService.AddToCart(HttpContext.Session, new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name + extraOptions,
                Price = finalPrice > 0 ? finalPrice : 0,
                Quantity = quantity,
                ImageUrl = product.ImageUrl
            });

            TempData["Success"] = $"Đã thêm \"{product.Name}\" vào giỏ hàng!";
            return RedirectToAction("Index", "Cart");
        }

        // ===== SO SÁNH =====
        [HttpPost]
        public IActionResult AddToCompare(int productId)
        {
            var compareListStr = HttpContext.Session.GetString("CompareList");
            List<int> compareIds = string.IsNullOrEmpty(compareListStr)
                ? new List<int>()
                : JsonSerializer.Deserialize<List<int>>(compareListStr)!;

            if (!compareIds.Contains(productId))
            {
                if (compareIds.Count >= 3)
                    return Json(new { success = false, message = "Chỉ được so sánh tối đa 3 sản phẩm cùng lúc!" });

                compareIds.Add(productId);
                HttpContext.Session.SetString("CompareList", JsonSerializer.Serialize(compareIds));
            }

            return Json(new { success = true, count = compareIds.Count, message = "Đã thêm vào danh sách so sánh." });
        }

        [HttpPost]
        public IActionResult RemoveFromCompare(int productId)
        {
            var compareListStr = HttpContext.Session.GetString("CompareList");
            if (!string.IsNullOrEmpty(compareListStr))
            {
                var compareIds = JsonSerializer.Deserialize<List<int>>(compareListStr)!;
                compareIds.Remove(productId);
                HttpContext.Session.SetString("CompareList", JsonSerializer.Serialize(compareIds));
            }
            return RedirectToAction(nameof(Compare));
        }

        public async Task<IActionResult> Compare()
        {
            var compareListStr = HttpContext.Session.GetString("CompareList");
            List<int> compareIds = string.IsNullOrEmpty(compareListStr)
                ? new List<int>()
                : JsonSerializer.Deserialize<List<int>>(compareListStr)!;

            var products = await _context.Products
                .Include(p => p.Specifications)
                .Where(p => compareIds.Contains(p.Id))
                .ToListAsync();

            return View(products);
        }

        // ===== WISHLIST =====
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ToggleWishlist(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Json(new { success = false, message = "Vui lòng đăng nhập!" });

            var existing = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == user.Id && w.ProductId == productId);

            if (existing != null)
            {
                _context.Wishlists.Remove(existing);
                await _context.SaveChangesAsync();
                return Json(new { success = true, isAdded = false, message = "Đã bỏ yêu thích." });
            }

            _context.Wishlists.Add(new Wishlist { UserId = user.Id, ProductId = productId });
            await _context.SaveChangesAsync();
            return Json(new { success = true, isAdded = true, message = "Đã thêm vào mục yêu thích." });
        }

        [Authorize]
        public async Task<IActionResult> Wishlist()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var wishlists = await _context.Wishlists
                .Include(w => w.Product)
                .ThenInclude(p => p!.Category)
                .Where(w => w.UserId == user.Id)
                .Select(w => w.Product)
                .ToListAsync();

            return View(wishlists);
        }
    }
}