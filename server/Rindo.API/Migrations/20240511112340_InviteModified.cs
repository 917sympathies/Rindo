using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rindo.API.Migrations
{
    /// <inheritdoc />
    public partial class InviteModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "SenderUsername",
                table: "Invitations");
        }
    }
}
