namespace TechShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index(int? categoryId, string? search)
        {
            var categories = await _context.Categories.Include(c => c.Products).ToListAsync();

            if (!categoryId.HasValue && string.IsNullOrEmpty(search) && categories.Any())
            {
                categoryId = categories.First().Id;
            }

            IQueryable<Product> query = _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive);

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    (p.Description != null && p.Description.Contains(search)));

            var products = await query.ToListAsync();

            // Lấy sản phẩm yêu thích nếu đã đăng nhập
            List<Product> wishlistProducts = new List<Product>();
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    wishlistProducts = await _context.Wishlists
                        .Include(w => w.Product)
                        .ThenInclude(p => p!.Category)
                        .Where(w => w.UserId == user.Id)
                        .Select(w => w.Product!)
                        .ToListAsync();
                }
            }

            // Lấy sản phẩm đang giảm giá (Promo)
            var promoProducts = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.DiscountPrice.HasValue && p.DiscountPrice < p.Price)
                .OrderByDescending(p => p.CreatedAt)
                .Take(12)
                .ToListAsync();

            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = categoryId;
            ViewBag.Search = search;
            ViewBag.WishlistProducts = wishlistProducts;
            ViewBag.PromoProducts = promoProducts;

            return View(products);
        }

        public async Task<IActionResult> FilterProducts(int? categoryId, string? search)
        {
            IQueryable<Product> query = _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive);

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    (p.Description != null && p.Description.Contains(search)));

            var products = await query.ToListAsync();
            return PartialView("_ProductList", products);
        }

        public IActionResult Privacy() => View();
        public IActionResult ChinhSach() => View();
        public IActionResult GioiThieu() => View();

        [HttpPost]
        public async Task<IActionResult> SubscribeNewsletter(string email)
        {
            if (string.IsNullOrEmpty(email)) return Json(new { success = false, message = "Email không hợp lệ." });
            
            // Logically you would save to DB here
            
            try 
            {
                await _emailService.SendAsync(email, "Cảm ơn bạn đã đăng ký TechShop Newsletter!", 
                    "<h2>Chào mừng bạn!</h2><p>TechShop đã ghi nhận đăng ký của bạn. Bạn sẽ nhận được những thông tin công nghệ mới nhất từ chúng tôi.</p>");
                return Json(new { success = true, message = "Đăng ký thành công! Vui lòng kiểm tra email của bạn." });
            }
            catch (Exception ex)
            {
                // Trả về lỗi nếu không gửi được mail để người dùng biết
                return Json(new { success = false, message = "Không thể gửi email lúc này. " + ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View();
    }
}
