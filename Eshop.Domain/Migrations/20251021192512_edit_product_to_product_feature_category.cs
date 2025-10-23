using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class edit_product_to_product_feature_category : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeatureCategory_ProductFeatureCategory_ProductFeatureCategoryId",
                table: "ProductFeatureCategory");

            migrationBuilder.DropIndex(
                name: "IX_ProductFeatureCategory_ProductFeatureCategoryId",
                table: "ProductFeatureCategory");

            migrationBuilder.DropColumn(
                name: "ProductFeatureCategoryId",
                table: "ProductFeatureCategory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductFeatureCategoryId",
                table: "ProductFeatureCategory",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureCategory_ProductFeatureCategoryId",
                table: "ProductFeatureCategory",
                column: "ProductFeatureCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeatureCategory_ProductFeatureCategory_ProductFeatureCategoryId",
                table: "ProductFeatureCategory",
                column: "ProductFeatureCategoryId",
                principalTable: "ProductFeatureCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
