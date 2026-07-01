using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourPlanner.Dal.Migrations
{
    /// <inheritdoc />
    public partial class AddRouteInformationToToursTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RouteInformation",
                table: "Tours",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<double>(
                name: "TotalTime",
                table: "TourLogs",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "TotalDistance",
                table: "TourLogs",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RouteInformation",
                table: "Tours");

            migrationBuilder.AlterColumn<int>(
                name: "TotalTime",
                table: "TourLogs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "TotalDistance",
                table: "TourLogs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}
