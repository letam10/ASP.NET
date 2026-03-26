using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models
{
    // ===== VIEW MODELS =====
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        [Display(Name = "Xác nhận mật khẩu")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }

    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [StringLength(200)]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá (VNĐ)")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }

        // Tính năng: Sales, Thu hút khách hàng
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountPrice { get; set; }

        // Tính năng: Thu cũ đổi mới
        public bool IsTradeInEligible { get; set; } = false;
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxTradeInValue { get; set; }

        // Chính sách bảo hành mặc định
        [StringLength(200)]
        public string WarrantyPolicy { get; set; } = "Bảo hành chính hãng 12 tháng";
        public string? ImageUrl { get; set; } // Ảnh đại diện
        public int Stock { get; set; } // Tổng tồn kho
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public List<ProductVariant> Variants { get; set; } = new();
        public ICollection<ProductVariantSelection> SelectedVariantOptions { get; set; } = new List<ProductVariantSelection>();
        // Quan hệ mới được thêm vào
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<ProductSpecification> Specifications { get; set; } = new List<ProductSpecification>();
        public ICollection<WarrantyPackage> WarrantyPackages { get; set; } = new List<WarrantyPackage>();
        public ICollection<StoreInventory> Inventories { get; set; } = new List<StoreInventory>();
        public ICollection<Wishlist> WishlistedBy { get; set; } = new List<Wishlist>();
    }
    // 1. Ảnh con hiển thị chi tiết
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;
    }

    // 2. Thông số kỹ thuật
    public class ProductSpecification
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        [StringLength(100)]
        public string SpecName { get; set; } = string.Empty; //"CPU", "GPU", "Màn hình"
        [StringLength(255)]
        public string SpecValue { get; set; } = string.Empty; //"Intel Core i9-14900HX", "RTX 4070 8GB"
    }

    // 3. Gói nâng cấp bảo hành
    public class WarrantyPackage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public string PackageName { get; set; } = string.Empty; //"Gói bảo hành vàng ? tháng"
        [Column(TypeName = "decimal(18,2)")]
        public decimal AdditionalPrice { get; set; } // Giá mua thêm
    }

    // 4. Sản phẩm yêu thích
    public class Wishlist
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.Now;
    }

    // 5. Đặt lịch Sửa chữa, Vệ sinh máy
    public class ServiceTicket
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập họ tên của bạn")]
        public string CustomerName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = "Vui lòng nhập tên thiết bị hoặc linh kiện")]
        public string DeviceModel { get; set; } = string.Empty; // Tên máy tính/Linh kiện
        public string ServiceType { get; set; } = "Vệ sinh máy"; //"Sửa chữa", "Nâng cấp"
        public string? Description { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = "Đang chờ xử lý"; // Đang chờ xử lý, Đang tiến hành, Đã hoàn thành, Đã hủy
    }

    // 6. Quản lý chi nhánh
    public class StoreBranch
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public ICollection<StoreInventory> Inventories { get; set; } = new List<StoreInventory>();
    }

    // 7. Nhà cung cấp
    public class Supplier
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }

    // 8. Quản lý kho trung gian
    public class StoreInventory
    {
        public int StoreBranchId { get; set; }
        public StoreBranch? StoreBranch { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Thành phố là bắt buộc")]
        [Display(Name = "Thành phố")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã bưu điện là bắt buộc")]
        [Display(Name = "Mã bưu điện")]
        public string PostalCode { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Tổng tiền")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "Pending";

        [Display(Name = "Ngày đặt")]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public bool LoyaltyPointsAwarded { get; set; } = false;
    }

    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Đơn giá")]
        public decimal UnitPrice { get; set; }
    }

    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Subtotal => Price * Quantity;
        public decimal Total { get; set; }
        public int? ProductVariantId { get; set; }
    }
    // Ghi chép lịch sử nhận/tiêu hoa hồng
    public class CommissionLog
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty; // VD: "Hoa hồng từ đơn hàng #123 của bạn bè"
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ExpiryDate { get; set; } = new DateTime(2026, 12, 31); // Thời hạn mặc định
        public bool IsUsed { get; set; } = false;
    }
    // ViewModel cho chức năng quên mật khẩu
    public class ForgotPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
    public class ResetPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    //view model cho biến thể
    public class ProductVariantGroup
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; // Màu Sắc, Dung Lượng, Nâng Cấp

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        public List<ProductVariantOption> Options { get; set; } = new();
    }
    public class ProductVariantOption
    {
        public int Id { get; set; }

        public int ProductVariantGroupId { get; set; }
        public ProductVariantGroup? ProductVariantGroup { get; set; }

        [Required]
        public string Value { get; set; } = string.Empty; // Xanh, Đỏ, 16GB, 1TB, Pro...

        public string? ColorHex { get; set; } // chỉ dùng cho màu sắc, ví dụ #000000
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }
    public class ProductVariantSelection
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int ProductVariantOptionId { get; set; }
        public ProductVariantOption? ProductVariantOption { get; set; }
    }
    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Sku { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; } = true;
        public Product? Product { get; set; }
        public List<ProductVariantValue> Values { get; set; } = new();
    }
    public class ProductVariantValue
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public int ProductVariantOptionId { get; set; }
        public ProductVariant? ProductVariant { get; set; }
        public ProductVariantOption? ProductVariantOption { get; set; }
    }
    //Quản lý mã giảm giá
    public class Coupon
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public int DiscountPercent { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public decimal? MinOrderAmount { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
