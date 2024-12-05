using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryDemo.Data.Migrations.Library
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Domain");

            migrationBuilder.CreateTable(
                name: "Books",
                schema: "Domain",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    publisher = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isbn = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    page_count = table.Column<int>(type: "int", nullable: false),
                    pub_date = table.Column<DateOnly>(type: "date", nullable: false),
                    img_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "BookRentals",
                schema: "Domain",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    book_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    due_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookRentals", x => x.id);
                    table.ForeignKey(
                        name: "FK_BookRentals_Books_book_id",
                        column: x => x.book_id,
                        principalSchema: "Domain",
                        principalTable: "Books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookRentals_Users_user_id",
                        column: x => x.user_id,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookReviews",
                schema: "Domain",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    book_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookReviews", x => x.id);
                    table.ForeignKey(
                        name: "FK_BookReviews_Books_book_id",
                        column: x => x.book_id,
                        principalSchema: "Domain",
                        principalTable: "Books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookReviews_Users_user_id",
                        column: x => x.user_id,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookRentals_book_id",
                schema: "Domain",
                table: "BookRentals",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_BookRentals_user_id",
                schema: "Domain",
                table: "BookRentals",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_BookReviews_book_id",
                schema: "Domain",
                table: "BookReviews",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_BookReviews_user_id",
                schema: "Domain",
                table: "BookReviews",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookRentals",
                schema: "Domain");

            migrationBuilder.DropTable(
                name: "BookReviews",
                schema: "Domain");

            migrationBuilder.DropTable(
                name: "Books",
                schema: "Domain");
        }
    }
}
