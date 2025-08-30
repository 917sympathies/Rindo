using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rindo.API.Migrations
{
    /// <inheritdoc />
    public partial class schemes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_Tasks_TaskId",
                table: "TaskComments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Stages_StageId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "ProjectUser");

            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "FinishDate",
                table: "Projects");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "TaskComments",
                newName: "TaskComments",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tags",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Stages",
                newName: "Stages",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Roles",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Projects",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Invitations",
                newName: "Invitations",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Chats",
                newName: "Chats",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ChatMessages",
                newName: "ChatMessages",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "ProjectTasks",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "ReporterUserId",
                schema: "dbo",
                table: "ProjectTasks",
                newName: "ReporterId");

            migrationBuilder.RenameColumn(
                name: "Progress",
                schema: "dbo",
                table: "ProjectTasks",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "FinishDate",
                schema: "dbo",
                table: "ProjectTasks",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "AsigneeUserId",
                schema: "dbo",
                table: "ProjectTasks",
                newName: "AssigneeId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_StageId",
                schema: "dbo",
                table: "ProjectTasks",
                newName: "IX_ProjectTasks_StageId");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "dbo",
                table: "Stages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectTasks",
                schema: "dbo",
                table: "ProjectTasks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Projects2Users",
                schema: "dbo",
                columns: table => new
                {
                    ProjectsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects2Users", x => new { x.ProjectsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_Projects2Users_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalSchema: "dbo",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects2Users_Users_UsersId",
                        column: x => x.UsersId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles2Users",
                schema: "dbo",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles2Users", x => new { x.RoleId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_Roles2Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Roles2Users_Users_UsersId",
                        column: x => x.UsersId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects2Users_UsersId",
                schema: "dbo",
                table: "Projects2Users",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles2Users_UsersId",
                schema: "dbo",
                table: "Roles2Users",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Stages_StageId",
                schema: "dbo",
                table: "ProjectTasks",
                column: "StageId",
                principalSchema: "dbo",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_ProjectTasks_TaskId",
                schema: "dbo",
                table: "TaskComments",
                column: "TaskId",
                principalSchema: "dbo",
                principalTable: "ProjectTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Stages_StageId",
                schema: "dbo",
                table: "ProjectTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_ProjectTasks_TaskId",
                schema: "dbo",
                table: "TaskComments");

            migrationBuilder.DropTable(
                name: "Projects2Users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Roles2Users",
                schema: "dbo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectTasks",
                schema: "dbo",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "dbo",
                table: "Stages");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "dbo",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "TaskComments",
                schema: "dbo",
                newName: "TaskComments");

            migrationBuilder.RenameTable(
                name: "Tags",
                schema: "dbo",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "Stages",
                schema: "dbo",
                newName: "Stages");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "dbo",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "Projects",
                schema: "dbo",
                newName: "Projects");

            migrationBuilder.RenameTable(
                name: "Invitations",
                schema: "dbo",
                newName: "Invitations");

            migrationBuilder.RenameTable(
                name: "Chats",
                schema: "dbo",
                newName: "Chats");

            migrationBuilder.RenameTable(
                name: "ChatMessages",
                schema: "dbo",
                newName: "ChatMessages");

            migrationBuilder.RenameTable(
                name: "ProjectTasks",
                schema: "dbo",
                newName: "Tasks");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Tasks",
                newName: "FinishDate");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Tasks",
                newName: "Progress");

            migrationBuilder.RenameColumn(
                name: "ReporterId",
                table: "Tasks",
                newName: "ReporterUserId");

            migrationBuilder.RenameColumn(
                name: "AssigneeId",
                table: "Tasks",
                newName: "AsigneeUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTasks_StageId",
                table: "Tasks",
                newName: "IX_Tasks_StageId");

            migrationBuilder.AddColumn<DateOnly>(
                name: "FinishDate",
                table: "Projects",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProjectUser",
                columns: table => new
                {
                    ProjectsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUser", x => new { x.ProjectsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ProjectUser_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RoleId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUser_UsersId",
                table: "ProjectUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UsersId",
                table: "RoleUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_Tasks_TaskId",
                table: "TaskComments",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Stages_StageId",
                table: "Tasks",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
