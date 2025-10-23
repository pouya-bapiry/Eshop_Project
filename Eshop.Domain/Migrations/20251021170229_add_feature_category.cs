using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class add_feature_category : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductFeatureCategoryId",
                table: "ProductFeatures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ProductFeatureCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeatureCategoryTitle = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ProductFeatureCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFeatureCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductFeatureCategory_ProductFeatureCategory_ProductFeatureCategoryId",
                        column: x => x.ProductFeatureCategoryId,
                        principalTable: "ProductFeatureCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatures_ProductFeatureCategoryId",
                table: "ProductFeatures",
                column: "ProductFeatureCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureCategory_ProductFeatureCategoryId",
                table: "ProductFeatureCategory",
                column: "ProductFeatureCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeatures_ProductFeatureCategory_ProductFeatureCategoryId",
                table: "ProductFeatures",
                column: "ProductFeatureCategoryId",
                principalTable: "ProductFeatureCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeatures_ProductFeatureCategory_ProductFeatureCategoryId",
                table: "ProductFeatures");

            migrationBuilder.DropTable(
                name: "ProductFeatureCategory");

            migrationBuilder.DropIndex(
                name: "IX_ProductFeatures_ProductFeatureCategoryId",
                table: "ProductFeatures");

            migrationBuilder.DropColumn(
                name: "ProductFeatureCategoryId",
                table: "ProductFeatures");
        }
    }
}
