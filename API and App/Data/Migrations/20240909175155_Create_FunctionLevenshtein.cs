using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RR.Data.Migrations
{
    /// <inheritdoc />
    public partial class Create_FunctionLevenshtein : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                CREATE OR ALTER FUNCTION dbo.Levenshtein(@s1 NVARCHAR(MAX), @s2 NVARCHAR(MAX))
                RETURNS INT
                AS
                BEGIN
                    DECLARE @s1_len INT, @s2_len INT
                    DECLARE @i INT, @j INT, @c INT, @c_temp INT
                    DECLARE @cv0 VARBINARY(MAX), @cv1 VARBINARY(MAX)

                    SELECT
                        @s1_len = LEN(@s1),
                        @s2_len = LEN(@s2),
                        @cv1 = 0x0000

                    WHILE @j <= @s2_len
                        SELECT @cv1 = @cv1 + CAST(@j AS BINARY(2)), @j = @j + 1

                    SELECT @i = 1
                    WHILE @i <= @s1_len
                    BEGIN
                        SELECT
                            @c = @i,
                            @cv0 = CAST(@i AS BINARY(2)),
                            @j = 1

                        WHILE @j <= @s2_len
                        BEGIN
                            SET @c = @c + 1
                            SET @c_temp = CAST(SUBSTRING(@cv1, @j * 2 - 1, 2) AS INT) +
                                CASE WHEN SUBSTRING(@s1, @i, 1) = SUBSTRING(@s2, @j, 1) THEN 0 ELSE 1 END
                            IF @c > @c_temp SET @c = @c_temp
                            SET @c_temp = CAST(SUBSTRING(@cv1, @j * 2 + 1, 2) AS INT) + 1
                            IF @c > @c_temp SET @c = @c_temp
                            SELECT @cv0 = @cv0 + CAST(@c AS BINARY(2)), @j = @j + 1
                        END

                        SELECT @cv1 = @cv0, @i = @i + 1
                    END

                    RETURN @c
                END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION dbo.Levenshtein");
        }
    }
}
