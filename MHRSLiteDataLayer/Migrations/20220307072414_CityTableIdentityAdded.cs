using Microsoft.EntityFrameworkCore.Migrations;

namespace MHRSLiteDataLayer.Migrations
{
    public partial class CityTableIdentityAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Id",
                table: "Cities",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint")
                .Annotation("SqlServer:Identity", "1, 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Id",
                table: "Cities",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
