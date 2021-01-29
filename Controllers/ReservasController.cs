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

namespace ReservasDeCine.Controllers
{
    //   [Authorize]
    public class ReservasController : Controller
    {
        private readonly ReservasDeCineDbContext _context;

        public ReservasController(ReservasDeCineDbContext context)
        {
            _context = context;
        }

        //       [Authorize(Roles = nameof(Rol.Administrador))]
        public async Task<IActionResult> Index()
        {
            // AR despues de resolver el logueo filtrar por cliente
            var ReservasDeCineDbContext = _context.Reservas
                .Include(j => j.Funcion)
                .ThenInclude(i => i.Pelicula)
                .Include(j => j.Funcion)
                .ThenInclude(i => i.Sala)
                .OrderByDescending(j => j.FechaAlta);

            return View(await ReservasDeCineDbContext.ToListAsync());
        }

        //        [Authorize(Roles = nameof(Rol.Administrador))]

        public IActionResult Create()
        {
            // AR filtrar solo las peliculas cuyas funciones esten habilitadas
            //var ReservasDeCineDbContext = _context.Peliculas.Join(_context.Funciones, x => x.Id, y => y.PeliculaId,
            //    (Pelicula, Funcion) => new { Pelicula, Funcion })
            //    .Where(x => x.Funcion.PeliculaId == x.Pelicula.Id).Where(x => x.Funcion.Confirmada == true);

            //ViewData["PeliculaId"] = new SelectList(ReservasDeCineDbContext.ToList(), "Pelicula.Id", "Titulo");

            // AR el lambda es un lio por lo que hice una vista (PeliculasCartel) con las peliculas disponibles en cartel y listo !

            ViewData["PeliculaId"] = new SelectList(_context.PeliculasCartel, "Id", "Titulo");
            return View();
        }

        [HttpGet("Reservas/CargaFunciones/{PeliculaId:Guid}/{Fecha}/{CantidadButacas}")]
        public JsonResult CargaFunciones(Guid? Peliculaid, string Fecha, int? CantidadButacas)
        {
            if (Peliculaid == null || Fecha == null || CantidadButacas == null)
                return Json(new { ok = false, msg = "Debe elegir una película, fecha y cantidad de butacas" });
            // AR traigo de la base de datos los horarios y salas disponibles 
            string query = @"SELECT fu.Id
              ,CONVERT(VARCHAR, fu.Fecha, 5) + ' a las ' + CONVERT(VARCHAR, fu.Hora) +  ' Sala:' + CONVERT(VARCHAR, sa.Numero) + ' Butacas disponibles:' + CONVERT(VARCHAR, fu.ButacasDisponibles) TextoFuncion
              FROM dbo.Funciones fu
              INNER JOIN dbo.Salas sa
              ON fu.SalaId = sa.Id
              WHERE" +
              " fu.PeliculaId = '" + Peliculaid + "'" +
              " AND CONVERT(varchar,fu.fecha,5) = '" + Fecha + "'" +
              " AND fu.Confirmada = 1 " +
              " AND fu.ButacasDisponibles >= " + CantidadButacas;

            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                List<SelectListItem> funcionesli = new List<SelectListItem>();
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    funcionesli.Add(new SelectListItem { Text = rdr[1].ToString(), Value = rdr[0].ToString() });
                }
                connection.Close();

                return Json(funcionesli);
            }


            // var FuncionesConfirmadas = _context.Funciones;
            //       .Where(c => c.PeliculaId == Peliculaid);
            // convierto las funciones el una lista de SelectlistItems
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Guid FuncionId, int CantidadButacas)
        {
            Reserva nuevaReserva = new Reserva();

            if (ModelState.IsValid)
            {
                nuevaReserva.Id = Guid.NewGuid();
                nuevaReserva.FechaAlta = DateTime.Now;
                // AR Hardcodeo cliente por ahora
                nuevaReserva.ClienteId = new Guid("F6A5E928-F3CD-4636-A362-0A4431C9DEDF");
                nuevaReserva.CantidadButacas = CantidadButacas;
                nuevaReserva.FuncionId = FuncionId;
                _context.Add(nuevaReserva);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(nuevaReserva);
        }

        //Action result for ajax call
        [HttpPost]
        public async Task<IActionResult> GetFuncionesReserva(Guid? Peliculaid, DateTime FechaReserva, int CantidadButacas)
        {
            if (Peliculaid == null)
            {
                return NotFound();
            }

            var funciones = _context.Funciones.Where(c => c.PeliculaId == Peliculaid).Where(c => c.Fecha == FechaReserva).ToList();

            if (funciones == null)
            {
                return NotFound();
            }

            return View(funciones);
            //List<Funcion> funciones = new List<Funcion>();
            //funciones = GetAllCity().Where(m => m.StateId == stateid).ToList();
            //SelectList obgcity = new SelectList(objcity, "Id", "CityName", 0);
            //return Json(funciones);
        }


        //       [Authorize(Roles = nameof(Rol.Administrador))]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }
            if (reserva.Funcion.Fecha <= DateTime.Now.AddDays(1))
            {
                TempData["Error"] = "No se puede cancelar una reserva 24 hs. antes de la funcion";
            }

            return View(reserva);
        }

        //       [Authorize(Roles = nameof(Rol.Administrador))]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
