using Microsoft.EntityFrameworkCore.Migrations;

namespace Auto_Part_WebUI.Migrations
{
    public partial class userorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId1",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "Orders",
                newName: "ECoPartUserId1");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Orders",
                newName: "ECoPartUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserId1",
                table: "Orders",
                newName: "IX_Orders_ECoPartUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_ECoPartUserId1",
                table: "Orders",
                column: "ECoPartUserId1",
                principalSchema: "Membership",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_ECoPartUserId1",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ECoPartUserId1",
                table: "Orders",
                newName: "UserId1");

            migrationBuilder.RenameColumn(
                name: "ECoPartUserId",
                table: "Orders",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ECoPartUserId1",
                table: "Orders",
                newName: "IX_Orders_UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId1",
                table: "Orders",
                column: "UserId1",
                principalSchema: "Membership",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
