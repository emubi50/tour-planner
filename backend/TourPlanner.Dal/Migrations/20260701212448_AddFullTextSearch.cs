using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace TourPlanner.Dal.Migrations
{
    /// <inheritdoc />
    public partial class AddFullTextSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER TABLE ""Tours""
ALTER COLUMN ""TransportType"" TYPE character varying(20)
USING (
    CASE ""TransportType""
        WHEN 0 THEN 'BIKE'
        WHEN 1 THEN 'WALK'
        WHEN 2 THEN 'CAR'
        WHEN 3 THEN 'PUBLIC'
        ELSE ""TransportType""::text
    END
);
");

            migrationBuilder.AddColumn<string>(
                name: "ChildFriendliness",
                table: "Tours",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Popularity",
                table: "Tours",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Tours",
                type: "tsvector",
                nullable: false);

            migrationBuilder.Sql(@"
CREATE INDEX ""IX_Tours_SearchVector""
ON ""Tours"" USING GIN (""SearchVector"");
");

            migrationBuilder.Sql(@"
CREATE OR REPLACE FUNCTION update_tour_computed_fields(p_tour_id integer)
RETURNS void AS $$
DECLARE
    v_log_count           integer;
    v_avg_difficulty      numeric;
    v_avg_total_time      numeric;
    v_avg_total_distance  numeric;
    v_popularity          text;
    v_child_friendliness  text;
    v_log_comments        text;
    v_tour                record;
BEGIN
    SELECT count(*),
           avg(""Difficulty""),
           avg(""TotalTime""),
           avg(""TotalDistance"")
      INTO v_log_count, v_avg_difficulty, v_avg_total_time, v_avg_total_distance
      FROM ""TourLogs""
     WHERE ""TourId"" = p_tour_id;

    v_popularity := CASE
        WHEN v_log_count >= 8 THEN 'High'
        WHEN v_log_count >= 3 THEN 'Medium'
        ELSE 'Low'
    END;

    IF v_log_count = 0 THEN
        v_child_friendliness := 'Moderate';
    ELSE
        v_child_friendliness := CASE
            WHEN v_avg_difficulty <= 2 AND v_avg_total_time <= 120 AND v_avg_total_distance <= 10
                THEN 'ChildFriendly'
            WHEN v_avg_difficulty <= 3 AND v_avg_total_time <= 240 AND v_avg_total_distance <= 20
                THEN 'Moderate'
            ELSE 'NotChildFriendly'
        END;
    END IF;

    SELECT string_agg(""Comment"", ' ')
      INTO v_log_comments
      FROM ""TourLogs""
     WHERE ""TourId"" = p_tour_id;

    SELECT * INTO v_tour FROM ""Tours"" WHERE ""Id"" = p_tour_id;

    UPDATE ""Tours""
       SET ""Popularity""        = v_popularity,
           ""ChildFriendliness"" = v_child_friendliness,
           ""SearchVector"" =
               setweight(to_tsvector('english', coalesce(v_tour.""Name"", '')), 'A') ||
               setweight(to_tsvector('english', coalesce(v_tour.""Description"", '')), 'B') ||
               setweight(to_tsvector('english', coalesce(v_tour.""From"", '') || ' ' || coalesce(v_tour.""To"", '')), 'B') ||
               setweight(to_tsvector('english', coalesce(v_tour.""TransportType"", '')), 'C') ||
               setweight(to_tsvector('english', v_popularity || ' popularity'), 'C') ||
               setweight(to_tsvector('english', v_child_friendliness || ' child friendly'), 'C') ||
               setweight(to_tsvector('english', coalesce(v_log_comments, '')), 'D')
     WHERE ""Id"" = p_tour_id;
END;
$$ LANGUAGE plpgsql;
");

            migrationBuilder.Sql(@"
CREATE OR REPLACE FUNCTION trg_tour_logs_changed()
RETURNS trigger AS $$
BEGIN
    IF TG_OP = 'DELETE' THEN
        PERFORM update_tour_computed_fields(OLD.""TourId"");
        RETURN OLD;
    ELSE
        PERFORM update_tour_computed_fields(NEW.""TourId"");
        IF TG_OP = 'UPDATE' AND OLD.""TourId"" <> NEW.""TourId"" THEN
            PERFORM update_tour_computed_fields(OLD.""TourId"");
        END IF;
        RETURN NEW;
    END IF;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER tour_logs_after_change
AFTER INSERT OR UPDATE OR DELETE ON ""TourLogs""
FOR EACH ROW EXECUTE FUNCTION trg_tour_logs_changed();
");

            migrationBuilder.Sql(@"
CREATE OR REPLACE FUNCTION trg_tours_text_changed()
RETURNS trigger AS $$
BEGIN
    PERFORM update_tour_computed_fields(NEW.""Id"");
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER tours_after_insert_update
AFTER INSERT OR UPDATE OF ""Name"", ""Description"", ""From"", ""To"", ""TransportType""
ON ""Tours""
FOR EACH ROW EXECUTE FUNCTION trg_tours_text_changed();
");

            migrationBuilder.Sql(@"
DO $$
DECLARE r record;
BEGIN
    FOR r IN SELECT ""Id"" FROM ""Tours"" LOOP
        PERFORM update_tour_computed_fields(r.""Id"");
    END LOOP;
END $$;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS tours_after_insert_update ON ""Tours"";");
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS tour_logs_after_change ON ""TourLogs"";");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS trg_tours_text_changed();");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS trg_tour_logs_changed();");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS update_tour_computed_fields(integer);");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_Tours_SearchVector"";");

            migrationBuilder.DropColumn(
                name: "ChildFriendliness",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Popularity",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "Tours");

            migrationBuilder.Sql(@"
ALTER TABLE ""Tours""
ALTER COLUMN ""TransportType"" TYPE integer
USING (
    CASE ""TransportType""
        WHEN 'BIKE' THEN 0
        WHEN 'WALK' THEN 1
        WHEN 'CAR' THEN 2
        WHEN 'PUBLIC' THEN 3
        ELSE 0
    END
);
");
        }
    }
}