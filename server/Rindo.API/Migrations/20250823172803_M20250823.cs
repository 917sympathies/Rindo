using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rindo.API.Migrations
{
    /// <inheritdoc />
    public partial class M20250823 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Users_UserId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_UserId",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "CanAddRoles",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanAddStage",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanAddTask",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanCompleteTask",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanDeleteStage",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanDeleteTask",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanExcludeUser",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanInviteUser",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanModifyRoles",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanModifyStage",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanModifyTask",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanUseChat",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "SenderUsername",
                table: "Invitations");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Tasks",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "ResponsibleUserId",
                table: "Tasks",
                newName: "AsigneeUserId");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Projects",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Invitations",
                newName: "SenderId");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ReporterUserId",
                table: "Tasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "BitRoleRights",
                table: "Roles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "RecipientId",
                table: "Invitations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_RecipientId",
                table: "Invitations",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Users_RecipientId",
                table: "Invitations",
                column: "RecipientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Users_RecipientId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_RecipientId",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ReporterUserId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "BitRoleRights",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "Invitations");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Tasks",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "AsigneeUserId",
                table: "Tasks",
                newName: "ResponsibleUserId");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Projects",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Invitations",
                newName: "UserId");

            migrationBuilder.AddColumn<bool>(
                name: "CanAddRoles",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanAddStage",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanAddTask",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanCompleteTask",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanDeleteStage",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanDeleteTask",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanExcludeUser",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanInviteUser",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanModifyRoles",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanModifyStage",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanModifyTask",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanUseChat",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "Invitations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderUsername",
                table: "Invitations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_UserId",
                table: "Invitations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Users_UserId",
                table: "Invitations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
