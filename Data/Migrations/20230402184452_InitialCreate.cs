using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeSheets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameEmployee = table.Column<string>(type: "TEXT", nullable: true),
                    Scope = table.Column<string>(type: "TEXT", nullable: true),
                    WorkHours = table.Column<double>(type: "REAL", precision: 4, scale: 2, nullable: true),
                    DateOfWorks = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    DateLastEdit = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSheets", x => x.Id);
                });
            migrationBuilder.Sql(
                @"INSERT INTO TimeSheets (Id, NameEmployee, Scope, WorkHours, DateOfWorks, Comment, DateLastEdit)" +
                "VALUES" +
                "(1, 'Mark', 'Clear teritory, Washing some dress', '3', '1995-06-01','some commet', '2023-03-23')," +
                "(2, 'Tomha', 'Do autotesting', '8', '1996-06-01','some commet 2', '2023-03-24')," +
                "(3, 'Nik', 'Play to Atomic hearth', '2', '1997-06-01','some commet 3', '2023-03-25')," +
                "(4, 'Tomha', 'Drink soda and eat ramen', '1', '1998-06-01','i wanna pitca', '2023-03-26')," +
                "(5, 'Buch', 'Sleep and snore', '9', '1999-06-01','some commet', '2023-03-27')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeSheets");
        }
    }
}
