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
                    Name = table.Column<string>(type: "TEXT", nullable: true),
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
            migrationBuilder.Sql(@"INSERT INTO TimeSheets (Id, Name, Scope, WorkHours, DateOfWorks, Comment, DateLastEdit)" +
                "VALUES" +
                "(1, 'Cleaning', 'Clear teritory, washing some dress', '3', '1995-06-01 00:00:00','some commet', '2023-03-23 00:00:00')," +
                "(2, 'Working', 'Do autotesting', '8', '1996-06-01 00:00:00','some commet 2', '2023-03-24 00:01:00')," +
                "(3, 'Plaing game', 'Play to Atomic hearth', '2', '1997-06-01 00:00:00','some commet 3', '2023-03-25 00:02:00')," +
                "(4, 'Eating', 'Drink soda and eat ramen', '1', '1998-06-01 00:00:00','i wanna pitca', '2023-03-26 00:03:00')," +
                "(5, 'Sleeping', 'Sleep and snore', '9', '1999-06-01 00:00:00','some commet', '2023-03-27 00:00:00')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeSheets");
        }
    }
}
