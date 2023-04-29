using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeScopeToSeparateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Scope",
                table: "TimeSheets");

            migrationBuilder.AddColumn<int>(
                name: "ScopeId",
                table: "TimeSheets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_TimeSheets_ScopeId",
                table: "TimeSheets",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_Scopes_CurrencyId",
                table: "Scopes",
                column: "CurrencyId");

            migrationBuilder.Sql(
                @"INSERT INTO Currencies (Id, ShortName, FullName, DollarExchangeRate)" +
                "VALUES" +
                "(643, 'RUB', 'Rouble', 0.012)," +
                "(826, 'GBP', 'Pound sterling', 1.24)," +
                "(840, 'USD', 'Dollar', 1)," +
                "(978, 'EUR', 'Euro', 1.11)");

            migrationBuilder.Sql(
                @"INSERT INTO Scopes (Id, Name, Rate, CurrencyId)" +
                "VALUES" +
                "(1, 'Clear teritory, Washing some dress', 230, 643)," +
                "(2, 'Do autotesting', 120, 826)," +
                "(3, 'Play to Atomic hearth', 300, 840)," +
                "(4, 'Play to Atomic hearth', 350, 840)," +
                "(5, 'Sleep and snore', 1200, 978)");

            migrationBuilder.Sql(@"UPDATE TimeSheets SET ScopeId = 1 WHERE Id = 2");
            migrationBuilder.Sql(@"UPDATE TimeSheets SET ScopeId = 2 WHERE Id = 1");
            migrationBuilder.Sql(@"UPDATE TimeSheets SET ScopeId = 5 WHERE Id = 3");
            migrationBuilder.Sql(@"UPDATE TimeSheets SET ScopeId = 4 WHERE Id = 5");
            migrationBuilder.Sql(@"UPDATE TimeSheets SET ScopeId = 3 WHERE Id = 4");

            migrationBuilder.Sql(
                @"INSERT INTO TimeSheets (Id, NameEmployee, ScopeId, WorkHours, DateOfWorks, Comment, DateLastEdit)" +
                "VALUES" +
                "(6, 'Alex', '3', '7', '2022-06-07','some commet', '2023-04-21')," +
                "(7, 'Aram', '3', '15', '2022-06-06','some commet', '2023-04-22')," +
                "(8, 'Gennady', '3', '10', '2022-06-05','some commet', '2023-04-23')," +
                "(9, 'Van', '4', '4', '2022-06-04','some commet', '2023-04-24')," +
                "(10, 'Billy', '5', '12', '2022-06-03','some commet', '2023-04-25')," +
                "(11, 'Hanibal', '4', '5', '2022-06-02','some commet', '2023-04-26')");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSheets_Scopes_ScopeId",
                table: "TimeSheets",
                column: "ScopeId",
                principalTable: "Scopes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSheets_Scopes_ScopeId",
                table: "TimeSheets");

            migrationBuilder.DropTable(
                name: "Scopes");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_TimeSheets_ScopeId",
                table: "TimeSheets");

            migrationBuilder.DropColumn(
                name: "ScopeId",
                table: "TimeSheets");

            migrationBuilder.AddColumn<string>(
                name: "Scope",
                table: "TimeSheets",
                type: "TEXT",
                nullable: true);
        }
    }
}
