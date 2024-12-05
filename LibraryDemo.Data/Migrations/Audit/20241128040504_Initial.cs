using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryDemo.Data.Migrations.Audit
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Audit");

            migrationBuilder.CreateTable(
                name: "SaveChangesAudits",
                schema: "Audit",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    start_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    end_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    succeeded = table.Column<bool>(type: "bit", nullable: false),
                    error_message = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveChangesAudits", x => x.id);
                    table.ForeignKey(
                        name: "FK_SaveChangesAudits_Users_user_id",
                        column: x => x.user_id,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityAudit",
                schema: "Audit",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    entity_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    state = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    save_changes_audit_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityAudit", x => x.id);
                    table.ForeignKey(
                        name: "FK_EntityAudit_SaveChangesAudits_save_changes_audit_id",
                        column: x => x.save_changes_audit_id,
                        principalSchema: "Audit",
                        principalTable: "SaveChangesAudits",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityAudit_save_changes_audit_id",
                schema: "Audit",
                table: "EntityAudit",
                column: "save_changes_audit_id");

            migrationBuilder.CreateIndex(
                name: "IX_SaveChangesAudits_user_id",
                schema: "Audit",
                table: "SaveChangesAudits",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityAudit",
                schema: "Audit");

            migrationBuilder.DropTable(
                name: "SaveChangesAudits",
                schema: "Audit");
        }
    }
}
