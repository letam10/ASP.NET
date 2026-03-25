using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thi_Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductVariantSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorHex",
                table: "ProductVariantOptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProductVariantOptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "ProductVariantOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "ProductVariantGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProductVariantGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "ProductVariantGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProductVariantSelections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductVariantOptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantSelections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariantSelections_ProductVariantOptions_ProductVariantOptionId",
                        column: x => x.ProductVariantOptionId,
                        principalTable: "ProductVariantOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductVariantSelections_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://dlcdnwebimgs.asus.com/gain/378C75D6-8210-4DA7-AAE9-84B48458B085");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantGroups_CategoryId",
                table: "ProductVariantGroups",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantSelections_ProductId",
                table: "ProductVariantSelections",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantSelections_ProductVariantOptionId",
                table: "ProductVariantSelections",
                column: "ProductVariantOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantGroups_Categories_CategoryId",
                table: "ProductVariantGroups",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantGroups_Categories_CategoryId",
                table: "ProductVariantGroups");

            migrationBuilder.DropTable(
                name: "ProductVariantSelections");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariantGroups_CategoryId",
                table: "ProductVariantGroups");

            migrationBuilder.DropColumn(
                name: "ColorHex",
                table: "ProductVariantOptions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProductVariantOptions");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "ProductVariantOptions");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ProductVariantGroups");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProductVariantGroups");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "ProductVariantGroups");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://dlcdnwebimgs.asus.com/gain/9BCCE5B8-9B6F-4857-B1B9-04FF37DAD6E1/w1000/h732");
        }
    }
}
