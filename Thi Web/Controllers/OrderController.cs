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
            if (tier == "Diamond") return 0;

            decimal shipping = 35000;

            if (totalItems >= 3 || cartTotal >= 5_000_000m)
                shipping = 15000;

            return shipping;
        }

        private async Task SetCheckoutViewBagsAsync(List<CartItem> cart, ApplicationUser user, string? couponCode = null, string? couponErrorOverride = null)
        {
            decimal cartTotal = _cartService.GetTotal(HttpContext.Session);
            int totalItems = cart.Sum(x => x.Quantity);

            decimal rate = GetMembershipDiscountRate(user.MembershipTier);
            decimal memberDiscount = Math.Round(cartTotal * rate, 0);
            var (couponDiscount, couponError, _) = await ApplyCouponAsync(couponCode, cartTotal);

            decimal shippingFee = CalculateShippingFee(totalItems, cartTotal, user.MembershipTier);
            decimal finalTotal = cartTotal - memberDiscount - couponDiscount + shippingFee;
            if (finalTotal < 0) finalTotal = 0;

            ViewBag.Cart = cart;
            ViewBag.CartTotal = cartTotal;
            ViewBag.MemberDiscount = memberDiscount;
            ViewBag.CouponDiscount = couponDiscount;
            ViewBag.CouponCode = couponCode;
            ViewBag.CouponError = couponErrorOverride ?? couponError;
            ViewBag.ShippingFee = shippingFee;
            ViewBag.FinalTotal = finalTotal;

            ViewBag.Total = finalTotal;

            ViewBag.ItemCount = totalItems;
            ViewBag.MembershipTier = user.MembershipTier;
            ViewBag.DiscountRate = rate;
        }

        private async Task<(decimal discount, string message, Coupon? coupon)> ApplyCouponAsync(string? couponCode, decimal cartTotal)
        {
            if (string.IsNullOrWhiteSpace(couponCode))
                return (0m, "", null);
            couponCode = couponCode.Trim().ToUpperInvariant();
            var coupon = await _context.Coupons.FirstOrDefaultAsync(x =>
                x.Code == couponCode &&
                x.IsActive &&
                x.Quantity > 0 &&
                (!x.ExpiredAt.HasValue || x.ExpiredAt > DateTime.Now));
            if (coupon == null)
                return (0m, "Mã giảm giá không hợp lệ.", null);
            if (coupon.MinOrderAmount.HasValue && cartTotal < coupon.MinOrderAmount.Value)
                return (0m, $"Đơn hàng phải từ {coupon.MinOrderAmount.Value:N0} ₫ để dùng mã.", null);
            decimal rawDiscount = cartTotal * coupon.DiscountPercent / 100m;
            decimal discount = coupon.MaxDiscountAmount.HasValue
                ? Math.Min(rawDiscount, coupon.MaxDiscountAmount.Value)
                : rawDiscount;
            return (discount, "", coupon);
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var cart = _cartService.GetCart(HttpContext.Session);
            if (!cart.Any()) return RedirectToAction("Index", "Cart");

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var model = new Order
            {
                FullName = user.FullName ?? user.UserName ?? string.Empty,
                Phone = user.PhoneNumber ?? string.Empty,
                CustomerEmail = user.Email ?? string.Empty
            };

            await SetCheckoutViewBagsAsync(cart, user);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order model, string? couponCode, string? paymentMethod)
        {
            var cart = _cartService.GetCart(HttpContext.Session);
            if (!cart.Any()) return RedirectToAction("Index", "Cart");

            ModelState.Remove("User");
            ModelState.Remove("UserId");
            ModelState.Remove("OrderDetails");

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                await SetCheckoutViewBagsAsync(cart, user, couponCode);
                return View(model);
            }

            decimal cartTotal = _cartService.GetTotal(HttpContext.Session);
            int totalItems = cart.Sum(x => x.Quantity);
            var (couponDiscount, couponError, appliedCoupon) = await ApplyCouponAsync(couponCode, cartTotal);
            if (!string.IsNullOrWhiteSpace(couponError))
            {
                ModelState.AddModelError("", couponError);
                await SetCheckoutViewBagsAsync(cart, user, couponCode, couponError);
                return View(model);
            }

            decimal rate = GetMembershipDiscountRate(user.MembershipTier);
            decimal memberDiscount = Math.Round(cartTotal * rate, 0);
            decimal shippingFee = CalculateShippingFee(totalItems, cartTotal, user.MembershipTier);
            decimal finalTotal = cartTotal - memberDiscount - couponDiscount + shippingFee;
            if (finalTotal < 0) finalTotal = 0;

            var normalizedPaymentMethod = string.Equals(paymentMethod, "bank", StringComparison.OrdinalIgnoreCase)
                ? "bank"
                : "cod";
            var customerEmail = (model.CustomerEmail ?? string.Empty).Trim();

            _logger.LogInformation("Checkout POST: received paymentMethod={PaymentMethodParam}, normalized={Normalized}", paymentMethod, normalizedPaymentMethod);

            var order = new Order
            {
                UserId = user.Id,
                FullName = model.FullName,
                Address = model.Address,
                City = model.City,
                PostalCode = model.PostalCode,
                Phone = model.Phone,
                CustomerEmail = customerEmail,
                TotalAmount = finalTotal,
                Status = normalizedPaymentMethod == "bank" ? "AwaitingBankTransfer" : "Pending",
                OrderDate = DateTime.Now,
                OrderDetails = cart.Select(c => new OrderDetail
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    UnitPrice = c.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            if (appliedCoupon != null)
                appliedCoupon.Quantity = Math.Max(0, appliedCoupon.Quantity - 1);
            await _context.SaveChangesAsync();

            var orderForEmail = await _context.Orders
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .FirstAsync(o => o.Id == order.Id);

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
                    TempData["Warning"] = "Đặt hàng thành công nhưng gửi email xác nhận thất bại. Vui lòng kiểm tra cấu hình SMTP.";
                }
            }

            _cartService.ClearCart(HttpContext.Session);

            // Pass paymentMethod explicitly to Completed view to avoid ambiguity
            return RedirectToAction(nameof(Completed), new { id = order.Id, paymentMethod = normalizedPaymentMethod });
        }

        public async Task<IActionResult> Completed(int id, string? paymentMethod)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            // Decide which method to show:
            // priority: explicit paymentMethod param (sent from Checkout redirect),
            // fallback: order.Status (existing logic).
            string resolved = (paymentMethod ?? (string.IsNullOrWhiteSpace(order.Status) ? "cod" :
                (order.Status.Equals("AwaitingBankTransfer", StringComparison.OrdinalIgnoreCase) ? "bank" : "cod")));

            ViewBag.PaymentMethod = resolved;
            _logger.LogInformation("Completed: OrderId={OrderId}, Order.Status={Status}, ResolvedPaymentMethod={Resolved}", order.Id, order.Status, resolved);

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