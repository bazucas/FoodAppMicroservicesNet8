using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tasko.Services.CouponApi.Migrations;

/// <inheritdoc />
public partial class AddCoupons : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.InsertData(
            table: "Coupons",
            columns: new[] { "CouponId", "CouponCode", "DiscountAmount", "MinAmount" },
            values: new object[,]
            {
                { 1, "10OFF", 10.0, 20 },
                { 2, "20OFF", 20.0, 40 }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DeleteData(
            table: "Coupons",
            keyColumn: "CouponId",
            keyValue: 1);

        _ = migrationBuilder.DeleteData(
            table: "Coupons",
            keyColumn: "CouponId",
            keyValue: 2);
    }
}
