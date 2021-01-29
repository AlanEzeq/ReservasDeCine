using Microsoft.EntityFrameworkCore;
using ReservasDeCine.Models;


namespace ReservasDeCine.Database
{
    public class ReservasDeCineDbContext : DbContext
    {
        public ReservasDeCineDbContext(DbContextOptions<ReservasDeCineDbContext> options) : base (options)
        {
        }
        #region DbSets

        public DbSet<Cliente>  Clientes { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Funcion> Funciones { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<PeliculaGenero> PeliculaGeneros { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Sala> Salas { get; set; }
        public DbSet<TipoSala> TipoSalas { get; set; }
        public DbSet<PeliculasCartel> PeliculasCartel { get; set; }

        #endregion

    }
}
