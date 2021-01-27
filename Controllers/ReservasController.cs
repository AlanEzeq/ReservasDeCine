using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservasDeCine.Database;
using ReservasDeCine.Models;
using ReservasDeCine.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            //string sql = @"select distinct pe.Id as Value, pe.Titulo as Text
            //        from dbo.Funciones fu
            //        inner join dbo.Peliculas pe
            //        on fu.PeliculaId = pe.Id
            //        where fu.Fecha > GETDATE()";

            //List<QueryResults> kk = _context.Database.ExecuteSqlRaw(sql,).ToList<QueryResults>();

            ViewData["PeliculaId"] = new SelectList(_context.Peliculas, "Id", "Titulo");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Reserva reserva)
        {

            if (ModelState.IsValid)
            {
                reserva.Id = Guid.NewGuid();
                reserva.FechaAlta = DateTime.Now;
                // AR como mando el cliente la cant de butacas y la funcion.

                _context.Add(reserva);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(reserva);
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
