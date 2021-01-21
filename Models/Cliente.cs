using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReservasDeCine.Models
{
    public class Cliente : Usuario
    {

        [Required(ErrorMessage = "Este campo es requerido")]
        [RegularExpression(@"[0-9]{2}\.[0-9]{3}\.[0-9]{3}", ErrorMessage = "El campo debe ser del formato NN.NNN.NNN")]
        [MaxLength(20, ErrorMessage = "El campo {0} tiene un máximo de {1} caracteres")]
        [Display(Name = "DNI")]
        public int DNI { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [RegularExpression(@"^\d{3}-\d{4}-\d{4}$", ErrorMessage = "El campo debe ser del formato NNN-NNNN-NNNN")]
        [MaxLength(20, ErrorMessage = "El campo {0} tiene un máximo de {1} caracteres")]
        [Display(Name = "Telefono")]
        public String Telefono { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del campo es de 50 caracteres")]
        [RegularExpression(@"[a-zA-Z0-9_\-]*", ErrorMessage = "El campo admite sólo caracteres alfanuméricos, guión bajo o guión medio")]
        [Display(Name = "Direccion")]
        public String Direccion { get; set; }


        public List<Reserva> Reservas { get; set; }

    }
}
