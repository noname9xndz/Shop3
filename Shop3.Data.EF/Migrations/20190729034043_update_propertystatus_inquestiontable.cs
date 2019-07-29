using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop3.Data.EF.Migrations
{
    public partial class update_propertystatus_inquestiontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Questions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Questions");
        }
    }
}
