using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PkoAnalizer.Db.Migrations
{
    public partial class AddGroupsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.UniqueConstraint("AK_Groups_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "BankTransactionGroups",
                columns: table => new
                {
                    BankTransactionId = table.Column<Guid>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransactionGroups", x => new { x.BankTransactionId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_BankTransactionGroups_BankTransactions_BankTransactionId",
                        column: x => x.BankTransactionId,
                        principalTable: "BankTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankTransactionGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactionGroups_GroupId",
                table: "BankTransactionGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
               name: "IX_BankTransactionGroups_BankTransactionId",
               table: "BankTransactionGroups",
               column: "BankTransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankTransactionGroups");

            migrationBuilder.DropTable(
                name: "Groups");
        }
    }
}
