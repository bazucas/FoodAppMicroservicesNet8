﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tasko.Services.CouponApi.Migrations;

/// <inheritdoc />
public partial class AddCouponToDb : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.CreateTable(
            name: "Coupons",
            columns: table => new
            {
                CouponId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CouponCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                DiscountAmount = table.Column<double>(type: "float", nullable: false),
                MinAmount = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_Coupons", x => x.CouponId);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropTable(
            name: "Coupons");
    }
}
