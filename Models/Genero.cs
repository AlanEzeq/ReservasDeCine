using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReservasDeCine.Models
{
    public class Genero
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del campo es de 50 caracteres")]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = "El campo admite sólo caracteres alfabéticos")]
        [Display(Name = "Nombre")]
        public String Nombre { get; set; }
        public List<PeliculaGenero> Peliculas{ get; set; }
    }
}
