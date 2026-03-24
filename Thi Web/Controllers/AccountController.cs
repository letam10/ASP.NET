using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using TechShop.Models;

namespace TechShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        // ===== LOGIN =====
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                TempData["Success"] = "Đăng nhập thành công!";
                return RedirectToLocal(returnUrl);
            }

            if (result.RequiresTwoFactor)
                return RedirectToAction(nameof(TwoFactorLogin), new { rememberMe = model.RememberMe });

            ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
            return View(model);
        }

        // ===== REGISTER =====
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
                await _signInManager.SignInAsync(user, false);

                try
                {
                    await _emailService.SendAsync(
                        model.Email,
                        "Chào mừng đến với TechShop",
                        $"<h2>Xin chào {System.Net.WebUtility.HtmlEncode(model.FullName)}</h2><p>Tài khoản của bạn đã được tạo thành công.</p>");
                }
                catch
                {
                    // không chặn đăng ký nếu email lỗi
                }

                TempData["Success"] = "Đăng ký thành công! Chào mừng bạn đến với TechShop.";
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        // ===== LOGOUT =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();

        // ===== GOOGLE & FACEBOOK LOGIN =====
        [HttpGet]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["Error"] = "Không lấy được thông tin từ mạng xã hội. Vui lòng thử lại.";
                return RedirectToAction(nameof(Login));
            }

            // Thử đăng nhập bằng external provider
            var result = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (result.Succeeded)
            {
                TempData["Success"] = $"Đăng nhập bằng {info.LoginProvider} thành công!";
                return RedirectToLocal(returnUrl);
            }

            // Lấy email & name từ claims
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name) ?? "";

            // Facebook đôi khi không trả email — dùng ProviderKey làm fallback
            if (string.IsNullOrEmpty(email))
            {
                email = $"{info.ProviderKey}@facebook.com";
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = name,
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    foreach (var error in createResult.Errors)
                        ModelState.AddModelError("", error.Description);

                    TempData["Error"] = "Không thể tạo tài khoản: " +
                        string.Join(", ", createResult.Errors.Select(e => e.Description));
                    return RedirectToAction(nameof(Login));
                }

                await _userManager.AddToRoleAsync(user, "Customer");

                try
                {
                    await _emailService.SendAsync(
                        user.Email!,
                        "Đăng ký bằng Google thành công",
                        $"<p>Xin chào {System.Net.WebUtility.HtmlEncode(user.FullName ?? user.Email!)}, tài khoản Google của bạn đã liên kết thành công với TechShop.</p>");
                }
                catch
                {
                    // không chặn luồng login
                }
            }

            // Gắn external login vào tài khoản
            var loginInfo = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (loginInfo == null)
            {
                await _userManager.AddLoginAsync(user, info);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            TempData["Success"] = $"Đăng nhập bằng {info.LoginProvider} thành công!";
            return RedirectToLocal(returnUrl);
        }

        // ===== 2FA SETUP =====
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> TwoFactorSetup()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            var key = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(key))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                key = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            ViewBag.Key = key;
            ViewBag.QrCode = $"otpauth://totp/TechShop:{user.Email}?secret={key}&issuer=TechShop";
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TwoFactorSetup(string code)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            var isValid = await _userManager.VerifyTwoFactorTokenAsync(
                user,
                _userManager.Options.Tokens.AuthenticatorTokenProvider,
                code.Replace(" ", "").Replace("-", ""));

            if (!isValid)
            {
                TempData["Error"] = "Mã xác minh không đúng. Vui lòng thử lại.";
                return RedirectToAction(nameof(TwoFactorSetup));
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            TempData["Success"] = "Đã bật xác minh 2 bước thành công!";
            return RedirectToAction("Index", "Profile");
        }

        // ===== 2FA DISABLE =====
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableTwoFactor()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
                await _userManager.SetTwoFactorEnabledAsync(user, false);

            TempData["Success"] = "Đã tắt xác minh 2 bước.";
            return RedirectToAction("Index", "Profile");
        }

        // ===== 2FA LOGIN =====
        [HttpGet]
        public IActionResult TwoFactorLogin(bool rememberMe)
        {
            ViewBag.RememberMe = rememberMe;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TwoFactorLogin(string code, bool rememberMe)
        {
            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(
                code.Replace(" ", "").Replace("-", ""), rememberMe, false);

            if (result.Succeeded)
            {
                TempData["Success"] = "Đăng nhập thành công!";
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Mã xác minh không đúng. Vui lòng thử lại.";
            return View();
        }

        // ===== HELPER =====
        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        // ===== FORGOT & RESET PASSWORD =====
        [HttpGet]
        public IActionResult ForgotPassword() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            // Không tiết lộ email có tồn tại hay không
            if (user == null)
                return RedirectToAction(nameof(ForgotPasswordConfirmation));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var link = Url.Action(nameof(ResetPassword), "Account",
                new { email = user.Email, token },
                protocol: Request.Scheme);

            await _emailService.SendPasswordResetEmailAsync(user.Email!, link!);
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation() => View();
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            return View(new ResetPasswordViewModel { Email = email, Token = token });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            // Không tiết lộ email
            if (user == null)
                return RedirectToAction(nameof(ResetPasswordConfirmation));

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
                return RedirectToAction(nameof(ResetPasswordConfirmation));

            foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
            return View(model);
        }
        [HttpGet]
        public IActionResult ResetPasswordConfirmation() => View();
    }
}