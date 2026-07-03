using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourPlanner.Dal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFixSearchVectorWithLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR REPLACE FUNCTION update_tour_computed_fields(p_tour_id integer)
RETURNS void AS $$
DECLARE
    v_log_count integer;
    v_avg_difficulty numeric;
    v_avg_total_time numeric;
    v_avg_total_distance numeric;
    v_popularity text;
    v_child_friendliness text;
    v_log_comments text;
    v_tour record;
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
    SET ""Popularity"" = v_popularity,
        ""ChildFriendliness"" = v_child_friendliness,
        ""SearchVector"" =
            setweight(to_tsvector('english', coalesce(""Name"", '')), 'A') ||
            setweight(to_tsvector('english', coalesce(""Description"", '')), 'B') ||
            setweight(to_tsvector('english',
                coalesce(""From_Label"", '') || ' ' ||
                coalesce(""To_Label"", '')
            ), 'B') ||
            setweight(to_tsvector('english', coalesce(""TransportType"", '')), 'C') ||
            setweight(to_tsvector('english', v_popularity || ' popularity'), 'C') ||
            setweight(to_tsvector('english', v_child_friendliness || ' child friendly'), 'C') ||
            setweight(to_tsvector('english', coalesce(v_log_comments, '')), 'D')
    WHERE ""Id"" = p_tour_id;
END;
$$ LANGUAGE plpgsql;
");

            migrationBuilder.Sql(@"
DROP TRIGGER IF EXISTS tours_after_insert_update ON ""Tours"";

CREATE TRIGGER tours_after_insert_update
AFTER INSERT OR UPDATE OF ""Name"", ""Description"", ""From_Label"", ""From_Latitude"", ""From_Longitude"",
                            ""To_Label"", ""To_Latitude"", ""To_Longitude"", ""TransportType""
ON ""Tours""
FOR EACH ROW EXECUTE FUNCTION trg_tours_text_changed();
");

            migrationBuilder.Sql(@"
CREATE OR REPLACE FUNCTION trg_tours_text_changed()
RETURNS trigger AS $$
BEGIN
    PERFORM update_tour_computed_fields(NEW.""Id"");
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS tours_after_insert_update ON ""Tours"";");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS trg_tours_text_changed();");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS update_tour_computed_fields(integer);");
        }
    }
}
