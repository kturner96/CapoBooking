using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapoBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddedServiceDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Services",
                type: "text",
                nullable: false,
                defaultValue: ""
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Description", table: "Services");
        }
    }
}
