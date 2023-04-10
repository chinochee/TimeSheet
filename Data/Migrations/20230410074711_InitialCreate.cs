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
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShortName = table.Column<string>(type: "TEXT", nullable: true),
                    FullName = table.Column<string>(type: "TEXT", nullable: true),
                    DollarExchangeRate = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scopes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Rate = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrencyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scopes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scopes_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeSheets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameEmployee = table.Column<string>(type: "TEXT", nullable: true),
                    ScopeId = table.Column<int>(type: "INTEGER", nullable: false),
                    WorkHours = table.Column<double>(type: "REAL", precision: 4, scale: 2, nullable: true),
                    DateOfWorks = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    DateLastEdit = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSheets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeSheets_Scopes_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "Scopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scopes_CurrencyId",
                table: "Scopes",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSheets_ScopeId",
                table: "TimeSheets",
                column: "ScopeId");

            migrationBuilder.Sql(
                @"INSERT INTO Currencies (Id, ShortName, FullName, DollarExchangeRate)" +
                "VALUES" +
                "(643, 'RUB', 'Rouble', 81)," +
                "(826, 'GBP', 'Pound sterling', 101)," +
                "(840, 'USD', 'Dollar', 1)," +
                "(978, 'EUR', 'Euro', 89)");

            migrationBuilder.Sql(
                @"INSERT INTO Scopes (Id, Name, Rate, CurrencyId)" +
                "VALUES" +
                "(1, 'Clear teritory, Washing some dress', 230, 643)," +
                "(2, 'Do autotesting', 120, 826)," +
                "(3, 'Play to Atomic hearth', 350, 840)," +
                "(4, 'Sleep and snore', 1200, 978)");

            migrationBuilder.Sql(
                @"INSERT INTO TimeSheets (Id, NameEmployee, ScopeId, WorkHours, DateOfWorks, Comment, DateLastEdit)" +
                "VALUES" +
                "(1, 'Mark', '1', '3', '1995-06-01','some commet', '2023-03-23')," +
                "(2, 'Tomha', '2', '8', '1996-06-01','some commet 2', '2023-03-24')," +
                "(3, 'Nik', '3', '2', '1997-06-01','some commet 3', '2023-03-25')," +
                "(4, 'Tomha', '4', '1', '1998-06-01','i wanna pitca', '2023-03-26')," +
                "(5, 'Buch', '4', '9', '1999-06-01','some commet', '2023-03-27')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeSheets");

            migrationBuilder.DropTable(
                name: "Scopes");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
