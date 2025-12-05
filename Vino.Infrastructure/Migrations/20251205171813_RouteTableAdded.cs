using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetterThanYou.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RouteTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DataRota = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    EnderecoPartida = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CidadePartida = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EstadoPartida = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    CepPartida = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    LatitudePartida = table.Column<double>(type: "double precision", precision: 10, scale: 8, nullable: true),
                    LongitudePartida = table.Column<double>(type: "double precision", precision: 11, scale: 8, nullable: true),
                    DistanciaTotal = table.Column<double>(type: "double precision", precision: 10, scale: 2, nullable: true),
                    TempoEstimado = table.Column<int>(type: "integer", nullable: true),
                    Otimizada = table.Column<bool>(type: "boolean", nullable: false),
                    DataOtimizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CriadoPor = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModificadoPor = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RouteStops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RouteId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrdemOriginal = table.Column<int>(type: "integer", nullable: false),
                    OrdemOtimizada = table.Column<int>(type: "integer", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", precision: 10, scale: 8, nullable: true),
                    Longitude = table.Column<double>(type: "double precision", precision: 11, scale: 8, nullable: true),
                    DistanciaParadaAnterior = table.Column<double>(type: "double precision", precision: 10, scale: 2, nullable: true),
                    TempoParadaAnterior = table.Column<int>(type: "integer", nullable: true),
                    HorarioChegadaPrevisto = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HorarioChegadaReal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HorarioSaidaReal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Visitado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteStops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteStops_Clients_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RouteStops_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_Ativo_DataRota",
                table: "Routes",
                columns: new[] { "Ativo", "DataRota" });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_DataRota",
                table: "Routes",
                column: "DataRota");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_Status",
                table: "Routes",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStops_ClienteId",
                table: "RouteStops",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStops_RouteId",
                table: "RouteStops",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStops_RouteId_OrdemOtimizada",
                table: "RouteStops",
                columns: new[] { "RouteId", "OrdemOtimizada" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouteStops");

            migrationBuilder.DropTable(
                name: "Routes");
        }
    }
}
