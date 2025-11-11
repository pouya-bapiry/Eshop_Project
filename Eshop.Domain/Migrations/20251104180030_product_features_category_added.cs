using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class product_features_category_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductFeaturesCategoryId",
                table: "ProductFeatures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ProductFeaturesCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeatureCategoryTitle = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFeaturesCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatures_ProductFeaturesCategoryId",
                table: "ProductFeatures",
                column: "ProductFeaturesCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeatures_ProductFeaturesCategory_ProductFeaturesCategoryId",
                table: "ProductFeatures",
                column: "ProductFeaturesCategoryId",
                principalTable: "ProductFeaturesCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeatures_ProductFeaturesCategory_ProductFeaturesCategoryId",
                table: "ProductFeatures");

            migrationBuilder.DropTable(
                name: "ProductFeaturesCategory");

            migrationBuilder.DropIndex(
                name: "IX_ProductFeatures_ProductFeaturesCategoryId",
                table: "ProductFeatures");

            migrationBuilder.DropColumn(
                name: "ProductFeaturesCategoryId",
                table: "ProductFeatures");
        }
    }
}
