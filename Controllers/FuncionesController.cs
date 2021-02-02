using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservasDeCine.Database;
using ReservasDeCine.Models;
using Microsoft.AspNetCore.Authorization;
using ReservasDeCine.Models.Enums;

namespace ReservasDeCine.Controllers
{
   // [Authorize(Roles = nameof(Rol.Administrador))]
    public class FuncionesController : Controller
    {
        private readonly ReservasDeCineDbContext _context;

        public FuncionesController(ReservasDeCineDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ReservasDeCineDbContext = _context.Funciones.Include(j => j.Pelicula).Include(j => j.Sala);
            return View(await ReservasDeCineDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcion = await _context.Funciones
                .Include(j => j.Pelicula)
                .Include(j => j.Sala)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcion == null)
            {
                return NotFound();
            }

            return View(funcion);
        }

        public IActionResult Create()
        {
            ViewData["PeliculaId"] = new SelectList(_context.Peliculas, "Id", "Titulo");
            ViewData["SalaId"] = new SelectList(_context.Salas, "Id", "Numero");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Funcion funcion)
        {
            if (ModelState.IsValid)
            {
                funcion.Id = Guid.NewGuid();
                _context.Add(funcion);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PeliculaId"] = new SelectList(_context.Peliculas, "Id", "Titulo", funcion.PeliculaId);
            ViewData["SalaId"] = new SelectList(_context.Salas, "Id", "Numero", funcion.SalaId);

            return View(funcion);
        }

        public async Task<IActionResult> EditAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcion = await _context.Funciones.FindAsync(id);
            
            if (funcion == null)
            {
                return NotFound();
            }


            ViewData["PeliculaId"] = new SelectList(_context.Peliculas, "Id", "Titulo", funcion.PeliculaId);
            ViewData["SalaId"] = new SelectList(_context.Salas, "Id", "Numero", funcion.SalaId);

            return View(funcion);
        }
        // FALTA EL POST DE EDIT

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Funcion funcion)//, Guid tipoSala)
        {
            if (id != funcion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // sala.TipoSalaId = tipoSala;
                    _context.Update(funcion);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuncionExists(funcion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(funcion);
        }


        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcion = await _context.Funciones
                .Include(j => j.Pelicula)
                .Include(j => j.Sala)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcion == null)
            {
                return NotFound();
            }

            return View(funcion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var funcion = await _context.Funciones.FindAsync(id);
            _context.Funciones.Remove(funcion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        private bool FuncionExists(Guid id)
        {
            return _context.Funciones.Any(e => e.Id == id);
        }
    }
}