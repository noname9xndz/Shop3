using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop3.Data.EF.Migrations
{
    public partial class addReOrderMessageToTableBill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishProducts_AspNetUsers_CustomerId",
                table: "WishProducts");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "WishProducts",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReOrderMesssage",
                table: "Bills",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WishProducts_AspNetUsers_CustomerId",
                table: "WishProducts",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishProducts_AspNetUsers_CustomerId",
                table: "WishProducts");

            migrationBuilder.DropColumn(
                name: "ReOrderMesssage",
                table: "Bills");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "WishProducts",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_WishProducts_AspNetUsers_CustomerId",
                table: "WishProducts",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
