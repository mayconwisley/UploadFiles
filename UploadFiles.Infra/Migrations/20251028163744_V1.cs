using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UploadFiles.Infra.Migrations;

/// <inheritdoc />
public partial class V1 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "PathFiles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Path = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PathFiles", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_PathFiles_Path",
            table: "PathFiles",
            column: "Path",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PathFiles");
    }
}
