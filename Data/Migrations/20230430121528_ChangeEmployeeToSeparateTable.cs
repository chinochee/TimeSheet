using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEmployeeToSeparateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameEmployee",
                table: "TimeSheets");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "TimeSheets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeSheets_EmployeeId",
                table: "TimeSheets",
                column: "EmployeeId");
            
            migrationBuilder.Sql(
                @"INSERT INTO Employees (Id, Name)" +
                "VALUES" +
                "(1, 'Alex')," +
                "(2, 'Van')," +
                "(3, 'Billy')," +
                "(4, 'Hanibal')," +
                "(5, 'Gennady')," +
                "(6, 'Aram')");

            migrationBuilder.Sql(@"UPDATE TimeSheets SET EmployeeId = 1 WHERE Id = 1");
            migrationBuilder.Sql(@"UPDATE TimeSheets SET EmployeeId = 2 WHERE Id = 2");
            migrationBuilder.Sql(@"UPDATE TimeSheets SET EmployeeId = 3 WHERE Id = 3");
            migrationBuilder.Sql(@"UPDATE TimeSheets SET EmployeeId = 4 WHERE Id = 4");
            migrationBuilder.Sql(@"UPDATE TimeSheets SET EmployeeId = 5 WHERE Id = 5");
            migrationBuilder.Sql(@"UPDATE TimeSheets SET EmployeeId = 2 WHERE Id = 6");
            migrationBuilder.Sql(@"UPDATE TimeSheets SET EmployeeId = 2 WHERE Id = 7");
            migrationBuilder.Sql(@"UPDATE TimeSheets SET EmployeeId = 3 WHERE Id = 8");
            migrationBuilder.Sql(@"UPDATE TimeSheets SET EmployeeId = 1 WHERE Id = 9");
            migrationBuilder.Sql(@"UPDATE TimeSheets SET EmployeeId = 6 WHERE Id = 10");
            migrationBuilder.Sql(@"UPDATE TimeSheets SET EmployeeId = 5 WHERE Id = 11");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSheets_Employees_EmployeeId",
                table: "TimeSheets",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSheets_Employees_EmployeeId",
                table: "TimeSheets");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_TimeSheets_EmployeeId",
                table: "TimeSheets");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "TimeSheets");

            migrationBuilder.AddColumn<string>(
                name: "NameEmployee",
                table: "TimeSheets",
                type: "TEXT",
                nullable: true);
        }
    }
}
