using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoleId",
                table: "AspNetUsers",
                column: "RoleId");

            migrationBuilder.Sql(
                @"INSERT INTO Roles (Id, Name)" +
                "VALUES" +
                "(1, 'Employee')," +
                "(2, 'HR')," +
                "(3, 'Manager')," +
                "(4, 'Admin')");

            migrationBuilder.Sql(@"UPDATE AspNetUsers SET RoleId = 1 WHERE Id = 2");
            migrationBuilder.Sql(@"UPDATE AspNetUsers SET RoleId = 2 WHERE Id = 1");
            migrationBuilder.Sql(@"UPDATE AspNetUsers SET RoleId = 3 WHERE Id = 3");
            migrationBuilder.Sql(@"UPDATE AspNetUsers SET RoleId = 4 WHERE Id = 5");
            migrationBuilder.Sql(@"UPDATE AspNetUsers SET RoleId = 4 WHERE Id = 4");
            migrationBuilder.Sql(@"UPDATE AspNetUsers SET RoleId = 3 WHERE Id = 6");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Roles_RoleId",
                table: "AspNetUsers",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Roles_RoleId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoleId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "AspNetUsers");
        }
    }
}
