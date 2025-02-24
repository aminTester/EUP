using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorWasmAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangrProfModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Professors");

            migrationBuilder.AddColumn<int>(
                name: "Country",
                table: "Professors",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Professors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailDate",
                table: "Professors",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Professors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "Professors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Papers",
                table: "Professors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Related",
                table: "Professors",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Result",
                table: "Professors",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Professors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "University",
                table: "Professors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Professors",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Web",
                table: "Professors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Wos",
                table: "Professors",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "EmailDate",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "Papers",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "Related",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "University",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "Web",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "Wos",
                table: "Professors");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Professors",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Professors",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Professors",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
