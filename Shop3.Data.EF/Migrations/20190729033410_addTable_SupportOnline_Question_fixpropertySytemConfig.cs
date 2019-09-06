using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Shop3.Data.EF.Migrations
{
    public partial class addTable_SupportOnline_Question_fixpropertySytemConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value5",
                table: "SystemConfigs",
                newName: "DecimalValue");

            migrationBuilder.RenameColumn(
                name: "Value4",
                table: "SystemConfigs",
                newName: "DateTimeValue");

            migrationBuilder.RenameColumn(
                name: "Value3",
                table: "SystemConfigs",
                newName: "BoolValue");

            migrationBuilder.RenameColumn(
                name: "Value2",
                table: "SystemConfigs",
                newName: "IntValue");

            migrationBuilder.RenameColumn(
                name: "Value1",
                table: "SystemConfigs",
                newName: "StringValue");

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 500, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupportOnlines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    Skype = table.Column<string>(maxLength: 128, nullable: true),
                    FaceBook = table.Column<string>(maxLength: 128, nullable: true),
                    Yahoo = table.Column<string>(maxLength: 128, nullable: true),
                    Pinterest = table.Column<string>(maxLength: 128, nullable: true),
                    Twitter = table.Column<string>(nullable: true),
                    Google = table.Column<string>(maxLength: 128, nullable: true),
                    Mobile = table.Column<string>(maxLength: 128, nullable: true),
                    Email = table.Column<string>(maxLength: 128, nullable: true),
                    Instagram = table.Column<string>(maxLength: 128, nullable: true),
                    Youtube = table.Column<string>(maxLength: 128, nullable: true),
                    Linkedin = table.Column<string>(maxLength: 128, nullable: true),
                    Zalo = table.Column<string>(maxLength: 128, nullable: true),
                    TimeOpenWindow = table.Column<string>(maxLength: 128, nullable: true),
                    Other = table.Column<string>(maxLength: 500, nullable: true),
                    DisplayOrder = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportOnlines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VisitorStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IPAddress = table.Column<string>(maxLength: 128, nullable: false),
                    VisitedDate = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorStatistics", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "SupportOnlines");

            migrationBuilder.DropTable(
                name: "VisitorStatistics");

            migrationBuilder.RenameColumn(
                name: "StringValue",
                table: "SystemConfigs",
                newName: "Value1");

            migrationBuilder.RenameColumn(
                name: "IntValue",
                table: "SystemConfigs",
                newName: "Value2");

            migrationBuilder.RenameColumn(
                name: "DecimalValue",
                table: "SystemConfigs",
                newName: "Value5");

            migrationBuilder.RenameColumn(
                name: "DateTimeValue",
                table: "SystemConfigs",
                newName: "Value4");

            migrationBuilder.RenameColumn(
                name: "BoolValue",
                table: "SystemConfigs",
                newName: "Value3");
        }
    }
}
