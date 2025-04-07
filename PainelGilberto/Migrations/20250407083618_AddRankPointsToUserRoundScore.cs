using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PainelGilberto.Migrations
{
    /// <inheritdoc />
    public partial class AddRankPointsToUserRoundScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RankingScore",
                table: "UserRoundScores",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RankingScore",
                table: "UserRoundScores");
        }
    }
}
