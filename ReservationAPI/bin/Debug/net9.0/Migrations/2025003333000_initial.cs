using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReservationAPI.Migrations;
public partial class InitialMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Reservation",
            columns: table => new
            {
                Id = table.Column<string>(nullable: false),
                ClientName = table.Column<string>(maxLength: 256, nullable: false),
                Date = table.Column<DateOnly>(nullable: false),
                Hour = table.Column<string>(nullable: false),
                Service = table.Column<string>(nullable: false)
            });

        migrationBuilder.CreateTable(
            name: "Service",
            columns: table => new
            {
                Id = table.Column<string>(nullable: false),
                Name = table.Column<string>(maxLength: 256, nullable: true)
            });
        migrationBuilder.InsertData(table: "Service", columns: new string[] { "Id", "Name" }, values: new string[] { "1", "Corte" });
        migrationBuilder.InsertData(table: "Service", columns: new string[] { "Id", "Name" }, values: new string[] { "2", "Barba" });
        migrationBuilder.InsertData(table: "Service", columns: new string[] { "Id", "Name" }, values: new string[] { "3", "Corte y Barba" });
        migrationBuilder.InsertData(table: "Service", columns: new string[] { "Id", "Name" }, values: new string[] { "4", "Manicura" });
        migrationBuilder.InsertData(table: "Service", columns: new string[] { "Id", "Name" }, values: new string[] { "5", "Teñido" });
        migrationBuilder.InsertData(table: "Service", columns: new string[] { "Id", "Name" }, values: new string[] { "6", "Rasurado" });
    }
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Service");

        migrationBuilder.DropTable(
            name: "Reservation");
    }
}
                