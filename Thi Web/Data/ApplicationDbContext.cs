using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechShop.Models;

namespace TechShop.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Review> Reviews { get; set; }
        // Database mới nha Việt Anh:
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductSpecification> ProductSpecifications { get; set; }
        public DbSet<WarrantyPackage> WarrantyPackages { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<ServiceTicket> ServiceTickets { get; set; }
        public DbSet<StoreBranch> StoreBranches { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<StoreInventory> StoreInventories { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Cấu hình Composite Key cho bảng tồn kho chi nhánh
            builder.Entity<StoreInventory>()
                .HasKey(si => new { si.StoreBranchId, si.ProductId });

            // Cấu hình quan hệ tránh xóa vòng lặp
            builder.Entity<StoreInventory>()
                .HasOne(si => si.StoreBranch)
                .WithMany(sb => sb.Inventories)
                .HasForeignKey(si => si.StoreBranchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<StoreInventory>()
                .HasOne(si => si.Product)
                .WithMany(p => p.Inventories)
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Laptop", Description = "Máy tính xách tay các loại" },
                new Category { Id = 2, Name = "Chuột", Description = "Chuột máy tính có dây và không dây" },
                new Category { Id = 3, Name = "Bàn phím", Description = "Bàn phím cơ và membrane" },
                new Category { Id = 4, Name = "Màn hình", Description = "Màn hình máy tính các kích thước" },
                new Category { Id = 5, Name = "Linh kiện PC", Description = "CPU, RAM, GPU, SSD và các linh kiện khác" }
            );
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Laptop ASUS ROG Strix G16",
                    Description = "Laptop gaming cao cấp, CPU Intel Core i9, GPU RTX 4070",
                    Price = 45000000,
                    Stock = 10,
                    CategoryId = 1,
                    IsActive = true,
                    CreatedAt = seedDate,
                    ImageUrl = "https://images.unsplash.com/photo-1525547719571-a2d4ac8945e2?w=400"
                },
                new Product
                {
                    Id = 2,
                    Name = "Laptop Dell XPS 15",
                    Description = "Laptop văn phòng cao cấp, màn hình OLED 15.6 inch",
                    Price = 38000000,
                    Stock = 8,
                    CategoryId = 1,
                    IsActive = true,
                    CreatedAt = seedDate,
                    ImageUrl = "https://images.unsplash.com/photo-1593642632559-0c6d3fc62b89?w=400"
                },
                new Product
                {
                    Id = 3,
                    Name = "Chuột Logitech G Pro X Superlight",
                    Description = "Chuột gaming không dây siêu nhẹ 61g, cảm biến HERO 25K",
                    Price = 2800000,
                    Stock = 25,
                    CategoryId = 2,
                    IsActive = true,
                    CreatedAt = seedDate,
                    ImageUrl = "https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?w=400"
                },
                new Product
                {
                    Id = 4,
                    Name = "Chuột Razer DeathAdder V3",
                    Description = "Chuột gaming ergonomic với cảm biến Focus Pro 30K",
                    Price = 1900000,
                    Stock = 30,
                    CategoryId = 2,
                    IsActive = true,
                    CreatedAt = seedDate,
                    ImageUrl = "https://images.unsplash.com/photo-1615663245857-ac93bb7c39e7?w=400"
                },
                new Product
                {
                    Id = 5,
                    Name = "Bàn phím cơ Keychron K2",
                    Description = "Bàn phím cơ compact 75%, kết nối Bluetooth",
                    Price = 1800000,
                    Stock = 20,
                    CategoryId = 3,
                    IsActive = true,
                    CreatedAt = seedDate,
                    ImageUrl = "https://images.unsplash.com/photo-1587829741301-dc798b83add3?w=400"
                },
                new Product
                {
                    Id = 6,
                    Name = "Bàn phím cơ Ducky One 3",
                    Description = "Bàn phím cơ fullsize cao cấp, hot-swap switch Cherry MX",
                    Price = 2500000,
                    Stock = 15,
                    CategoryId = 3,
                    IsActive = true,
                    CreatedAt = seedDate,
                    ImageUrl = "https://images.unsplash.com/photo-1601445638532-3c6f6c3aa1d6?w=400"
                },
                new Product
                {
                    Id = 7,
                    Name = "Màn hình LG 27GP850-B",
                    Description = "Màn hình gaming 27 inch 2K 165Hz IPS",
                    Price = 8500000,
                    Stock = 12,
                    CategoryId = 4,
                    IsActive = true,
                    CreatedAt = seedDate,
                    ImageUrl = "https://images.unsplash.com/photo-1527443224154-c4a3942d3acf?w=400"
                },
                new Product
                {
                    Id = 8,
                    Name = "Màn hình Samsung Odyssey G7",
                    Description = "Màn hình cong 32 inch 4K 144Hz, VA panel",
                    Price = 15000000,
                    Stock = 6,
                    CategoryId = 4,
                    IsActive = true,
                    CreatedAt = seedDate,
                    ImageUrl = "https://images.unsplash.com/photo-1593642702821-c8da6771f0c6?w=400"
                },
                new Product
                {
                    Id = 9,
                    Name = "GPU NVIDIA RTX 4070 Ti",
                    Description = "Card đồ họa gaming cao cấp 12GB GDDR6X",
                    Price = 22000000,
                    Stock = 5,
                    CategoryId = 5,
                    IsActive = true,
                    CreatedAt = seedDate,
                    ImageUrl = "https://images.unsplash.com/photo-1591488320449-011701bb6704?w=400"
                },
                new Product
                {
                    Id = 10,
                    Name = "RAM Corsair Vengeance DDR5 32GB",
                    Description = "Bộ nhớ DDR5 6000MHz CL36, 2x16GB",
                    Price = 3200000,
                    Stock = 20,
                    CategoryId = 5,
                    IsActive = true,
                    CreatedAt = seedDate,
                    ImageUrl = "https://images.unsplash.com/photo-1562976540-1502c2145851?w=400"
                }
            );
        }
    }
}