using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Thi_Web.Migrations
{
    /// <inheritdoc />
    public partial class AddProductVariants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReferralCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferredByCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommissionBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LoyaltyPoints = table.Column<int>(type: "int", nullable: false),
                    MembershipTier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountPercent = table.Column<int>(type: "int", nullable: false),
                    MaxDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinOrderAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariantGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceModel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreBranches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreBranches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommissionLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommissionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommissionLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoyaltyPointsAwarded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsTradeInEligible = table.Column<bool>(type: "bit", nullable: false),
                    MaxTradeInValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WarrantyPolicy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariantOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductVariantGroupId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariantOptions_ProductVariantGroups_ProductVariantGroupId",
                        column: x => x.ProductVariantGroupId,
                        principalTable: "ProductVariantGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSpecifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SpecName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SpecValue = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSpecifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSpecifications_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Sku = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreInventories",
                columns: table => new
                {
                    StoreBranchId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreInventories", x => new { x.StoreBranchId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_StoreInventories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreInventories_StoreBranches_StoreBranchId",
                        column: x => x.StoreBranchId,
                        principalTable: "StoreBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarrantyPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    PackageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarrantyPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarrantyPackages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wishlists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wishlists_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wishlists_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariantValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductVariantId = table.Column<int>(type: "int", nullable: false),
                    ProductVariantOptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariantValues_ProductVariantOptions_ProductVariantOptionId",
                        column: x => x.ProductVariantOptionId,
                        principalTable: "ProductVariantOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductVariantValues_ProductVariants_ProductVariantId",
                        column: x => x.ProductVariantId,
                        principalTable: "ProductVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Máy tính xách tay các loại", "Laptop" },
                    { 2, "Chuột máy tính có dây và không dây", "Chuột" },
                    { 3, "Bàn phím cơ và membrane", "Bàn phím" },
                    { 4, "Màn hình máy tính các kích thước", "Màn hình" },
                    { 5, "CPU, RAM, GPU, SSD và các linh kiện khác", "Linh kiện PC" },
                    { 6, "Tai nghe gaming và văn phòng", "Tai nghe" },
                    { 7, "SSD, HDD lưu trữ dữ liệu", "Ổ cứng" },
                    { 8, "Ghế gaming chuyên dụng", "Ghế gaming" },
                    { 9, "Smartphone iOS và Android", "Điện thoại" },
                    { 10, "Key Office, Windows, phần mềm AI và thiết kế", "Bản quyền phần mềm" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "DiscountPrice", "ImageUrl", "IsActive", "IsTradeInEligible", "MaxTradeInValue", "Name", "Price", "Stock", "WarrantyPolicy" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Laptop gaming cao cấp Intel Core i9-14900HX, RTX 4070 8GB, RAM 16GB DDR5, SSD 1TB PCIe 4.0, màn hình 16 inch QHD 165Hz", 42990000m, "https://dlcdnwebimgs.asus.com/gain/9BCCE5B8-9B6F-4857-B1B9-04FF37DAD6E1/w1000/h732", true, false, null, "ASUS ROG Strix G16 2024", 45990000m, 10, "Bảo hành chính hãng 12 tháng" },
                    { 2, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Laptop văn phòng cao cấp Intel Core i7-13700H, RTX 4060 8GB, màn hình OLED 3.5K 60Hz, RAM 16GB, SSD 512GB, pin 86Whr", null, "https://i.dell.com/is/image/DellContent/content/dam/ss2/product-images/dell-client-products/notebooks/xps-notebooks/xps-15-9530/media-gallery/black/notebook-xps-15-9530-nt-black-gallery-1.psd?fmt=png-alpha&pscan=auto&scl=1&hei=402&wid=402&qlt=100,1&resMode=sharp2&size=402,402&chrss=full", true, false, null, "Dell XPS 15 9530", 38990000m, 8, "Bảo hành chính hãng 12 tháng" },
                    { 3, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Laptop Apple chip M3 Pro 11 nhân, màn hình Liquid Retina XDR 14.2 inch, RAM 18GB, SSD 512GB, pin 18 giờ, MagSafe 3", null, "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/mbp14-spaceblack-select-202310?wid=904&hei=840&fmt=jpeg&qlt=90&.v=1697230830200", true, false, null, "MacBook Pro M3 Pro 14 inch", 52990000m, 6, "Bảo hành chính hãng 12 tháng" },
                    { 4, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Laptop mỏng nhẹ Apple chip M3 8 nhân, màn hình Liquid Retina 15.3 inch, RAM 8GB, SSD 256GB, pin 18 giờ, không quạt tản nhiệt", 30990000m, "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/mba15-midnight-select-202306?wid=904&hei=840&fmt=jpeg&qlt=90&.v=1684518479433", true, false, null, "MacBook Air M3 15 inch", 32990000m, 10, "Bảo hành chính hãng 12 tháng" },
                    { 5, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Laptop doanh nhân siêu mỏng Intel Core Ultra 7 155U, RAM 32GB LPDDR5, SSD 1TB, màn hình 2.8K OLED, pin 15 giờ, cân nặng chỉ 1.12kg", null, "https://p1-ofp.static.pub/medias/bWFzdGVyfHJvb3R8MjQ2NjI5fGltYWdlL3BuZ3xoZTYvaGRkLzE0Mjc5NTM5MDYyNjE0LnBuZ3w0YjdiMzJjYmNhMzFkMGEzMGM0YzYxZDQ4OThhMzY5NGRhNjBhODA3YmFiMTBjODU1MzNhYzFiZDA3ZjY5YTAy/Lenovo-laptop-thinkpad-x1-carbon-gen-12-hero.png", true, false, null, "Lenovo ThinkPad X1 Carbon Gen 12", 42990000m, 5, "Bảo hành chính hãng 12 tháng" },
                    { 6, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Laptop 2-in-1 cao cấp Intel Core Ultra 5 125H, màn hình OLED cảm ứng 2.8K 120Hz, RAM 16GB, SSD 512GB, bút HP Tilt Pen đi kèm", 32990000m, "https://ssl-product-images.www8-hp.com/digmedialib/prodimg/knowledgebase/c08586213.png", true, false, null, "HP Spectre x360 14", 35990000m, 7, "Bảo hành chính hãng 12 tháng" },
                    { 7, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Laptop gaming tầm trung Intel Core i5-13500H, RTX 4050 6GB, RAM 16GB DDR5, SSD 512GB, màn hình 16 inch FHD 144Hz, tản nhiệt 4 quạt", 20990000m, "https://static-ecshop.acer.com/media/catalog/product/n/h/nh.qlfsv.00e_1_1.png", true, false, null, "Acer Nitro 16 AN16-51", 22990000m, 15, "Bảo hành chính hãng 12 tháng" },
                    { 8, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Laptop gaming flagship Intel Core i9-14900HX, RTX 4090 16GB, RAM 64GB DDR5, SSD 2TB, màn hình 17 inch QHD+ 240Hz, Thunderbolt 4", null, "https://asset.msi.com/resize/image/global/product/product_16920631213e2f6d573ead3.png62405b38c58fe0f07fcef2367d8a9ba1/1024.png", true, false, null, "MSI Raider GE78 HX", 79990000m, 3, "Bảo hành chính hãng 12 tháng" },
                    { 9, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Laptop mỏng nhẹ Intel Core Ultra 9 185H, màn hình OLED 2.8K 120Hz, RAM 32GB LPDDR5X, SSD 1TB, pin 75Whr sạc nhanh 65W", null, "https://dlcdnwebimgs.asus.com/gain/C43AECF2-D312-4BDA-B8D1-43BD8A2C3D99/w1000/h732", true, false, null, "ASUS Zenbook 14 OLED UX3405", 28990000m, 12, "Bảo hành chính hãng 12 tháng" },
                    { 10, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Laptop gaming siêu cao cấp Intel Core i9-14900HX, RTX 4090 16GB, màn hình OLED 4K 240Hz, RAM 32GB DDR5, SSD 2TB", 84990000m, "https://assets2.razerzone.com/images/pnx.assets/93f03d7a0bf8a50d7a8e8e2b2bc1e4a2/razer-blade-16-2024-500x500.png", true, false, null, "Razer Blade 16 2024", 89990000m, 3, "Bảo hành chính hãng 12 tháng" },
                    { 11, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Laptop siêu nhẹ Intel Core Ultra 7 155H, màn hình 17 inch WQXGA IPS, RAM 16GB, SSD 512GB, cân nặng 1.35kg, pin 90Whr 20 giờ", null, "https://www.lg.com/us/images/laptops/md08003753/gallery/large01.jpg", true, false, null, "LG Gram 17 2024", 34990000m, 8, "Bảo hành chính hãng 12 tháng" },
                    { 12, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Laptop 2-in-1 màn hình AMOLED 16 inch 3K cảm ứng, Intel Core Ultra 7 155H, RAM 16GB, SSD 512GB, bút S Pen tích hợp, pin 76Whr", 35990000m, "https://images.samsung.com/is/image/samsung/p6pim/vn/2401/gallery/vn-galaxy-book4-pro-360-np960qgk-kg1vn-thumb-539493745", true, false, null, "Samsung Galaxy Book4 Pro 360", 38990000m, 6, "Bảo hành chính hãng 12 tháng" },
                    { 13, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chuột gaming không dây siêu nhẹ 60g, cảm biến HERO 2 25600 DPI, pin 95 giờ, kết nối LIGHTSPEED, thiết kế đối xứng pro", 2690000m, "https://resource.logitech.com/w_692,c_lpad,ar_4:3,q_auto,f_auto,dpr_1.0/d_transparent.gif/content/dam/gaming/en/products/pro-x-superlight-2/pro-x-superlight-2-gallery-1.png", true, false, null, "Logitech G Pro X Superlight 2", 2990000m, 30, "Bảo hành chính hãng 12 tháng" },
                    { 14, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chuột gaming ergonomic không dây, cảm biến Focus Pro 30000 DPI, 6 nút lập trình, pin 90 giờ, kết nối HyperSpeed 4000Hz", null, "https://assets2.razerzone.com/images/pnx.assets/07cf6e7eb0e7e8f55ad32e2b17d5d7c9/razer-deathadder-v3-pro-500x500.png", true, false, null, "Razer DeathAdder V3 Pro", 2490000m, 25, "Bảo hành chính hãng 12 tháng" },
                    { 15, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chuột gaming không dây siêu nhẹ 74g lưới, cảm biến TrueMove Air+ 18000 DPI, IP54, 9 nút, pin 180 giờ", 1790000m, "https://media.steelseriescdn.com/thumbs/catalogue/products/01806-aerox-5-wireless/5e9e1a4f1e6a4b3baa3c0e0e0e0e0e0e/aerox-5-wireless-pdp-hero.png", true, false, null, "SteelSeries Aerox 5 Wireless", 1990000m, 20, "Bảo hành chính hãng 12 tháng" },
                    { 16, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chuột văn phòng cao cấp, cảm biến 8000 DPI, cuộn MagSpeed điện từ, pin 70 ngày, Bluetooth + USB, 7 nút tùy chỉnh", null, "https://resource.logitech.com/w_692,c_lpad,ar_4:3,q_auto,f_auto,dpr_1.0/d_transparent.gif/content/dam/logitech/en/products/mice/mx-master-3s/gallery/mx-master-3s-mouse-top-view-graphite.png", true, false, null, "Logitech MX Master 3S", 1690000m, 40, "Bảo hành chính hãng 12 tháng" },
                    { 17, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chuột gaming không dây esports, cảm biến 3200 DPI, thiết kế ergonomic tay phải, không cần driver, kết nối 2.4G độ trễ thấp", null, "https://zowie.benq.com/content/dam/zowie/products/mouse/ec2-cw/gallery/ec2-cw-front.png", true, false, null, "Zowie EC2-CW", 2190000m, 15, "Bảo hành chính hãng 12 tháng" },
                    { 18, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chuột gaming không dây siêu nhẹ 55g, cảm biến PAW3395 26000 DPI, thiết kế đối xứng, pin 70 giờ, polling 1000Hz", null, "https://www.pulsar.gg/cdn/shop/files/x2v2-white-hero.png?v=1706864731&width=1080", true, false, null, "Pulsar X2V2 Wireless", 1890000m, 18, "Bảo hành chính hãng 12 tháng" },
                    { 19, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chuột không dây Apple thiết kế mỏng phẳng, Multi-Touch surface, kết nối Bluetooth, sạc Lightning, tương thích Mac", null, "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/MMMQ3?wid=1144&hei=1144&fmt=jpeg&qlt=90&.v=1645138962000", true, false, null, "Apple Magic Mouse", 1590000m, 25, "Bảo hành chính hãng 12 tháng" },
                    { 20, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chuột gaming không dây, cảm biến Focus Pro 30000 DPI, cuộn HyperScroll Tilt, 11 nút lập trình, đèn RGB Chroma, pin 90 giờ", 1990000m, "https://assets2.razerzone.com/images/pnx.assets/c0d1ff37f06e07fecef4bb7a234a82e5/razer-basilisk-v3-pro-500x500.png", true, false, null, "Razer Basilisk V3 Pro", 2290000m, 22, "Bảo hành chính hãng 12 tháng" },
                    { 21, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chuột gaming không dây 89g, cảm biến HERO 25600 DPI, 13 nút lập trình, đèn LIGHTFORCE, cuộn LIGHTTUNE, pin 130 giờ", null, "https://resource.logitech.com/w_692,c_lpad,ar_4:3,q_auto,f_auto,dpr_1.0/d_transparent.gif/content/dam/gaming/en/products/g502-x-plus/g502-x-plus-gallery-1-black.png", true, false, null, "Logitech G502 X Plus", 2190000m, 18, "Bảo hành chính hãng 12 tháng" },
                    { 22, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chuột gaming không dây siêu nhẹ 60g, cảm biến PixArt PAW3395 26000 DPI, pin 200 giờ, thiết kế ergonomic tay phải, USB-C sạc", null, "https://www.corsair.com/medias/sys_master/images/images/h72/hec/9476017373214/CH-931D010-NA-Gallery-M75-Air-Wireless-01.png", true, false, null, "Corsair M75 Air Wireless", 1790000m, 20, "Bảo hành chính hãng 12 tháng" },
                    { 23, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bàn phím cơ 75% không dây, khung nhôm CNC, switch QMK/VIA, hotswap, Bluetooth 5.1, pin 4000mAh, gasket mount", null, "https://cdn.shopify.com/s/files/1/0059/0630/1017/files/Keychron-Q1-Pro-QMK-VIA-wireless-custom-mechanical-keyboard-1_1800x1800.jpg", true, false, null, "Keychron Q1 Pro", 3290000m, 20, "Bảo hành chính hãng 12 tháng" },
                    { 24, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bàn phím cơ 65% premium switch Cherry MX Red, hotswap socket, đèn RGB per-key, thiết kế compact gaming, PBT double-shot keycaps", 2490000m, "https://www.duckychannel.com.tw/upload/images/keyboard/one3/one3-sf-matcha/2022_one3_sf_matcha_01.jpg", true, false, null, "Ducky One 3 SF 65%", 2690000m, 15, "Bảo hành chính hãng 12 tháng" },
                    { 25, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bàn phím cơ TKL không dây GL Low Profile switch, đèn RGB LIGHTSYNC, pin 40 giờ, LIGHTSPEED 1ms, Bluetooth 5.0, thiết kế mỏng", null, "https://resource.logitech.com/w_692,c_lpad,ar_4:3,q_auto,f_auto,dpr_1.0/d_transparent.gif/content/dam/gaming/en/products/g915-tkl/gallery/g915-tkl-keyboard-top-view-black.png", true, false, null, "Logitech G915 TKL Wireless", 3990000m, 10, "Bảo hành chính hãng 12 tháng" },
                    { 26, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bàn phím cơ fullsize switch Razer Yellow, đèn Chroma RGB per-key, macro dial, wrist rest từ tính, USB passthrough, media keys", null, "https://assets2.razerzone.com/images/pnx.assets/ba8a3b1f68b26fae3dddca5dbb4b6840/razer-blackwidow-v4-pro-500x500.png", true, false, null, "Razer BlackWidow V4 Pro", 3490000m, 12, "Bảo hành chính hãng 12 tháng" },
                    { 27, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bàn phím cơ TKL giá tốt switch Akko CS Jelly Pink, hotswap, đèn RGB, PBT double-shot keycaps, thiết kế màu sắc đẹp", 990000m, "https://en.akkogear.com/wp-content/uploads/2021/08/Akko-3087-DS-Ocean-Star-1.jpg", true, false, null, "Akko 3087 DS Ocean Star", 1190000m, 35, "Bảo hành chính hãng 12 tháng" },
                    { 28, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bàn phím cơ fullsize cao cấp switch Cherry MX Brown, thiết kế tối giản không đèn, PBT double-shot keycaps bền, chất lượng build quality xuất sắc", null, "https://mechanicalkeyboards.com/cdn/shop/products/fc900r-pd-white-blue_grande.jpg", true, false, null, "Leopold FC900R PD", 1990000m, 8, "Bảo hành chính hãng 12 tháng" },
                    { 29, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bàn phím không dây Apple slim profile, phím scissor mechanism, Touch ID tích hợp, Bluetooth, sạc Lightning, dành cho Mac", null, "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/MK293LL?wid=1144&hei=1144&fmt=jpeg&qlt=90&.v=1645138963000", true, false, null, "Apple Magic Keyboard Touch ID", 2390000m, 15, "Bảo hành chính hãng 12 tháng" },
                    { 30, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bàn phím cơ 75% không dây low profile, switch Gateron Low Profile 2.0, Bluetooth 5.0 + USB, RGB, pin 3000mAh, gasket mount", 1990000m, "https://cdn.shopify.com/s/files/1/0268/9673/3662/products/NuPhy-Air75-V2-Wireless-Mechanical-Keyboard-1.jpg", true, false, null, "NuPhy Air75 V2", 2190000m, 18, "Bảo hành chính hãng 12 tháng" },
                    { 31, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bàn phím cơ 60% analog hall effect, rapid trigger công nghệ, switch Lekker 60g, polling 1000Hz, dành cho esports pro player", null, "https://wooting.io/static/wooting-60he-plus-product-shot-b19f9d5f1a4e5a6d2c7b8e9f0a1b2c3d.jpg", true, false, null, "Wooting 60HE+", 3890000m, 8, "Bảo hành chính hãng 12 tháng" },
                    { 32, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bàn phím gaming TKL không dây, switch OmniPoint 2.0 điều chỉnh actuation, OLED display, đèn RGB, LIGHTSPEED 1ms, pin 45 giờ", 3990000m, "https://media.steelseriescdn.com/thumbs/catalogue/products/01776-apex-pro-tkl-wireless/98ae37f8c8bc4c2d8c2d4e5f6a7b8c9d/apex-pro-tkl-wireless-pdp-hero.png", true, false, null, "SteelSeries Apex Pro TKL Wireless", 4490000m, 10, "Bảo hành chính hãng 12 tháng" },
                    { 33, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Màn hình gaming 27 inch 2K 165Hz Nano IPS, 1ms GtG, AMD FreeSync Premium Pro, G-Sync Compatible, HDR400, DisplayPort 1.4 + HDMI 2.0", 7990000m, "https://www.lg.com/us/images/monitors/md07521413/gallery/large01.jpg", true, false, null, "LG 27GP850-B Nano IPS 2K 165Hz", 8990000m, 12, "Bảo hành chính hãng 12 tháng" },
                    { 34, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Màn hình gaming cong 32 inch 4K 144Hz VA, HDR600, 1ms, G-Sync Compatible, DisplayHDR 600, USB Hub, Height Adjustable", null, "https://image-us.samsung.com/SamsungUS/home/computing/monitors/gaming/05222020/lc32g75tqsnxza-gallery-1.jpg", true, false, null, "Samsung Odyssey G7 32 inch 4K", 15990000m, 6, "Bảo hành chính hãng 12 tháng" },
                    { 35, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Màn hình gaming 27 inch 2K 240Hz IPS, 1ms GTG, G-Sync, ELMB Sync, HDR400, DisplayPort 1.4, ASUS Extreme Low Motion Blur", null, "https://dlcdnwebimgs.asus.com/gain/2BF2E66D-C5F9-4D6A-89FD-A5F7C8BC64B7/w1000/h732", true, false, null, "ASUS ROG Swift PG279QM 240Hz", 18990000m, 5, "Bảo hành chính hãng 12 tháng" },
                    { 36, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Màn hình văn phòng 27 inch 4K 60Hz IPS Black, Delta-E < 2, 100% sRGB, USB-C 90W, KVM built-in, Height/Pivot/Tilt/Swivel", null, "https://i.dell.com/is/image/DellContent/content/dam/ss2/product-images/dell-client-products/peripherals/monitors/u-series/u2723de/media-gallery/monitor-u2723de-gallery-1.psd?fmt=png-alpha&pscan=auto&scl=1&hei=402&wid=402&qlt=100,1&resMode=sharp2&size=402,402&chrss=full", true, false, null, "Dell U2723D UltraSharp 4K", 14990000m, 8, "Bảo hành chính hãng 12 tháng" },
                    { 37, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Màn hình gaming 27 inch 2K 144Hz IPS, HDRi tự động, loa 2.1 tích hợp, FreeSync Premium, USB-C, remote điều khiển", 8990000m, "https://video.benq.com/is/image/benqco/EX2780Q_main?$ResponsiveImage$", true, false, null, "BenQ EX2780Q 2K 144Hz IPS", 9990000m, 10, "Bảo hành chính hãng 12 tháng" },
                    { 38, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Màn hình gaming 28 inch 4K 155Hz IPS, G-Sync Ultimate, HDR400, 1ms GTG, VESA DisplayHDR 400, thiết kế gaming premium", null, "https://static.acer.com/up/Resource/Acer/Monitors/Predator/Predator-X28/Specifications/20211101/X28-hero-image.png", true, false, null, "Acer Predator X28 4K 155Hz", 22990000m, 4, "Bảo hành chính hãng 12 tháng" },
                    { 39, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Màn hình gaming 27 inch 2K 165Hz Rapid IPS Quantum Dot, 1ms, FreeSync Premium, 99% DCI-P3, USB Hub, Night Vision tắt tối", 7490000m, "https://asset.msi.com/resize/image/global/product/product_17006987161efe8b5e9df7e.png62405b38c58fe0f07fcef2367d8a9ba1/1024.png", true, false, null, "MSI MAG 274QRF-QD 2K 165Hz", 8490000m, 12, "Bảo hành chính hãng 12 tháng" },
                    { 40, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Màn hình OLED gaming siêu rộng 45 inch cong 800R, 2K 240Hz, 0.03ms, G-Sync Compatible, HDR True Black 400, DCI-P3 98.5%", 34990000m, "https://www.lg.com/us/images/monitors/md08003253/gallery/large01.jpg", true, false, null, "LG 45GR95QE OLED 45 inch 240Hz", 38990000m, 3, "Bảo hành chính hãng 12 tháng" },
                    { 41, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Màn hình gaming 27 inch 2K 240Hz Fast IPS, 1ms, KVM built-in, USB-C 18W, FreeSync Premium Pro, thiết kế không viền 3 cạnh", null, "https://static.gigabyte.com/StaticFile/Image/Global/8cbcc5bb5ad84de20e9af62c9faf5c6e/Product/30133/png/1000", true, false, null, "Gigabyte M27Q X 2K 240Hz", 7990000m, 14, "Bảo hành chính hãng 12 tháng" },
                    { 42, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Màn hình đồ họa chuyên nghiệp 27 inch 4K 60Hz IPS, Delta-E < 1, 100% sRGB/P3, Calman Verified, USB-C 96W, HDMI 2.1, DisplayPort 1.4", null, "https://dlcdnwebimgs.asus.com/gain/CB02B60E-D4C9-4E7F-BD21-A3FA55F6F7B6/w1000/h732", true, false, null, "ASUS ProArt PA279CRV 4K USB-C", 17990000m, 5, "Bảo hành chính hãng 12 tháng" },
                    { 43, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Card đồ họa gaming 16GB GDDR6X, DLSS 3.5 Frame Generation, Ray Tracing Ada, PCIe 4.0 x16, TDP 285W, 4x DisplayPort 1.4a + HDMI 2.1", null, "https://www.nvidia.com/content/dam/en-zz/Solutions/geforce/ada/rtx-4070-ti-super/geforce-rtx-4070-ti-super-product-photo-003-webp.webp", true, false, null, "NVIDIA GeForce RTX 4070 Ti Super", 24990000m, 5, "Bảo hành chính hãng 12 tháng" },
                    { 44, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CPU AMD flagship 16 nhân 32 luồng, 4.2GHz base 5.7GHz boost, 3D V-Cache 128MB L3, TDP 120W, socket AM5, dành cho gaming và workstation", 17990000m, "https://www.amd.com/system/files/2023-02/7950x3d-PIB-angle.png", true, false, null, "AMD Ryzen 9 7950X3D", 19990000m, 6, "Bảo hành chính hãng 12 tháng" },
                    { 45, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CPU Intel flagship 24 nhân (8P+16E) 36 luồng, 3.2GHz base 6.2GHz boost, TDP 150W, socket LGA1700, hỗ trợ DDR5 và DDR4", 15490000m, "https://www.intel.com/content/dam/www/public/us/en/images/products/hero/14th-gen-core-desktop-processor-hero-badge-rwd.png", true, false, null, "Intel Core i9-14900KS", 16990000m, 8, "Bảo hành chính hãng 12 tháng" },
                    { 46, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "RAM DDR5 4x16GB dual channel, 6000MHz CL36, XMP 3.0, Intel Extreme Memory Profile, tản nhiệt nhôm thấp, tương thích Intel và AMD", 5490000m, "https://www.corsair.com/medias/sys_master/images/images/h44/hce/9501839073310/CMK32GX5M2B6000C36-Gallery-CMK32GX5M2B6000C36-1.png", true, false, null, "Corsair Vengeance DDR5 64GB 6000MHz", 5990000m, 20, "Bảo hành chính hãng 12 tháng" },
                    { 47, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SSD NVMe PCIe 4.0 2TB, đọc 7450 MB/s ghi 6900 MB/s, cache 2GB LPDDR4, tản nhiệt heatshield, TBW 1200TB, bảo hành 5 năm", 3490000m, "https://image-us.samsung.com/SamsungUS/home/computing/memory-storage/ssd/07182022/MZ-V9P2T0B-galaxy-999.jpg", true, false, null, "Samsung 990 Pro 2TB NVMe PCIe 4.0", 3990000m, 20, "Bảo hành chính hãng 12 tháng" },
                    { 48, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mainboard AMD B650E ATX socket AM5, DDR5 7200MHz OC, PCIe 5.0 x16, WiFi 6E, Bluetooth 5.3, 4x M.2 PCIe 4.0, 2.5G LAN, USB4 Type-C", null, "https://dlcdnwebimgs.asus.com/gain/EBD3A3E1-7437-4F91-A5A2-6B1C7ADE7EC6/w1000/h732", true, false, null, "ASUS ROG Strix B650E-F Gaming WiFi", 8490000m, 6, "Bảo hành chính hãng 12 tháng" },
                    { 49, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Nguồn 1000W modular 80+ Gold, fan Zero RPM mode, ATX 3.0, PCIe 5.0 native, cổng side-connector độc đáo, bảo hành 10 năm", null, "https://www.corsair.com/medias/sys_master/images/images/hcf/hae/9476017471518/CP-9020253-NA-Gallery-RM1000x-SHIFT-01.png", true, false, null, "Corsair RM1000x Shift 80+ Gold", 4990000m, 10, "Bảo hành chính hãng 12 tháng" },
                    { 50, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tản nhiệt CPU dual tower cao cấp, 2 quạt 140mm NF-A15, TDP 250W+, socket 1700/AM5, chiều cao 165mm, zero noise ở tải thấp", null, "https://noctua.at/pub/media/catalog/product/cache/0d834e97c2d4c56a4cbf49b1dd48db85/n/h/nh-d15-chromax-black_1.jpg", true, false, null, "Noctua NH-D15 chromax.black", 1890000m, 15, "Bảo hành chính hãng 12 tháng" },
                    { 51, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Card đồ họa AMD RDNA3 24GB GDDR6, compute 61 TFLOPS, DisplayPort 2.1, HDMI 2.1, Radeon Super Resolution, FSR3, TDP 355W", 19990000m, "https://www.amd.com/system/files/2022-11/1013991-radeon-rx-7900-xt-xtx-product-photo.png", true, false, null, "AMD Radeon RX 7900 XTX 24GB", 21990000m, 4, "Bảo hành chính hãng 12 tháng" },
                    { 52, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "RAM DDR5 2x16GB 7200MHz CL34, XMP 3.0, tản nhiệt nhôm RGB cao cấp, hỗ trợ Intel 12th/13th/14th gen, kit được test kỹ càng", 2990000m, "https://www.gskill.com/img/product/memory/20230210120753-zoom.jpg", true, false, null, "G.SKILL Trident Z5 RGB DDR5 32GB", 3490000m, 22, "Bảo hành chính hãng 12 tháng" },
                    { 53, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tai nghe gaming không dây cao cấp driver 40mm neodymium, ANC chủ động AI, Hi-Res Audio 96kHz/24-bit, pin hot-swap 2 viên, đa nền tảng PC/PS", null, "https://media.steelseriescdn.com/thumbs/catalogue/products/01852-arctis-nova-pro-wireless/fba175af15594f5f9b51a37aac2e9e44/arctis-nova-pro-wireless-pdp-hero.png", true, false, null, "SteelSeries Arctis Nova Pro Wireless", 8990000m, 8, "Bảo hành chính hãng 12 tháng" },
                    { 54, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tai nghe chống ồn số 1 thế giới, 8 mic ANC, Hi-Res Audio 30Hz-40kHz, pin 30 giờ, Bluetooth 5.2 LDAC, multipoint 2 thiết bị, QN2e chip", 6490000m, "https://www.sony.com/image/5d02da5df552836db894cead8a68f5f3?fmt=png-alpha&wid=440", true, false, null, "Sony WH-1000XM5", 7490000m, 15, "Bảo hành chính hãng 12 tháng" },
                    { 55, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tai nghe gaming không dây kỷ lục pin 300 giờ, driver 50mm Dual Chamber riêng bass/mid-treble, âm thanh 7.1 ảo, mic cardioid tháo rời", null, "https://hyperx.com/cdn/shop/products/HyperX-Cloud-Alpha-Wireless-Gaming-Headset-Hero.png", true, false, null, "HyperX Cloud Alpha Wireless", 3290000m, 20, "Bảo hành chính hãng 12 tháng" },
                    { 56, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tai nghe gaming không dây esports, driver 50mm Triforce Titanium 3 đơn vị riêng, mic SuperCardioid, pin 70 giờ, THX Spatial Audio 360°", 3490000m, "https://assets2.razerzone.com/images/pnx.assets/4a17a8c73e8f0c69b2b3e6ddd9e0b853/razer-blackshark-v2-pro-2023-500x500.png", true, false, null, "Razer BlackShark V2 Pro 2023", 3990000m, 12, "Bảo hành chính hãng 12 tháng" },
                    { 57, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tai nghe true wireless ANC H2 chip, Transparency mode Adaptive, Personalized Spatial Audio, IP54, MagSafe, pin 6h + 24h case, USB-C", 5490000m, "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/MQTP3?wid=1144&hei=1144&fmt=jpeg&qlt=90&.v=1660803972000", true, false, null, "Apple AirPods Pro 2nd Gen", 5990000m, 20, "Bảo hành chính hãng 12 tháng" },
                    { 58, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tai nghe gaming không dây 47.4g siêu nhẹ, driver PRO-G 50mm graphene, Blue VO!CE mic AI, pin 50 giờ, LIGHTSPEED + Bluetooth", null, "https://resource.logitech.com/w_692,c_lpad,ar_4:3,q_auto,f_auto,dpr_1.0/d_transparent.gif/content/dam/gaming/en/products/g-pro-x-2/g-pro-x-2-gallery-1.png", true, false, null, "Logitech G PRO X 2 Lightspeed", 4490000m, 10, "Bảo hành chính hãng 12 tháng" },
                    { 59, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tai nghe studio open-back 250 Ohm, dải tần 5-35000Hz, driver Tesla, thiết kế velour earpads, không dây, dành cho mixing/mastering chuyên nghiệp", null, "https://www.beyerdynamic.com/media/catalog/product/cache/1/image/800x800/9df78eab33525d08d6e5fb8d27136e95/d/t/dt-990-pro-01.png", true, false, null, "Beyerdynamic DT 990 Pro 250 Ohm", 2890000m, 8, "Bảo hành chính hãng 12 tháng" },
                    { 60, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tai nghe true wireless ANC thông minh, driver 2-way coaxial, SSC Hi-Fi codec, IP57, pin 6h + 24h case, Galaxy AI real-time translation", 3490000m, "https://images.samsung.com/vn/smartphones/galaxy-buds3-pro/images/galaxy-buds3-pro-silver-detail-image-01.jpg", true, false, null, "Samsung Galaxy Buds3 Pro", 3990000m, 18, "Bảo hành chính hãng 12 tháng" },
                    { 61, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tai nghe gaming không dây, driver 50mm, ANC + Ambient Aware, JBL QuantumSURROUND, 2.4GHz + Bluetooth, pin 34 giờ ANC off", 2890000m, "https://www.jbl.com/dw/image/v2/AAUJ_PRD/on/demandware.static/-/Sites-masterCatalog_Harman/default/dw3b3c3b3d/JBL_QUANTUM910_WIRELESS_IMAGE_HERO_BLK.png", true, false, null, "JBL Quantum 910 Wireless", 3290000m, 12, "Bảo hành chính hãng 12 tháng" },
                    { 62, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tai nghe audiophile open-back, driver 38mm transducer, E.A.R. technology, dải tần 12-38500Hz, thiết kế ergonomic, đi kèm 2 cáp 3m và 1.2m", 1690000m, "https://assets.sennheiser.com/img/21630/x1_desktop_Sennheiser_HD599SE_03.jpg", true, false, null, "Sennheiser HD 599 SE", 1990000m, 10, "Bảo hành chính hãng 12 tháng" },
                    { 63, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SSD gaming NVMe PCIe 4.0 2TB, đọc 7300 MB/s ghi 6600 MB/s, Game Mode 2.0, tối ưu PS5, tản nhiệt heatsink tùy chọn, TBW 1200TB", 3490000m, "https://shop.westerndigital.com/content/dam/store/en-us/assets/products/internal-gaming-drives/wd-black-sn850x-nvme-ssd/gallery/wd-black-sn850x-nvme-ssd-1tb-hero.png.thumb.1280.1280.png", true, false, null, "WD Black SN850X 2TB NVMe PCIe 4.0", 3990000m, 20, "Bảo hành chính hãng 12 tháng" },
                    { 64, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ổ cứng HDD 3.5 inch 4TB, 5400 RPM, cache 256MB, SATA III 6Gb/s, phù hợp NAS và lưu trữ dữ liệu lớn, bảo hành 2 năm", null, "https://www.seagate.com/www-content/product-content/barracuda-fam/barracuda-new/images/barracuda-3-5-ssd-2tb-400x400.png", true, false, null, "Seagate Barracuda 4TB HDD 3.5 inch", 1990000m, 30, "Bảo hành chính hãng 12 tháng" },
                    { 65, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SSD PCIe 5.0 nhanh nhất thế giới 2TB, đọc 12400 MB/s ghi 11800 MB/s, heatsink tản nhiệt graphene, DirectStorage Xbox, bảo hành 5 năm", 4490000m, "https://www.crucial.com/content/dam/crucial/ssd-products/t700/images/in-box/crucial-t700-ssd-with-heatsink-in-box-image.psd.transform/medium-jpg/img.jpg", true, false, null, "Crucial T700 2TB PCIe 5.0 NVMe", 4990000m, 10, "Bảo hành chính hãng 12 tháng" },
                    { 66, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ổ cứng NAS chuyên dụng 20TB, 7200 RPM, cache 256MB, IronWolf Health Management, tối ưu cho NAS 24/7, bảo hành 5 năm + Rescue", null, "https://www.seagate.com/www-content/product-content/ironwolf-fam/ironwolf-pro/images/ironwolf-pro-14tb-400x400.png", true, false, null, "Seagate IronWolf Pro 20TB NAS", 9990000m, 8, "Bảo hành chính hãng 12 tháng" },
                    { 67, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SSD di động USB 3.2 Gen 2x2 tốc độ 2000 MB/s, 4TB, chống shock, IP65 bụi/nước, nhỏ gọn 88g, cáp USB-C + USB-A đi kèm", 2990000m, "https://image-us.samsung.com/SamsungUS/home/computing/memory-storage/portable-ssd/08032023/MU-PG4T0B-galaxy-999.jpg", true, false, null, "Samsung T9 4TB Portable SSD", 3490000m, 15, "Bảo hành chính hãng 12 tháng" },
                    { 68, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SSD NVMe PCIe 4.0 2TB enterprise-grade, đọc 7000 MB/s ghi 7000 MB/s, controller Phison E18, TBW 1.6PB, nhiệt độ hoạt động -40°C đến 85°C", null, "https://www.kingston.com/dataSheets/SKC3000D2048G_en.pdf", true, false, null, "Kingston KC3000 2TB PCIe 4.0", 3290000m, 18, "Bảo hành chính hãng 12 tháng" },
                    { 69, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ổ cứng HDD 2.5 inch 1TB cho laptop, 5400 RPM, cache 128MB, SATA III, chiều dày 7mm, phù hợp nâng cấp laptop đời cũ", 790000m, "https://shop.westerndigital.com/content/dam/store/en-us/assets/products/internal-pc-storage/wd-blue-mobile-hard-drive/gallery/wd-blue-mobile-hard-drive-500gb-2.5-7mm.png.thumb.1280.1280.png", true, false, null, "WD Blue 1TB 2.5 inch HDD", 890000m, 40, "Bảo hành chính hãng 12 tháng" },
                    { 70, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SSD NVMe PCIe 4.0 2TB heatsink tản nhiệt, đọc 7300 MB/s ghi 6800 MB/s, controller Phison E18, dung lượng cache 2GB DDR4", 2190000m, "https://www.silicon-power.com/web/images/product/SP_XD80_1.png", true, false, null, "Silicon Power XS70 2TB PCIe 4.0", 2490000m, 20, "Bảo hành chính hãng 12 tháng" },
                    { 71, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Thẻ nhớ CFexpress Type B 256GB tốc độ cao, đọc 1800 MB/s ghi 1700 MB/s, dành cho máy ảnh/quay phim 8K RAW, bền bỉ -25°C đến 85°C", 4490000m, "https://www.lexar.com/content/dam/lexar/products/cfexpress-cards/professional-1800x-cfexpress-type-b-card/gallery/lexar-cfexpress-type-b-256gb-1800x-product.png", true, false, null, "Lexar Professional 1800x CFexpress B", 4990000m, 8, "Bảo hành chính hãng 12 tháng" },
                    { 72, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SSD NVMe PCIe 4.0 4TB dung lượng lớn, đọc 7100 MB/s ghi 6600 MB/s, Phison E18, TBW 3000TB, bảo hành 5 năm, dành cho creative professional", 6990000m, "https://sabrent.com/cdn/shop/products/SB-RKT4P-4TB-main.jpg", true, false, null, "Sabrent Rocket 4 Plus 4TB NVMe", 7990000m, 5, "Bảo hành chính hãng 12 tháng" },
                    { 73, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ghế gaming cao cấp nhất, da Neo Hybrid Leatherette, tựa đầu 4D từ tính, tựa lưng điều chỉnh 165°, lumbar built-in, tay vịn 4D điều chỉnh", 10490000m, "https://secretlab.co/cdn/shop/files/TITAN-Evo-2022-SoftWeave-Plus-Charcoal-Blue-Hero_1800x.jpg", true, false, null, "Secretlab TITAN Evo 2022 XL", 11990000m, 8, "Bảo hành chính hãng 12 tháng" },
                    { 74, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ghế gaming ergonomic cao cấp nhất, PostureFit SL hỗ trợ cột sống, vải thoáng khí Rhythm, bảo hành 12 năm, tùy chỉnh hoàn toàn", null, "https://www.hermanmiller.com/content/dam/hermanmiller/images/products/seating/embody/embody-gaming-chair-3q-black-tr-gr-az.jpg", true, false, null, "Herman Miller Embody Gaming Chair", 42990000m, 3, "Bảo hành chính hãng 12 tháng" },
                    { 75, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ghế gaming phổ biến nhất esports, khung thép, đệm PU leather cao cấp, tựa lưng 90-135°, tay vịn 3D, gối đầu và lưng đi kèm", 5490000m, "https://www.dxracer.com/cdn/shop/files/Formula_Series_F08_Black_and_Red_001.jpg", true, false, null, "DXRacer Formula F08 Gaming Chair", 5990000m, 15, "Bảo hành chính hãng 12 tháng" },
                    { 76, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ghế gaming premium Đức, da vegan PU màng đôi, Memory Foam lumbar, tựa lưng 135°, tải 150kg, 4D armrest, thiết kế thể thao", 7990000m, "https://noblechairs.com/cdn/shop/products/hero-series-gaming-chair-black-white.jpg", true, false, null, "Noblechairs HERO Gaming Chair", 8990000m, 10, "Bảo hành chính hãng 12 tháng" },
                    { 77, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ghế gaming tải 180kg, vải Linen Cloud 5D, tựa lưng 165°, 4D armrest, tựa đầu và lưng Memory Foam, khung thép 2mm, bảo hành 2 năm", 5990000m, "https://www.andaseat.com/cdn/shop/products/kaiser-4-xl-gaming-chair.jpg", true, false, null, "AndaSeat Kaiser 4 XL", 6490000m, 12, "Bảo hành chính hãng 12 tháng" },
                    { 78, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ghế gaming Corsair leatherette cao cấp, tựa lưng 90-180° flat, Memory Foam tựa đầu và lưng, tay vịn 4D, tải 120kg, thiết kế racing", null, "https://www.corsair.com/medias/sys_master/images/images/ha9/h3b/9545432113182/CF-9010052-WW-Gallery-Corsair-T3-RUSH-Fabric-Gaming-Chair-Grey-Blue-01.png", true, false, null, "Corsair TC200 Leatherette Gaming Chair", 7990000m, 8, "Bảo hành chính hãng 12 tháng" },
                    { 79, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ghế gaming Razer ergonomic, tựa lưng lumbar built-in điều chỉnh, da vegan, tựa đầu memory foam, tay vịn 4D, tải 136kg, bảo hành 2 năm", 8990000m, "https://assets2.razerzone.com/images/pnx.assets/0a1b2c3d4e5f6a7b8c9d0e1f2a3b4c5d/razer-iskur-v2-500x500.png", true, false, null, "Razer Iskur V2", 9990000m, 6, "Bảo hành chính hãng 12 tháng" },
                    { 80, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ghế gaming giá tốt, khung thép chắc chắn, da PU bền, tựa lưng 150°, tay vịn 3D, bơm hơi điều chỉnh độ cao, phù hợp sinh viên", 2590000m, "https://cdn.cnbj1.fds.api.mi-img.com/b2c-shopapi-pms/pms_1694677900.11358949.png", true, false, null, "Xiaomi Gaming Chair", 2990000m, 20, "Bảo hành chính hãng 12 tháng" },
                    { 81, 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "iPhone cao cấp nhất Apple chip A18 Pro, màn hình Super Retina XDR 6.9 inch ProMotion 120Hz, camera 48MP Fusion + 12MP Tetra Prism 5x zoom", 32990000m, "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone-16-pro-finish-select-202409-6-9inch-deserttitanium?wid=5120&hei=2880&fmt=p-jpg&qlt=80&.v=1725415431361", true, false, null, "iPhone 16 Pro Max 256GB", 34990000m, 15, "Bảo hành chính hãng 12 tháng" },
                    { 82, 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "iPhone tầm trung Apple chip A16 Bionic, màn hình Super Retina XDR 6.1 inch, Dynamic Island, camera 48MP chính, USB-C, pin 20 giờ video", 18490000m, "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone-15-finish-select-202309-6-1inch-pink?wid=5120&hei=2880&fmt=p-jpg&qlt=80&.v=1692982705895", true, false, null, "iPhone 15 128GB", 19990000m, 20, "Bảo hành chính hãng 12 tháng" },
                    { 83, 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Flagship Android Snapdragon 8 Elite, màn hình Dynamic AMOLED 6.9 inch 120Hz, bút S Pen tích hợp, camera 200MP, Galaxy AI, RAM 12GB", 29990000m, "https://images.samsung.com/vn/smartphones/galaxy-s25-ultra/images/galaxy-s25-ultra-titanium-blue-1.jpg", true, false, null, "Samsung Galaxy S25 Ultra 512GB", 32990000m, 12, "Bảo hành chính hãng 12 tháng" },
                    { 84, 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Flagship Android Snapdragon 8 Elite, màn hình Dynamic AMOLED 6.7 inch 120Hz, camera 50MP, Galaxy AI translation real-time, IP68, RAM 12GB", 20990000m, "https://images.samsung.com/vn/smartphones/galaxy-s25-plus/images/galaxy-s25-plus-icyblue-1.jpg", true, false, null, "Samsung Galaxy S25+ 256GB", 22990000m, 15, "Bảo hành chính hãng 12 tháng" },
                    { 85, 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Flagship Google chip Tensor G4, màn hình LTPO OLED 6.8 inch 1-120Hz, camera 50MP + 48MP ultrawide + 48MP 5x zoom, Gemini AI tích hợp", null, "https://lh3.googleusercontent.com/nu1BpvkpFDh1KI7oqKjhFjwOyV4H7q0RoAU0KWtTM34WvHf1vXVHHjAJ8VYr6A7LVHjQ7nT3HGw=rw-w820", true, false, null, "Google Pixel 9 Pro XL 256GB", 24990000m, 8, "Bảo hành chính hãng 12 tháng" },
                    { 86, 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Flagship Android Snapdragon 8 Elite, màn hình AMOLED 6.82 inch 1-120Hz, camera Hasselblad 50MP, sạc nhanh 100W, sạc không dây 50W, RAM 16GB", 16990000m, "https://image01.oneplus.net/ebp/202412/17/1-m00-53-e9-rb8bwwygczuatooxaap5c3cukzw967.png", true, false, null, "OnePlus 13 512GB", 18990000m, 10, "Bảo hành chính hãng 12 tháng" },
                    { 87, 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Flagship Xiaomi Snapdragon 8 Elite, màn hình LTPO AMOLED 6.73 inch 1-120Hz, camera Leica 50MP, sạc 90W có dây + 50W không dây, RAM 16GB", 21990000m, "https://i02.appmifile.com/mi-com-product/fly-birds/xiaomi-15-pro/m/3b90b51c0c0869ac55618bc1c5b84e95.png", true, false, null, "Xiaomi 15 Pro 512GB", 23990000m, 8, "Bảo hành chính hãng 12 tháng" },
                    { 88, 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Flagship OPPO Dimensity 9400, màn hình AMOLED 6.78 inch 120Hz BOE, camera Hasselblad 50MP periscope 6x, sạc nhanh 80W + 50W wireless, RAM 16GB", 23990000m, "https://image.oppo.com/content/dam/oppo/common/mkt/v2en/find-x8-pro/find-x8-pro-kv.png", true, false, null, "OPPO Find X8 Pro 512GB", 25990000m, 6, "Bảo hành chính hãng 12 tháng" },
                    { 89, 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tầm trung Samsung Exynos 1480, màn hình Super AMOLED 6.6 inch FHD+ 120Hz, camera 50MP OIS, IP67, RAM 8GB, pin 5000mAh sạc 45W", 8490000m, "https://images.samsung.com/vn/smartphones/galaxy-a55-5g/images/galaxy-a55-5g-awesome-icyblue-1.jpg", true, false, null, "Samsung Galaxy A55 5G 256GB", 9490000m, 25, "Bảo hành chính hãng 12 tháng" },
                    { 90, 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "iPhone nhỏ gọn giá tốt Apple chip A15 Bionic, màn hình Retina 4.7 inch, camera 12MP, Touch ID Home button, 5G, pin 15 giờ video", 10990000m, "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone-se-finish-select-202203-6-1inch-midnight?wid=5120&hei=2880&fmt=p-jpg&qlt=80&.v=1646445666203", true, false, null, "iPhone SE 3rd Gen 64GB", 11990000m, 18, "Bảo hành chính hãng 12 tháng" },
                    { 91, 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Flagship Vivo Dimensity 9400, camera Zeiss 200MP periscope + 50MP ultrawide, màn hình AMOLED 6.78 inch 1-120Hz, sạc 90W, pin 6000mAh, RAM 16GB", 20490000m, "https://www.vivo.com/content/dam/vivo/en/products/x200-pro/blue.png", true, false, null, "Vivo X200 Pro 512GB", 22990000m, 7, "Bảo hành chính hãng 12 tháng" },
                    { 92, 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Thiết kế Glyph Interface LED độc đáo, Dimensity 7350 Pro, màn hình AMOLED 6.7 inch 120Hz, camera 50MP + 50MP ultrawide, RAM 12GB, IP54", 7990000m, "https://global-website-assets.nothing.tech/images/phone-2a-plus/phone-2a-plus-overview-01.jpg", true, false, null, "Nothing Phone 2a Plus 256GB", 8990000m, 12, "Bảo hành chính hãng 12 tháng" },
                    { 93, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bộ Office đầy đủ Word, Excel, PowerPoint, Outlook, OneNote, OneDrive 1TB, Teams Premium, cập nhật tự động, 1 người dùng đa thiết bị", 1390000m, "https://img-prod-cms-rt-microsoft-com.akamaized.net/cms/api/am/imageFileData/RE4OXeo?ver=6db3", true, false, null, "Microsoft 365 Personal 1 năm", 1590000m, 999, "Bảo hành chính hãng 12 tháng" },
                    { 94, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bộ Office cho gia đình 6 người dùng, Word/Excel/PowerPoint/Outlook, OneDrive 1TB/người, 60 phút Skype/tháng, đa thiết bị Win/Mac/iOS/Android", 2090000m, "https://img-prod-cms-rt-microsoft-com.akamaized.net/cms/api/am/imageFileData/RE4OXeo?ver=6db3", true, false, null, "Microsoft 365 Family 1 năm", 2390000m, 999, "Bảo hành chính hãng 12 tháng" },
                    { 95, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bản quyền Windows 11 Pro chính hãng Microsoft, kích hoạt trực tuyến vĩnh viễn, hỗ trợ nâng cấp miễn phí, dùng được cho 1 thiết bị, ngôn ngữ đa quốc gia", 990000m, "https://img-prod-cms-rt-microsoft-com.akamaized.net/cms/api/am/imageFileData/RWEHjV?ver=b5e6", true, false, null, "Windows 11 Pro OEM Key", 1290000m, 999, "Bảo hành chính hãng 12 tháng" },
                    { 96, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Toàn bộ ứng dụng Adobe: Photoshop, Illustrator, Premiere Pro, After Effects, Lightroom, Acrobat Pro, 100GB cloud storage, cập nhật liên tục", 10990000m, "https://www.adobe.com/content/dam/cc/icons/Creative_Cloud.svg", true, false, null, "Adobe Creative Cloud All Apps 1 năm", 12990000m, 999, "Bảo hành chính hãng 12 tháng" },
                    { 97, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Đăng ký ChatGPT Plus GPT-4o không giới hạn, DALL-E 3 tạo ảnh AI, GPT-4 phân tích file, web browsing real-time, Custom GPTs, ưu tiên tốc độ", null, "https://upload.wikimedia.org/wikipedia/commons/thumb/0/04/ChatGPT_logo.svg/1024px-ChatGPT_logo.svg.png", true, false, null, "ChatGPT Plus 1 tháng", 520000m, 999, "Bảo hành chính hãng 12 tháng" },
                    { 98, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gói ChatGPT Plus 1 năm tiết kiệm 15%, GPT-4o + DALL-E 3 + Advanced Data Analysis + Browsing, Custom GPTs marketplace, ưu tiên tốc độ 24/7", 4990000m, "https://upload.wikimedia.org/wikipedia/commons/thumb/0/04/ChatGPT_logo.svg/1024px-ChatGPT_logo.svg.png", true, false, null, "ChatGPT Plus 12 tháng", 5290000m, 999, "Bảo hành chính hãng 12 tháng" },
                    { 99, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Đăng ký Claude Pro Anthropic, truy cập Claude 3.5 Sonnet/Opus không giới hạn, Projects tổ chức hội thoại, độ dài context 200K token, ưu tiên tốc độ", null, "https://upload.wikimedia.org/wikipedia/commons/thumb/8/8a/Claude_AI_logo.svg/1200px-Claude_AI_logo.svg.png", true, false, null, "Claude Pro 1 tháng", 520000m, 999, "Bảo hành chính hãng 12 tháng" },
                    { 100, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tạo ảnh AI không giới hạn Midjourney v6.1, 15 giờ Fast GPU/tháng, Relax mode unlimited, Stealth mode ẩn ảnh, Remix + Vary Region chỉnh sửa", 350000m, "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e6/Midjourney_Emblem.png/800px-Midjourney_Emblem.png", true, false, null, "Midjourney Standard Plan 1 tháng", 390000m, 999, "Bảo hành chính hãng 12 tháng" },
                    { 101, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Thiết kế chuyên nghiệp Canva Pro 1 năm, 100M+ template, 75M+ stock ảnh/video, Background Remover, Magic Resize, Brand Kit, 1TB storage", 990000m, "https://upload.wikimedia.org/wikipedia/commons/thumb/b/bb/Canva_Logo.svg/1200px-Canva_Logo.svg.png", true, false, null, "Canva Pro 1 năm", 1290000m, 999, "Bảo hành chính hãng 12 tháng" },
                    { 102, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Workspace all-in-one Notion Plus + AI không giới hạn, AI viết/tóm tắt/dịch/phân tích, 100GB file storage, unlimited guest, custom domain", 1390000m, "https://upload.wikimedia.org/wikipedia/commons/4/45/Notion_app_logo.png", true, false, null, "Notion AI Plus 1 năm", 1590000m, 999, "Bảo hành chính hãng 12 tháng" },
                    { 103, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kiểm tra ngữ pháp tiếng Anh AI nâng cao, full-sentence rewrites, tone detector, plagiarism checker, tích hợp 500k+ app, phân tích writing analytics", 2490000m, "https://upload.wikimedia.org/wikipedia/commons/thumb/6/6e/Grammarly_logo.svg/2560px-Grammarly_logo.svg.png", true, false, null, "Grammarly Business 1 năm", 2990000m, 999, "Bảo hành chính hãng 12 tháng" },
                    { 104, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "VPN bảo mật hàng đầu thế giới 6 thiết bị/cùng lúc, 6400+ server 111 quốc gia, Threat Protection chặn malware, Meshnet kết nối thiết bị riêng", 990000m, "https://upload.wikimedia.org/wikipedia/commons/thumb/5/52/NordVPN_logo_monochromatic.svg/1280px-NordVPN_logo_monochromatic.svg.png", true, false, null, "NordVPN 2 năm", 1390000m, 999, "Bảo hành chính hãng 12 tháng" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CommissionLogs_UserId",
                table: "CommissionLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecifications_ProductId",
                table: "ProductSpecifications",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantOptions_ProductVariantGroupId",
                table: "ProductVariantOptions",
                column: "ProductVariantGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantValues_ProductVariantId",
                table: "ProductVariantValues",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantValues_ProductVariantOptionId",
                table: "ProductVariantValues",
                column: "ProductVariantOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreInventories_ProductId",
                table: "StoreInventories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyPackages_ProductId",
                table: "WarrantyPackages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_ProductId",
                table: "Wishlists",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_UserId",
                table: "Wishlists",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CommissionLogs");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductSpecifications");

            migrationBuilder.DropTable(
                name: "ProductVariantValues");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "ServiceTickets");

            migrationBuilder.DropTable(
                name: "StoreInventories");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "WarrantyPackages");

            migrationBuilder.DropTable(
                name: "Wishlists");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ProductVariantOptions");

            migrationBuilder.DropTable(
                name: "ProductVariants");

            migrationBuilder.DropTable(
                name: "StoreBranches");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ProductVariantGroups");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
