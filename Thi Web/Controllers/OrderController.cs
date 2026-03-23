using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechShop.Data;
using TechShop.Models;
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
        private readonly ILogger<OrderController> _logger;

        public OrderController(
            ApplicationDbContext context,
            ICartService cartService,
            UserManager<ApplicationUser> userManager,
            IEmailService emailService,
            ILogger<OrderController> logger)
        {
            _context = context;
            _cartService = cartService;
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger;
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

        private decimal CalculateShippingFee(int totalItems, decimal cartTotal, string tier)
        {
            // Rule của bạn: giảm ship nếu mua nhiều / giá trị cao,
            // miễn ship cho thẻ cao nhất (mình hiểu là Diamond).
            if (tier == "Diamond") return 0;

            decimal shipping = 35000;

            if (totalItems >= 3 || cartTotal >= 5_000_000m)
                shipping = 15000;

            return shipping;
        }

        private void SetCheckoutViewBags(List<CartItem> cart, ApplicationUser user)
        {
            decimal cartTotal = _cartService.GetTotal(HttpContext.Session);
            int totalItems = cart.Sum(x => x.Quantity);

            decimal rate = GetMembershipDiscountRate(user.MembershipTier);
            decimal memberDiscount = Math.Round(cartTotal * rate, 0);

            decimal shippingFee = CalculateShippingFee(totalItems, cartTotal, user.MembershipTier);
            decimal finalTotal = cartTotal - memberDiscount + shippingFee;

            ViewBag.Cart = cart;
            ViewBag.CartTotal = cartTotal;
            ViewBag.MemberDiscount = memberDiscount;
            ViewBag.ShippingFee = shippingFee;
            ViewBag.FinalTotal = finalTotal;

            // Nếu bạn vẫn dùng ViewBag.Total ở view cũ:
            ViewBag.Total = finalTotal;

            ViewBag.ItemCount = totalItems;
            ViewBag.MembershipTier = user.MembershipTier;
            ViewBag.DiscountRate = rate; // nếu muốn show % giảm
        }

        [HttpGet]
        private async Task<(decimal discount, string message)> ApplyCouponAsync(string? couponCode, decimal cartTotal)
        {
            if (string.IsNullOrWhiteSpace(couponCode))
                return (0m, "");
            couponCode = couponCode.Trim().ToUpperInvariant();
            var coupon = await _context.Coupons.FirstOrDefaultAsync(x =>
                x.Code == couponCode &&
                x.IsActive &&
                x.Quantity > 0 &&
                (!x.ExpiredAt.HasValue || x.ExpiredAt > DateTime.Now));
            if (coupon == null)
                return (0m, "Mã giảm giá không hợp lệ.");
            if (coupon.MinOrderAmount.HasValue && cartTotal < coupon.MinOrderAmount.Value)
                return (0m, $"Đơn hàng phải từ {coupon.MinOrderAmount.Value:N0} ₫ để dùng mã.");
            decimal rawDiscount = cartTotal * coupon.DiscountPercent / 100m;
            decimal discount = coupon.MaxDiscountAmount.HasValue
                ? Math.Min(rawDiscount, coupon.MaxDiscountAmount.Value)
                : rawDiscount;
            return (discount, "");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order model)
        {
            var cart = _cartService.GetCart(HttpContext.Session);
            if (!cart.Any()) return RedirectToAction("Index", "Cart");

            // Bỏ validate các navigation/property do EF/Identity:
            ModelState.Remove("User");
            ModelState.Remove("UserId");
            ModelState.Remove("OrderDetails");

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            // Nếu invalid thì bạn set ViewBag ngay TẠI ĐÂY (đây chính là “POST invalid model state”)
            if (!ModelState.IsValid)
            {
                SetCheckoutViewBags(cart, user);
                return View(model);
            }

            // Tính lại totals để tránh user sửa HTML
            decimal cartTotal = _cartService.GetTotal(HttpContext.Session);
            int totalItems = cart.Sum(x => x.Quantity);

            decimal rate = GetMembershipDiscountRate(user.MembershipTier);
            decimal memberDiscount = Math.Round(cartTotal * rate, 0);
            decimal shippingFee = CalculateShippingFee(totalItems, cartTotal, user.MembershipTier);
            decimal finalTotal = cartTotal - memberDiscount + shippingFee;

            var order = new Order
            {
                UserId = user.Id,
                FullName = model.FullName,
                Address = model.Address,
                City = model.City,
                PostalCode = model.PostalCode,

                // nếu bạn đã thêm Phone/CustomerEmail vào Order model:
                Phone = model.Phone,
                CustomerEmail = model.CustomerEmail,

                TotalAmount = finalTotal,
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

            // Query lại order kèm Product để email có tên sản phẩm
            var orderForEmail = await _context.Orders
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .FirstAsync(o => o.Id == order.Id);

            // Gửi email (có log lỗi)
            if (!string.IsNullOrWhiteSpace(order.CustomerEmail))
            {
                try
                {
                    await _emailService.SendOrderConfirmationAsync(
                        order.CustomerEmail,
                        user.FullName ?? user.UserName ?? "Khách hàng",
                        orderForEmail);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Send order email failed. OrderId={OrderId}, To={Email}, UserId={UserId}",
                        order.Id, order.CustomerEmail, user.Id);
                    // Không throw để không chặn checkout
                }
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
