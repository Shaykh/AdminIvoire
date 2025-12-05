using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminIvoire.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RetraitCommune : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Villages_Communes_CommuneId",
                table: "Villages");

            migrationBuilder.DropTable(
                name: "Communes");

            migrationBuilder.DropIndex(
                name: "IX_Villages_CommuneId",
                table: "Villages");

            migrationBuilder.DropColumn(
                name: "CommuneId",
                table: "Villages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CommuneId",
                table: "Villages",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Communes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartementId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Population = table.Column<int>(type: "integer", nullable: false),
                    Superficie = table.Column<decimal>(type: "numeric", nullable: false),
                    CoordonneesGeographiques_Latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    CoordonneesGeographiques_Longitude = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Communes_Departements_DepartementId",
                        column: x => x.DepartementId,
                        principalTable: "Departements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Villages_CommuneId",
                table: "Villages",
                column: "CommuneId");

            migrationBuilder.CreateIndex(
                name: "IX_Communes_DepartementId",
                table: "Communes",
                column: "DepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_Communes_Nom",
                table: "Communes",
                column: "Nom",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Villages_Communes_CommuneId",
                table: "Villages",
                column: "CommuneId",
                principalTable: "Communes",
                principalColumn: "Id");
        }
    }
}
