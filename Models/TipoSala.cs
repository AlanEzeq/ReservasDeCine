using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasDeCine.Models
{
    public class TipoSala
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [MaxLength(100, ErrorMessage = "La longitud máxima del campo es de 100 caracteres")]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = "El campo admite sólo caracteres alfabéticos")]
        [Display(Name = "Nombre")]
        public String Nombre { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Range(0, 999.99)]
        [Display(Name = "Precio")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Precio { get; set; }
        public List<Sala> Salas { get; set; }
    }
}
