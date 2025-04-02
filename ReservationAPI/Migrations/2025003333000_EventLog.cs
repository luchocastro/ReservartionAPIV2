using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReservationAPI.Migrations;
public partial class EventLogMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "EventLog",
            columns: table => new
            {
                EventId = table.Column<string>(maxLength: 256, nullable: false),
                Log = table.Column<string>(maxLength: 256, nullable: false)
            });

    }
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "EventLog");
    }
}
                