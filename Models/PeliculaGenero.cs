using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasDeCine.Models
{
    public class PeliculaGenero
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Genero))]
        [Display(Name = "Genero")]
        public Guid GeneroId { get; set; }
        public Genero Genero { get; set; }

        [ForeignKey(nameof(Pelicula))]
        [Display(Name = "Pelicula")]
        public Guid PeliculaId { get; set; }
        public Pelicula Pelicula { get; set; }
    }
}
