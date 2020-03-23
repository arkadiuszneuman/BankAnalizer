using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PkoAnalizer.Db.Migrations
{
    public partial class AddUsersConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersConnections",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserRequestingConnectionId = table.Column<Guid>(nullable: false),
                    UserRequestedToConnectId = table.Column<Guid>(nullable: false),
                    IsRequestApproved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersConnections", x => x.Id);
                    table.UniqueConstraint("AK_UsersConnections_UserRequestedToConnectId_UserRequestingConnectionId", x => new { x.UserRequestedToConnectId, x.UserRequestingConnectionId });
                    table.ForeignKey(
                        name: "FK_UsersConnections_Users_UserRequestedToConnectId",
                        column: x => x.UserRequestedToConnectId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UsersConnections_Users_UserRequestingConnectionId",
                        column: x => x.UserRequestingConnectionId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersConnections_UserRequestedToConnectId",
                table: "UsersConnections",
                column: "UserRequestedToConnectId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersConnections_UserRequestingConnectionId",
                table: "UsersConnections",
                column: "UserRequestingConnectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersConnections");
        }
    }
}
