using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasDeCine.Models
{
    public class Funcion
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Fecha")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Range(0, 24, ErrorMessage = "El horario de la funcion es desde las 0 hasta las 24 hs.")]
        [RegularExpression(@"[0-9]*", ErrorMessage = "El campo admite sólo caracteres numéricos.")]
        [Display(Name = "Hora")]
        public int Hora { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [MaxLength(100, ErrorMessage = "La longitud máxima del campo es de 100 caracteres")]
        [RegularExpression(@"[a-zA-Z0-9_\-]*", ErrorMessage = "El campo admite sólo caracteres alfanuméricos, guión bajo o guión medio")]
        [Display(Name = "Descripcion")]
        public String Descripcion { get; set; }


        [Required(ErrorMessage = "Este campo es requerido")]
        [Range(0, 500, ErrorMessage = "Cantidad maxima de butacas es {1}.")]
        [RegularExpression(@"[0-9]*", ErrorMessage = "El campo admite sólo caracteres numéricos.")]
        [Display(Name = "Butacas Disponibles")]
        public int ButacasDisponibles { get; set; }

        [Display(Name = "Funcion confirmada")]
        public Boolean Confirmada { get; set; }

        [ForeignKey(nameof(Pelicula))]
        [Display(Name = "Pelicula")]
        public Guid PeliculaId { get; set; }

        public Pelicula Pelicula { get; set; }

        [ForeignKey(nameof(Sala))]
        [Display(Name = "Sala")]
        public Guid SalaId { get; set; }
        public Sala Sala { get; set; }
   

        public List<Reserva> Reserva { get; set; }
    }
}
