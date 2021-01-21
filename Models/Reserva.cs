using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasDeCine.Models
{
    public class Reserva
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Funcion))]
        [Display(Name = "Funcion")]
        public Guid FuncionId { get; set; }
        public Funcion Funcion { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Alta")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm}")]
        public DateTime FechaAlta { get; set; }

        [ForeignKey(nameof(Cliente))]
        [Display(Name = "Cliente")]
        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Range(1, 20, ErrorMessage = "Cantidad maxima de butacas a reservar es {1}.")]
        [RegularExpression(@"[0-9]*", ErrorMessage = "El campo admite sólo caracteres numéricos.")]
        [Display(Name = "Cantidad de butacas")]
        public int CantidadButacas { get; set; }

    }
}
