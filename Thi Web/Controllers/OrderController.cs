using TechShop.Services;

namespace TechShop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public OrderController(ApplicationDbContext context, ICartService cartService, UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _context = context;
            _cartService = cartService;
            _userManager = userManager;
            _emailService = emailService;
        }
        private decimal GetMembershipDiscountRate(string tier)
        {
            return tier switch
            {
                "Silver" => 0.03m,
                "Gold" => 0.05m,
                "Diamond" => 0.08m,
                _ => 0m
            };
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = _cartService.GetCart(HttpContext.Session);
            if (!cart.Any()) return RedirectToAction("Index", "Cart");
            ViewBag.Cart = cart;
            ViewBag.Total = _cartService.GetTotal(HttpContext.Session);
            return View(new Order());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order model)
        {
            var cart = _cartService.GetCart(HttpContext.Session);
            if (!cart.Any()) return RedirectToAction("Index", "Cart");

            ModelState.Remove("User");
            ModelState.Remove("UserId");
            ModelState.Remove("OrderDetails");

            if (!ModelState.IsValid)
            {
                ViewBag.Cart = cart;
                ViewBag.Total = _cartService.GetTotal(HttpContext.Session);
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            // TÍNH PHÍ VẬN CHUYỂN
            decimal shippingFee = 35000; // Phí ship mặc định
            int totalItems = cart.Sum(c => c.Quantity);
            decimal cartTotal = _cartService.GetTotal(HttpContext.Session);

            // Giảm phí ship nếu mua nhiều hoặc đơn giá cao
            if (totalItems >= 3 || cartTotal > 5000000)
            {
                shippingFee = 15000;
            }

            // Miễn phí vận chuyển cho khách VIP
            if (user.MembershipTier == "Gold" || user.MembershipTier == "Diamond")
            {
                shippingFee = 0;
            }

            var order = new Order
            {
                UserId = user.Id,
                FullName = model.FullName,
                Address = model.Address,
                City = model.City,
                PostalCode = model.PostalCode,
                TotalAmount = cartTotal + shippingFee, // Đã cộng phí ship
                Status = "Pending",
                OrderDate = DateTime.Now,
                OrderDetails = cart.Select(c => new OrderDetail
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    UnitPrice = c.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // TÍCH ĐIỂM VÀ NÂNG HẠNG THẺ CHO NGƯỜI DÙNG
            int earnedPoints = (int)(order.TotalAmount / 100000);
            user.LoyaltyPoints += earnedPoints;

            if (user.LoyaltyPoints >= 5000)
                user.MembershipTier = "Diamond";
            else if (user.LoyaltyPoints >= 2000)
                user.MembershipTier = "Gold";
            else if (user.LoyaltyPoints >= 500)
                user.MembershipTier = "Silver";
            else
                user.MembershipTier = "Bronze";

            await _userManager.UpdateAsync(user);

            // HÀM GỬI EMAIL 
            if (!string.IsNullOrEmpty(user.Email))
            {
                _ = _emailService.SendOrderConfirmationAsync(user.Email, user.UserName ?? "Khách hàng", order);
            }
            _cartService.ClearCart(HttpContext.Session);
            return RedirectToAction(nameof(Completed), new { id = order.Id });
        }

        public async Task<IActionResult> Completed(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }

        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");
            var orders = await _context.Orders
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return View(orders);
        }
    }
}
