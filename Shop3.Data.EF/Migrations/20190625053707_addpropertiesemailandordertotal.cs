using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop3.Data.EF.Migrations
{
    public partial class addpropertiesemailandordertotal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerEmail",
                table: "Bills",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OrderTotal",
                table: "Bills",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerEmail",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "OrderTotal",
                table: "Bills");
        }
    }
}
