using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tek.Gomoku.Service.Migrations
{
    public partial class ChangeGameTableSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WhiteSideUserName",
                table: "Game",
                newName: "WhiteSidePlayer");

            migrationBuilder.RenameColumn(
                name: "BlackSideUserName",
                table: "Game",
                newName: "BlackSidePlayer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WhiteSidePlayer",
                table: "Game",
                newName: "WhiteSideUserName");

            migrationBuilder.RenameColumn(
                name: "BlackSidePlayer",
                table: "Game",
                newName: "BlackSideUserName");
        }
    }
}
