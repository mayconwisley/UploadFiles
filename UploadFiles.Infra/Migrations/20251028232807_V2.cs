﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UploadFiles.Infra.Migrations;

/// <inheritdoc />
public partial class V2 : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "Users",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "TEXT", nullable: false),
				Username = table.Column<string>(type: "TEXT", nullable: false),
				Password = table.Column<string>(type: "TEXT", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_Users", x => x.Id);
			});

		migrationBuilder.CreateIndex(
			name: "IX_Users_Username",
			table: "Users",
			column: "Username",
			unique: true);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "Users");
	}
}
