using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tek.Gomoku.Service.Migrations
{
    public partial class ColorInString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "GameMove");

            migrationBuilder.AddColumn<string>(
                name: "ColorInString",
                table: "GameMove",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorInString",
                table: "GameMove");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeStamp",
                table: "GameMove",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
