using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PkoAnalizer.Db.Migrations
{
    public partial class CreateBankTransactionAndType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankTransactionTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransactionTypes", x => x.Id);
                    table.UniqueConstraint("AK_BankTransactionTypes_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "BankTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    BankTransactionTypeId = table.Column<Guid>(nullable: true),
                    OperationDate = table.Column<DateTime>(type: "Date", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "Date", nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Currency = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Extensions = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankTransactions_BankTransactionTypes_BankTransactionTypeId",
                        column: x => x.BankTransactionTypeId,
                        principalTable: "BankTransactionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_BankTransactionTypeId",
                table: "BankTransactions",
                column: "BankTransactionTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankTransactions");

            migrationBuilder.DropTable(
                name: "BankTransactionTypes");
        }
    }
}
