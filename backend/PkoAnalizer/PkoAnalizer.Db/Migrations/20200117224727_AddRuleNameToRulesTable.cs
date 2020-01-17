using Microsoft.EntityFrameworkCore.Migrations;

namespace PkoAnalizer.Db.Migrations
{
    public partial class AddRuleNameToRulesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RuleName",
                table: "Rules",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RuleName",
                table: "Rules");
        }
    }
}
