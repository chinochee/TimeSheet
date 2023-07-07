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

            migrationBuilder.Sql(
                @"INSERT INTO Roles (Id, Name)" +
                "VALUES" +
                "(1, 'Employee')," +
                "(2, 'HR')," +
                "(3, 'Manager')," +
                "(4, 'Admin')");

            migrationBuilder.CreateTable(
                name: "EmployeeRole",
                columns: table => new
                {
                    EmployeeListId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoleListId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRole", x => new { x.EmployeeListId, x.RoleListId });
                    table.ForeignKey(
                        name: "FK_EmployeeRole_AspNetUsers_EmployeeListId",
                        column: x => x.EmployeeListId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeRole_Roles_RoleListId",
                        column: x => x.RoleListId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(
                @"INSERT INTO EmployeeRole (EmployeeListId, RoleListId)" +
                "VALUES" +
                "(1, 1)," +
                "(2, 1)," +
                "(3, 1)," +
                "(4, 2)," +
                "(5, 1)," +
                "(5, 3)," +
                "(6, 1)," +
                "(6, 3)," +
                "(6, 4)");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRole_RoleListId",
                table: "EmployeeRole",
                column: "RoleListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeRole");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
