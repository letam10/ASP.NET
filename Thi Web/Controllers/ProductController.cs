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

        public ProductController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(
            string? category,
            string? search,
            string? priceRange,
            string? sortOrder,
            int page = 1)
        {
            // Đảm bảo page hợp lệ
            if (page < 1) page = 1;
            string activeCategory = string.IsNullOrEmpty(category)
                ? "smartphones" : category;
            DummyResult result;
            if (!string.IsNullOrEmpty(search))
                result = await _dummyJson.SearchAsync(search, page, 12);
            else
                result = await _dummyJson.GetByCategoryAsync(activeCategory, page, 12);
            var products = result.Products;
            // Lọc giá
            if (!string.IsNullOrEmpty(priceRange))
            {
                products = priceRange switch
                {
                    "under5" => products.Where(p => p.DiscountedPriceVnd < 5_000_000).ToList(),
                    "5to10" => products.Where(p => p.DiscountedPriceVnd >= 5_000_000 && p.DiscountedPriceVnd <= 10_000_000).ToList(),
                    "10to15" => products.Where(p => p.DiscountedPriceVnd > 10_000_000 && p.DiscountedPriceVnd <= 15_000_000).ToList(),
                    "over15" => products.Where(p => p.DiscountedPriceVnd > 15_000_000).ToList(),
                    _ => products
                };
            }
            // Sắp xếp
            products = sortOrder switch
            {
                "price_asc" => products.OrderBy(p => p.DiscountedPriceVnd).ToList(),
                "price_desc" => products.OrderByDescending(p => p.DiscountedPriceVnd).ToList(),
                "name_asc" => products.OrderBy(p => p.Title).ToList(),
                "rating" => products.OrderByDescending(p => p.Rating).ToList(),
                _ => products
            };
            // Truyền page/totalPages đúng từ API response
            int totalPages = result.Total > 0 && result.Limit > 0
      ? (int)Math.Ceiling((double)result.Total / 12.0)
      : 1;

            // Giới hạn totalPages hợp lý
            if (totalPages < 1) totalPages = 1;
            if (page > totalPages) page = totalPages;
            ViewBag.Categories = DummyJsonService.TechCategories;
            ViewBag.Category = activeCategory;
            ViewBag.Search = search ?? "";
            ViewBag.SortOrder = sortOrder ?? "";
            ViewBag.PriceRange = priceRange ?? "";
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            return View(products);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _dummyJson.GetDetailAsync(id);
            if (product == null) return NotFound();
            var related = await _dummyJson.GetByCategoryAsync(product.Category, 1, 5);
            ViewBag.Related = related.Products.Where(p => p.Id != id).Take(4).ToList();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(
            int productId, string title,
            decimal priceVnd, string? thumbnail,
            int quantity = 1)
        {
            _cartService.AddToCart(HttpContext.Session, new CartItem
            {
                ProductId = productId,
                ProductName = title,
                Price = priceVnd,
                Quantity = quantity,
                ImageUrl = thumbnail
            });

            TempData["Success"] = $"Đã thêm \"{title}\" vào giỏ hàng!";
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public IActionResult AddToCompare(int productId)
        {
            var compareListStr = HttpContext.Session.GetString("CompareList");
            List<int> compareIds = string.IsNullOrEmpty(compareListStr)
                ? new List<int>()
                : JsonSerializer.Deserialize<List<int>>(compareListStr);
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
                var compareIds = JsonSerializer.Deserialize<List<int>>(compareListStr);
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
                : JsonSerializer.Deserialize<List<int>>(compareListStr);

            var products = await _context.Products
                .Include(p => p.Specifications)
                .Where(p => compareIds.Contains(p.Id))
                .ToListAsync();
            return View(products);
        }

        // DANH SÁCH YÊU THÍCH
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ToggleWishlist(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false, message = "Vui lòng đăng nhập!" });
            var existing = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == user.Id && w.ProductId == productId);
            if (existing != null)
            {
                _context.Wishlists.Remove(existing);
                await _context.SaveChangesAsync();
                return Json(new { success = true, isAdded = false, message = "Đã bỏ yêu thích." });
            }
            else
            {
                _context.Wishlists.Add(new Wishlist { UserId = user.Id, ProductId = productId });
                await _context.SaveChangesAsync();
                return Json(new { success = true, isAdded = true, message = "Đã thêm vào mục yêu thích." });
            }
        }

        [Authorize]
        public async Task<IActionResult> Wishlist()
        {
            var user = await _userManager.GetUserAsync(User);
            var wishlists = await _context.Wishlists
                .Include(w => w.Product)
                .Where(w => w.UserId == user.Id)
                .Select(w => w.Product)
                .ToListAsync();
            return View(wishlists);
        }
    }
}