using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReservasDeCine.Migrations
{
    public partial class PrimeraVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    FechaAlta = table.Column<DateTime>(nullable: false),
                    Password = table.Column<byte[]>(nullable: true),
                    DNI = table.Column<int>(maxLength: 20, nullable: false),
                    Telefono = table.Column<string>(maxLength: 20, nullable: false),
                    Direccion = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    FechaAlta = table.Column<DateTime>(nullable: false),
                    Password = table.Column<byte[]>(nullable: true),
                    DNI = table.Column<int>(maxLength: 20, nullable: false),
                    Telefono = table.Column<string>(maxLength: 20, nullable: false),
                    Direccion = table.Column<string>(maxLength: 50, nullable: false),
                    Legajo = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Generos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Generos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Peliculas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FechaLanzamiento = table.Column<int>(nullable: false),
                    Titulo = table.Column<string>(maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peliculas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoSalas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoSalas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PeliculaGeneros",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GeneroId = table.Column<Guid>(nullable: false),
                    PeliculaId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeliculaGeneros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeliculaGeneros_Generos_GeneroId",
                        column: x => x.GeneroId,
                        principalTable: "Generos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeliculaGeneros_Peliculas_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "Peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Salas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Numero = table.Column<int>(nullable: false),
                    TipoSalaId = table.Column<Guid>(nullable: false),
                    CapacidadButacas = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Salas_TipoSalas_TipoSalaId",
                        column: x => x.TipoSalaId,
                        principalTable: "TipoSalas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Funciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Hora = table.Column<int>(nullable: false),
                    Descripcion = table.Column<string>(maxLength: 100, nullable: false),
                    ButacasDisponibles = table.Column<int>(nullable: false),
                    Confirmada = table.Column<bool>(nullable: false),
                    PeliculaId = table.Column<Guid>(nullable: false),
                    SalaId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Funciones_Peliculas_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "Peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Funciones_Salas_SalaId",
                        column: x => x.SalaId,
                        principalTable: "Salas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FuncionId = table.Column<Guid>(nullable: false),
                    FechaAlta = table.Column<DateTime>(nullable: false),
                    ClienteId = table.Column<Guid>(nullable: false),
                    CantidadButacas = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservas_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservas_Funciones_FuncionId",
                        column: x => x.FuncionId,
                        principalTable: "Funciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Funciones_PeliculaId",
                table: "Funciones",
                column: "PeliculaId");

            migrationBuilder.CreateIndex(
                name: "IX_Funciones_SalaId",
                table: "Funciones",
                column: "SalaId");

            migrationBuilder.CreateIndex(
                name: "IX_PeliculaGeneros_GeneroId",
                table: "PeliculaGeneros",
                column: "GeneroId");

            migrationBuilder.CreateIndex(
                name: "IX_PeliculaGeneros_PeliculaId",
                table: "PeliculaGeneros",
                column: "PeliculaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_ClienteId",
                table: "Reservas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_FuncionId",
                table: "Reservas",
                column: "FuncionId");

            migrationBuilder.CreateIndex(
                name: "IX_Salas_TipoSalaId",
                table: "Salas",
                column: "TipoSalaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "PeliculaGeneros");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Generos");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Funciones");

            migrationBuilder.DropTable(
                name: "Peliculas");

            migrationBuilder.DropTable(
                name: "Salas");

            migrationBuilder.DropTable(
                name: "TipoSalas");
        }
    }
}
