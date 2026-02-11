using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rindo.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "chats",
                schema: "dbo",
                columns: table => new
                {
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chats", x => x.chat_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "dbo",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "chat_messages",
                schema: "dbo",
                columns: table => new
                {
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sender_id = table.Column<Guid>(type: "uuid", nullable: false),
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chat_messages", x => x.message_id);
                    table.ForeignKey(
                        name: "fk_chat_messages_chats_chat_id",
                        column: x => x.chat_id,
                        principalSchema: "dbo",
                        principalTable: "chats",
                        principalColumn: "chat_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                schema: "dbo",
                columns: table => new
                {
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_by = table.Column<Guid>(type: "uuid", nullable: false),
                    deadline_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_projects", x => x.project_id);
                    table.ForeignKey(
                        name: "fk_projects_chats_chat_id",
                        column: x => x.chat_id,
                        principalSchema: "dbo",
                        principalTable: "chats",
                        principalColumn: "chat_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invitations",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sender_id = table.Column<Guid>(type: "uuid", nullable: false),
                    recipient_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invitations", x => x.id);
                    table.ForeignKey(
                        name: "fk_invitations_projects_project_id",
                        column: x => x.project_id,
                        principalSchema: "dbo",
                        principalTable: "projects",
                        principalColumn: "project_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_invitations_users_recipient_id",
                        column: x => x.recipient_id,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "projects_to_users",
                schema: "dbo",
                columns: table => new
                {
                    projects_project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    users_user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_projects_to_users", x => new { x.projects_project_id, x.users_user_id });
                    table.ForeignKey(
                        name: "fk_projects_to_users_projects_projects_project_id",
                        column: x => x.projects_project_id,
                        principalSchema: "dbo",
                        principalTable: "projects",
                        principalColumn: "project_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_projects_to_users_users_users_user_id",
                        column: x => x.users_user_id,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "dbo",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    color = table.Column<string>(type: "text", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bit_role_rights = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.role_id);
                    table.ForeignKey(
                        name: "fk_roles_projects_project_id",
                        column: x => x.project_id,
                        principalSchema: "dbo",
                        principalTable: "projects",
                        principalColumn: "project_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stages",
                schema: "dbo",
                columns: table => new
                {
                    stage_id = table.Column<Guid>(type: "uuid", nullable: false),
                    custom_name = table.Column<string>(type: "text", nullable: true),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    index = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stages", x => x.stage_id);
                    table.ForeignKey(
                        name: "fk_stages_projects_project_id",
                        column: x => x.project_id,
                        principalSchema: "dbo",
                        principalTable: "projects",
                        principalColumn: "project_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "roles_to_users",
                schema: "dbo",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    users_user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles_to_users", x => new { x.role_id, x.users_user_id });
                    table.ForeignKey(
                        name: "fk_roles_to_users_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "dbo",
                        principalTable: "roles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_roles_to_users_users_users_user_id",
                        column: x => x.users_user_id,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "project_tasks",
                schema: "dbo",
                columns: table => new
                {
                    task_id = table.Column<Guid>(type: "uuid", nullable: false),
                    task_number = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    index = table.Column<int>(type: "integer", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    stage_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assignee_id = table.Column<Guid>(type: "uuid", nullable: true),
                    reporter_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_by = table.Column<Guid>(type: "uuid", nullable: false),
                    modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    deadline_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_tasks", x => x.task_id);
                    table.ForeignKey(
                        name: "fk_project_tasks_projects_project_id",
                        column: x => x.project_id,
                        principalSchema: "dbo",
                        principalTable: "projects",
                        principalColumn: "project_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_project_tasks_stages_stage_id",
                        column: x => x.stage_id,
                        principalSchema: "dbo",
                        principalTable: "stages",
                        principalColumn: "stage_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_comments",
                schema: "dbo",
                columns: table => new
                {
                    comment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    task_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task_comments", x => x.comment_id);
                    table.ForeignKey(
                        name: "fk_task_comments_project_tasks_task_id",
                        column: x => x.task_id,
                        principalSchema: "dbo",
                        principalTable: "project_tasks",
                        principalColumn: "task_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_chat_messages_chat_id",
                schema: "dbo",
                table: "chat_messages",
                column: "chat_id");

            migrationBuilder.CreateIndex(
                name: "ix_invitations_project_id",
                schema: "dbo",
                table: "invitations",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_invitations_recipient_id",
                schema: "dbo",
                table: "invitations",
                column: "recipient_id");

            migrationBuilder.CreateIndex(
                name: "ix_project_tasks_project_id",
                schema: "dbo",
                table: "project_tasks",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_project_tasks_stage_id",
                schema: "dbo",
                table: "project_tasks",
                column: "stage_id");

            migrationBuilder.CreateIndex(
                name: "ix_projects_chat_id",
                schema: "dbo",
                table: "projects",
                column: "chat_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_projects_to_users_users_user_id",
                schema: "dbo",
                table: "projects_to_users",
                column: "users_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_roles_project_id",
                schema: "dbo",
                table: "roles",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_roles_to_users_users_user_id",
                schema: "dbo",
                table: "roles_to_users",
                column: "users_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_stages_project_id",
                schema: "dbo",
                table: "stages",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_task_comments_task_id",
                schema: "dbo",
                table: "task_comments",
                column: "task_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat_messages",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "invitations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "projects_to_users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "roles_to_users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "task_comments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "project_tasks",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "stages",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "projects",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "chats",
                schema: "dbo");
        }
    }
}
