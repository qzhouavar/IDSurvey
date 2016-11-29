using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IDSurvey.Data.Migrations
{
    public partial class rate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompleteRates",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    QTR = table.Column<string>(nullable: false),
                    WAVE = table.Column<string>(nullable: false),
                    TYPE = table.Column<string>(nullable: false),
                    SERVICE_AREA = table.Column<string>(nullable: false),
                    TOTAL = table.Column<int>(nullable: false),
                    COMPLETE = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompleteRates", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompleteRates");
        }
    }
}
