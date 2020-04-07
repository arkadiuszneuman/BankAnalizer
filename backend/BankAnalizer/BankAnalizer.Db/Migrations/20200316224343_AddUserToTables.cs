using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankAnalizer.Db.Migrations
{
    public partial class AddUserToTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_BankTransactionTypes_Name",
                table: "BankTransactionTypes");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Rules",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Groups",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BankTransactionTypes",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "BankTransactionTypes",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "BankTransactions",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Rules_UserId",
                table: "Rules",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_UserId",
                table: "Groups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactionTypes_Name",
                table: "BankTransactionTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactionTypes_UserId",
                table: "BankTransactionTypes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_UserId",
                table: "BankTransactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_Users_UserId",
                table: "BankTransactions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactionTypes_Users_UserId",
                table: "BankTransactionTypes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_UserId",
                table: "Groups",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Users_UserId",
                table: "Rules",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_Users_UserId",
                table: "BankTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactionTypes_Users_UserId",
                table: "BankTransactionTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_UserId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Users_UserId",
                table: "Rules");

            migrationBuilder.DropIndex(
                name: "IX_Rules_UserId",
                table: "Rules");

            migrationBuilder.DropIndex(
                name: "IX_Groups_UserId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_BankTransactionTypes_Name",
                table: "BankTransactionTypes");

            migrationBuilder.DropIndex(
                name: "IX_BankTransactionTypes_UserId",
                table: "BankTransactionTypes");

            migrationBuilder.DropIndex(
                name: "IX_BankTransactions_UserId",
                table: "BankTransactions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BankTransactionTypes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BankTransactions");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BankTransactionTypes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_BankTransactionTypes_Name",
                table: "BankTransactionTypes",
                column: "Name");
        }
    }
}
