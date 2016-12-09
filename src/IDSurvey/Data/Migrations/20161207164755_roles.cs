using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IDSurvey.Data.Migrations
{
    public partial class roles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MailSurveyResult",
                columns: table => new
                {
                    key = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CommunicationComp = table.Column<decimal>(nullable: true),
                    CoordComp = table.Column<decimal>(nullable: true),
                    CourtesyComp = table.Column<decimal>(nullable: true),
                    InterviewDate = table.Column<DateTime>(nullable: false),
                    LoadDate = table.Column<DateTime>(nullable: false),
                    OverallComp = table.Column<decimal>(nullable: true),
                    RegionCode = table.Column<int>(nullable: false),
                    ResponsivenessComp = table.Column<decimal>(nullable: true),
                    SampleDate = table.Column<DateTime>(nullable: false),
                    StateCode = table.Column<string>(nullable: true),
                    SurveyOutcome = table.Column<string>(nullable: true),
                    SurveyQuarter = table.Column<string>(nullable: true),
                    SurveyRound = table.Column<int>(nullable: false),
                    SurveyType = table.Column<string>(nullable: true),
                    WesId = table.Column<string>(nullable: true),
                    q1 = table.Column<decimal>(nullable: false),
                    q10 = table.Column<decimal>(nullable: true),
                    q11 = table.Column<decimal>(nullable: true),
                    q12 = table.Column<decimal>(nullable: true),
                    q13 = table.Column<decimal>(nullable: true),
                    q14 = table.Column<decimal>(nullable: true),
                    q15 = table.Column<decimal>(nullable: true),
                    q16 = table.Column<decimal>(nullable: true),
                    q17 = table.Column<decimal>(nullable: true),
                    q18 = table.Column<decimal>(nullable: true),
                    q19 = table.Column<decimal>(nullable: true),
                    q2 = table.Column<decimal>(nullable: true),
                    q20 = table.Column<string>(nullable: true),
                    q3 = table.Column<decimal>(nullable: true),
                    q4 = table.Column<string>(nullable: true),
                    q5 = table.Column<decimal>(nullable: true),
                    q6 = table.Column<decimal>(nullable: true),
                    q7 = table.Column<decimal>(nullable: true),
                    q8 = table.Column<decimal>(nullable: true),
                    q9 = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailSurveyResult", x => x.key);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailSurveyResult");
        }
    }
}
