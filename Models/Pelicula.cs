using ReservasDeCine.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReservasDeCine.Models
{
    public class Pelicula
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Año de lanzamiento")]
        [AnioDesdeHastaHoy(1950)] 
        public int FechaLanzamiento { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del campo es de 50 caracteres")]
        [RegularExpression(@"[ áéíóúña-zA-Z0-9_\-]*", ErrorMessage = "El campo admite sólo caracteres alfanuméricos, guión bajo o guión medio")]
        [Display(Name = "Titulo")]
        public String Titulo { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [MaxLength(100, ErrorMessage = "La longitud máxima del campo es de 100 caracteres")]
        [RegularExpression(@"[ áéíóúña-zA-Z0-9_\-]*", ErrorMessage = "El campo admite sólo caracteres alfanuméricos, guión bajo o guión medio")]
        [Display(Name = "Descripcion")]
        public String Descripcion { get; set; }
        public List<PeliculaGenero> Generos { get; set; }
        public List<Funcion> Funciones { get; set; }

    }
}