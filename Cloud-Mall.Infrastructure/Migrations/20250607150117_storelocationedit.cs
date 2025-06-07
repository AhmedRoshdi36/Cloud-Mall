using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cloud_Mall.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class storelocationedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationID",
                table: "StoreAddresses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationID",
                table: "StoreAddresses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
