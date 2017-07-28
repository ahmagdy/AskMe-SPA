using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspcorespa.Migrations
{
    public partial class ChangePictoImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PicURL",
                table: "AspNetUsers",
                newName: "ImageUrl");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(2017, 7, 14, 19, 44, 44, 681, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2017, 6, 22, 4, 35, 57, 426, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionDate",
                table: "Messages",
                nullable: false,
                defaultValue: new DateTime(2017, 7, 14, 19, 44, 44, 675, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2017, 6, 22, 4, 35, 57, 419, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "AspNetUsers",
                newName: "PicURL");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(2017, 6, 22, 4, 35, 57, 426, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2017, 7, 14, 19, 44, 44, 681, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionDate",
                table: "Messages",
                nullable: false,
                defaultValue: new DateTime(2017, 6, 22, 4, 35, 57, 419, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2017, 7, 14, 19, 44, 44, 675, DateTimeKind.Local));
        }
    }
}
