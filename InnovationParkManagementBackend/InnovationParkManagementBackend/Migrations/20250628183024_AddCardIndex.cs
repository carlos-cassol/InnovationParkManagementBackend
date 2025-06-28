﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovationParkManagementBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddCardIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "Cards");
        }
    }
}
