using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminIvoire.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Superficie = table.Column<decimal>(type: "numeric", nullable: false),
                    Population = table.Column<int>(type: "integer", nullable: false),
                    CoordonneesGeographiques_Latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    CoordonneesGeographiques_Longitude = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parametrage",
                columns: table => new
                {
                    Key = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parametrage", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DistrictId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Superficie = table.Column<decimal>(type: "numeric", nullable: false),
                    Population = table.Column<int>(type: "integer", nullable: false),
                    CoordonneesGeographiques_Latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    CoordonneesGeographiques_Longitude = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Regions_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Departements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RegionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Superficie = table.Column<decimal>(type: "numeric", nullable: false),
                    Population = table.Column<int>(type: "integer", nullable: false),
                    CoordonneesGeographiques_Latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    CoordonneesGeographiques_Longitude = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departements_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Communes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartementId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Superficie = table.Column<decimal>(type: "numeric", nullable: false),
                    Population = table.Column<int>(type: "integer", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "SousPrefectures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartementId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Superficie = table.Column<decimal>(type: "numeric", nullable: false),
                    Population = table.Column<int>(type: "integer", nullable: false),
                    CoordonneesGeographiques_Latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    CoordonneesGeographiques_Longitude = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SousPrefectures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SousPrefectures_Departements_DepartementId",
                        column: x => x.DepartementId,
                        principalTable: "Departements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Villages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SousPrefectureId = table.Column<Guid>(type: "uuid", nullable: false),
                    CommuneId = table.Column<Guid>(type: "uuid", nullable: true),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Superficie = table.Column<decimal>(type: "numeric", nullable: false),
                    Population = table.Column<int>(type: "integer", nullable: false),
                    CoordonneesGeographiques_Latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    CoordonneesGeographiques_Longitude = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Villages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Villages_Communes_CommuneId",
                        column: x => x.CommuneId,
                        principalTable: "Communes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Villages_SousPrefectures_SousPrefectureId",
                        column: x => x.SousPrefectureId,
                        principalTable: "SousPrefectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Communes_DepartementId",
                table: "Communes",
                column: "DepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_Communes_Nom",
                table: "Communes",
                column: "Nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departements_Nom",
                table: "Departements",
                column: "Nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departements_RegionId",
                table: "Departements",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_Nom",
                table: "Districts",
                column: "Nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_DistrictId",
                table: "Regions",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Nom",
                table: "Regions",
                column: "Nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SousPrefectures_DepartementId",
                table: "SousPrefectures",
                column: "DepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_Villages_CommuneId",
                table: "Villages",
                column: "CommuneId");

            migrationBuilder.CreateIndex(
                name: "IX_Villages_SousPrefectureId",
                table: "Villages",
                column: "SousPrefectureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parametrage");

            migrationBuilder.DropTable(
                name: "Villages");

            migrationBuilder.DropTable(
                name: "Communes");

            migrationBuilder.DropTable(
                name: "SousPrefectures");

            migrationBuilder.DropTable(
                name: "Departements");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Districts");
        }
    }
}
