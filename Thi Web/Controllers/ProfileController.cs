using System.ComponentModel.DataAnnotations;

namespace TechShop.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _env;
        public ProfileController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var model = new ProfileViewModel
            {
                FullName = user.FullName ?? "",
                Email = user.Email ?? "",
                PhoneNumber = user.PhoneNumber ?? "",
                AvatarUrl = user.AvatarUrl,
                LoyaltyPoints = user.LoyaltyPoints,
                MembershipTier = user.MembershipTier
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfileViewModel model)
        {
            ModelState.Remove("CurrentPassword");
            ModelState.Remove("NewPassword");
            ModelState.Remove("ConfirmPassword");

            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");
            
            if (model.AvatarFile != null && model.AvatarFile.Length > 0)
            {
                var avatarResult = await TrySaveAvatarAsync(user, model.AvatarFile);
                if (!avatarResult.success)
                {
                    TempData["Error"] = avatarResult.message;
                    return View(model);
                }
            }

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                TempData["Success"] = "Cập nhật thông tin thành công!";
            else
                foreach (var e in result.Errors)
                    ModelState.AddModelError("", e.Description);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            if (string.IsNullOrEmpty(model.CurrentPassword) ||
                string.IsNullOrEmpty(model.NewPassword) ||
                string.IsNullOrEmpty(model.ConfirmPassword))
            {
                TempData["Error"] = "Vui lòng điền đầy đủ thông tin mật khẩu.";
                return RedirectToAction(nameof(Index));
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                TempData["Error"] = "Mật khẩu xác nhận không khớp.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["Success"] = "Đổi mật khẩu thành công!";
            }
            else
            {
                TempData["Error"] = "Mật khẩu hiện tại không đúng.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadAvatar(IFormFile? avatarFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            if (avatarFile == null || avatarFile.Length == 0)
            {
                TempData["Error"] = "Vui lòng chọn ảnh avatar.";
                return RedirectToAction(nameof(Index));
            }

            var avatarResult = await TrySaveAvatarAsync(user, avatarFile);
            if (!avatarResult.success)
            {
                TempData["Error"] = avatarResult.message;
                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.UpdateAsync(user);
            TempData[result.Succeeded ? "Success" : "Error"] = result.Succeeded
                ? "Cập nhật avatar thành công!"
                : "Không thể cập nhật avatar.";

            return RedirectToAction(nameof(Index));
        }

        private async Task<(bool success, string message)> TrySaveAvatarAsync(ApplicationUser user, IFormFile avatarFile)
        {
            const long maxBytes = 2 * 1024 * 1024; // 2MB
            if (avatarFile.Length > maxBytes)
                return (false, "Avatar quá lớn (tối đa 2MB).");

            var allowedExt = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(avatarFile.FileName).ToLowerInvariant();
            if (!allowedExt.Contains(ext))
                return (false, "Chỉ cho phép ảnh .jpg, .jpeg, .png, .webp");

            if (avatarFile.ContentType == null || !avatarFile.ContentType.StartsWith("image/"))
                return (false, "File upload không phải hình ảnh.");

            var uploads = Path.Combine(_env.WebRootPath, "uploads", "avatars");
            Directory.CreateDirectory(uploads);

            var fileName = $"{user.Id}_{Guid.NewGuid():N}{ext}";
            var filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.CreateNew))
            {
                await avatarFile.CopyToAsync(stream);
            }

            if (!string.IsNullOrEmpty(user.AvatarUrl) && user.AvatarUrl.StartsWith("/uploads/avatars/"))
            {
                var oldPath = Path.Combine(_env.WebRootPath, user.AvatarUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            user.AvatarUrl = "/uploads/avatars/" + fileName;
            return (true, "");
        }
    }

    public class ProfileViewModel
    {
        public IFormFile? AvatarFile { get; set; }
        public string? AvatarUrl { get; set; }
        public int LoyaltyPoints { get; set; }
        public string MembershipTier { get; set; } = "Bronze";

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu hiện tại")]
        public string? CurrentPassword { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu mới")]
        public string? ConfirmPassword { get; set; }
    }
}