using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigrationToFixIssues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AtListId",
                table: "Assignments",
                newName: "AssignmentListId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignmentListId",
                table: "Assignments",
                column: "AssignmentListId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_UserId",
                table: "Assignments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentList_UserId",
                table: "AssignmentList",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentList_User_UserId",
                table: "AssignmentList",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AssignmentList_AssignmentListId",
                table: "Assignments",
                column: "AssignmentListId",
                principalTable: "AssignmentList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_User_UserId",
                table: "Assignments",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentList_User_UserId",
                table: "AssignmentList");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AssignmentList_AssignmentListId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_User_UserId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_AssignmentListId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_UserId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentList_UserId",
                table: "AssignmentList");

            migrationBuilder.RenameColumn(
                name: "AssignmentListId",
                table: "Assignments",
                newName: "AtListId");
        }
    }
}
