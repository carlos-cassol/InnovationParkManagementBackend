using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovationParkManagementBackend.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIncubationPlanToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardFiles_Cards_CardId1",
                table: "CardFiles");

            migrationBuilder.DropIndex(
                name: "IX_CardFiles_CardId1",
                table: "CardFiles");

            migrationBuilder.DropColumn(
                name: "CardId1",
                table: "CardFiles");

            migrationBuilder.AlterColumn<string>(
                name: "IncubationPlan",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IncubationPlan",
                table: "Cards",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "CardId1",
                table: "CardFiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardFiles_CardId1",
                table: "CardFiles",
                column: "CardId1",
                unique: true,
                filter: "[CardId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CardFiles_Cards_CardId1",
                table: "CardFiles",
                column: "CardId1",
                principalTable: "Cards",
                principalColumn: "Id");
        }
    }
}
