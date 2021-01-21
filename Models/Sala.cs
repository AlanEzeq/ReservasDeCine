using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasDeCine.Models
{
    public class Sala
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [RegularExpression(@"[0-9]*", ErrorMessage = "El campo admite sólo caracteres numéricos.")]
        [Display(Name = "Nombre de usuario")]
        public int Numero { get; set; }

        [ForeignKey(nameof(TipoSala))]
        [Display(Name = "TipoSala")]
        public Guid TipoSalaId { get; set; } 
        public TipoSala TipoSala { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Range(0,500, ErrorMessage = "Cantidad maxima de butacas es {1}.")]
        [RegularExpression(@"[0-9]*", ErrorMessage = "El campo admite sólo caracteres numéricos.")]
        [Display(Name = "Capacidad de Butacas")]
        public int CapacidadButacas { get; set; }
        public List<Funcion> Funciones { get; set; }
    }
}
