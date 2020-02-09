using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PkoAnalizer.Db.Migrations
{
    public partial class AddRuleToGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RuleId",
                table: "Groups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_RuleId",
                table: "Groups",
                column: "RuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Rules_RuleId",
                table: "Groups",
                column: "RuleId",
                principalTable: "Rules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Rules_RuleId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_RuleId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "RuleId",
                table: "Groups");
        }
    }
}
