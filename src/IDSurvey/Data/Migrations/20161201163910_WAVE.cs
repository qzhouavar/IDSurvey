using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IDSurvey.Data.Migrations
{
    public partial class WAVE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "WAVE",
                table: "CompleteRates",
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WAVE",
                table: "CompleteRates",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
