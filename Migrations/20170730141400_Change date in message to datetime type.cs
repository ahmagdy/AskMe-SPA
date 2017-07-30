using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspcorespa.Migrations
{
    public partial class Changedateinmessagetodatetimetype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(2017, 7, 30, 16, 14, 0, 792, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2017, 7, 23, 16, 5, 34, 871, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionDate",
                table: "Messages",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2017, 7, 23, 16, 5, 34, 862, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(2017, 7, 23, 16, 5, 34, 871, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2017, 7, 30, 16, 14, 0, 792, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionDate",
                table: "Messages",
                nullable: false,
                defaultValue: new DateTime(2017, 7, 23, 16, 5, 34, 862, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getutcdate()");
        }
    }
}
