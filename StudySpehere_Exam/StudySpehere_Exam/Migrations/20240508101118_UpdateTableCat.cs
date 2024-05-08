﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudySpehere_Exam.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableCat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "categories");
        }
    }
}
