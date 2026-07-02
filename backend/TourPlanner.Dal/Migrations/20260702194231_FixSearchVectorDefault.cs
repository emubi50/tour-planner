using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourPlanner.Dal.Migrations
{
    /// <inheritdoc />
    public partial class FixSearchVectorDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER TABLE ""Tours""
ALTER COLUMN ""SearchVector"" SET DEFAULT ''::tsvector;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER TABLE ""Tours""
ALTER COLUMN ""SearchVector"" DROP DEFAULT;
");
        }
    }
}
