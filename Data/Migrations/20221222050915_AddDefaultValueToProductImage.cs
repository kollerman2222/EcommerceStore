using Microsoft.EntityFrameworkCore.Migrations;

namespace TawassolProject.Data.Migrations
{
    public partial class AddDefaultValueToProductImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
            name: "ProductImage",
            table: "Products",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "/Uploads/defaultproduct.PNG");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
             name: "ProductImage",
             table: "Products",
             type: "nvarchar(max)",
             nullable: false,
             defaultValue: null);
        }
    }
}
