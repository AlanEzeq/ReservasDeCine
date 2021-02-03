using System;
using System.ComponentModel.DataAnnotations;

namespace ReservasDeCine.Models
{
    public class Empleado : Usuario
    {
        [Required(ErrorMessage = "Este campo es requerido")]
        [RegularExpression(@"[0-9]{8}", ErrorMessage = "El campo debe ser del formato NN.NNN.NNN")]
        [MaxLength(20, ErrorMessage = "El campo {0} tiene un máximo de {1} caracteres")]
        [Display(Name = "DNI")]
        public String DNI { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [RegularExpression(@"^\d{3}-\d{4}-\d{4}$", ErrorMessage = "El campo debe ser del formato NNN-NNNN-NNNN")]
        [MaxLength(20, ErrorMessage = "El campo {0} tiene un máximo de {1} caracteres")]
        [Display(Name = "Telefono")]
        public String Telefono { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del campo es de 50 caracteres")]
        [RegularExpression(@"[ áéíóúña-zA-Z0-9_\-]*", ErrorMessage = "El campo admite sólo caracteres alfanuméricos, guión bajo o guión medio")]
        [Display(Name = "Direccion")]
        public String Direccion { get; set; }

        [ScaffoldColumn(false)]
        public Guid Legajo { get; set; }
    }
}