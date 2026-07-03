using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourPlanner.Dal.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnedLocationToTour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "To",
                table: "Tours",
                newName: "To_Label");

            migrationBuilder.RenameColumn(
                name: "From",
                table: "Tours",
                newName: "From_Label");

            migrationBuilder.AddColumn<double>(
                name: "From_Latitude",
                table: "Tours",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "From_Longitude",
                table: "Tours",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "To_Latitude",
                table: "Tours",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "To_Longitude",
                table: "Tours",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "From_Latitude",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "From_Longitude",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "To_Latitude",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "To_Longitude",
                table: "Tours");

            migrationBuilder.RenameColumn(
                name: "To_Label",
                table: "Tours",
                newName: "To");

            migrationBuilder.RenameColumn(
                name: "From_Label",
                table: "Tours",
                newName: "From");
        }
    }
}
