using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservasDeCine.Database;
using ReservasDeCine.Models;
using ReservasDeCine.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using ReservasDeCine.Models.Enums;
using System.Security.Claims;

namespace ReservasDeCine.Controllers
{
    [Authorize]
    public class BalanceRecaudacionController : Controller
    {
        
        private readonly ReservasDeCineDbContext _context;
        public BalanceRecaudacionController(ReservasDeCineDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = nameof(Rol.Empleado))]
        public async Task<IActionResult> Index()
        {
            ViewData["PeliculaId"] = new SelectList(_context.Peliculas, "Id", "Titulo");
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerBalance(Guid PeliculaId, int Ano, int Mes)
        {
            if (PeliculaId == null)
            {
                return NotFound();
            }

            var reservasBalanceContext = _context.Reservas
                  .Include(j => j.Funcion)
                  .ThenInclude(k => k.Sala)
                  .ThenInclude(l => l.TipoSala)
                  .Include(i => i.Funcion)
                  .ThenInclude(h => h.Pelicula)
                  .Where(i => i.Funcion.PeliculaId == PeliculaId)
                  .Where(j => j.Funcion.Fecha.Year.ToString() == Ano.ToString())
                  .Where(j => j.Funcion.Fecha.Month.ToString() == Mes.ToString());

            var peliculasContext = _context.Peliculas
                .Where(j => j.Id == PeliculaId);

            var listaReservas = reservasBalanceContext.ToList(); 

            TempData["Ano"] = Ano;
            TempData["Mes"] = Mes;
            Decimal montoRecaudado = 0;

            if (listaReservas.FirstOrDefault() == null)
            {
                TempData["Titulo"] = "No hay reservas de " + peliculasContext.FirstOrDefault().Titulo + " bajo estos parámetros";
                TempData["Total"] = string.Format("{0:C}", montoRecaudado);
                return View();
            }



            foreach (Reserva reserva in listaReservas)
            {
                montoRecaudado = montoRecaudado + (reserva.Funcion.Sala.TipoSala.Precio * reserva.CantidadButacas);
            }

            // AR convierto el total a un string con formato moneda
            TempData["Total"] = string.Format("{0:C}", montoRecaudado);
            TempData["Titulo"] = "Monto recaudado de " + peliculasContext.FirstOrDefault().Titulo;

            return View();


        }


    }
}