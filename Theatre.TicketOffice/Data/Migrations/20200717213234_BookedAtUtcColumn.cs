using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Theatre.TicketOffice.Data.Migrations
{
    public partial class BookedAtUtcColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BookedAtUtc",
                table: "Bookings",
                nullable: false,
                defaultValueSql: "getutcdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookedAtUtc",
                table: "Bookings");
        }
    }
}
