using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cyclone.Services.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class Mag02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DiscountAmount",
                table: "Coupons",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinAmount",
                table: "Coupons",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "CouponId", "CouponCode", "DiscountAmount", "MinAmount" },
                values: new object[,]
                {
                    { new Guid("89b050e0-e95f-493b-9c6a-f0ee9907354a"), "TSR2535", 10.0, 20.0 },
                    { new Guid("c82a3c68-a37e-42ff-8a61-7cfa7b2070bf"), "DFQ6721", 15.0, 25.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: new Guid("89b050e0-e95f-493b-9c6a-f0ee9907354a"));

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: new Guid("c82a3c68-a37e-42ff-8a61-7cfa7b2070bf"));

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "MinAmount",
                table: "Coupons");
        }
    }
}
