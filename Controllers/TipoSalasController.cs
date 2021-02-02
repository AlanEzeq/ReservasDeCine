using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservasDeCine.Database;
using ReservasDeCine.Models;
using Microsoft.AspNetCore.Authorization;
using ReservasDeCine.Models.Enums;

namespace ReservasDeCine.Controllers
{
    // [Authorize(Roles = nameof(Rol.Administrador))]
    public class TipoSalasController : Controller
    {
        private readonly ReservasDeCineDbContext _context;

        public TipoSalasController(ReservasDeCineDbContext context)
        {
            _context = context;
        }

        // GET: Generos
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoSalas.ToListAsync());
        }

        // GET: Generos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSala = await _context.TipoSalas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoSala == null)
            {
                return NotFound();
            }

            return View(tipoSala);
        }

        // GET: Generos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Generos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Precio")] TipoSala tipoSala)
        {
            if (ModelState.IsValid)
            {
                tipoSala.Id = Guid.NewGuid();
                _context.Add(tipoSala);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoSala);
        }

        // GET: Generos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSala = await _context.TipoSalas.FindAsync(id);
            if (tipoSala == null)
            {
                return NotFound();
            }
            return View(tipoSala);
        }

        // POST: Generos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nombre,Precio")] TipoSala tipoSala)
        {
            if (id != tipoSala.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoSala);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoSalaExists(tipoSala.Id))
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
            return View(tipoSala);
        }

        // GET: Generos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSala = await _context.TipoSalas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoSala == null)
            {
                return NotFound();
            }

            return View(tipoSala);
        }

        // POST: Generos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tipoSala = await _context.TipoSalas.FindAsync(id);
            _context.TipoSalas.Remove(tipoSala);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoSalaExists(Guid id)
        {
            return _context.TipoSalas.Any(e => e.Id == id);
        }
    }
}