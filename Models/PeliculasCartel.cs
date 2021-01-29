using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReservasDeCine.Models
{
    public class PeliculasCartel
    {  
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
    }
}
