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
    public class SalasController : Controller
    {
        private readonly ReservasDeCineDbContext _context;

        public SalasController(ReservasDeCineDbContext context)
        {
            _context = context;
        }

        //       [Authorize(Roles = nameof(Rol.Administrador))]
        public async Task<IActionResult> Index()
        {
            // AR despues de resolver el logueo filtrar por cliente
            var ReservasDeCineDbContext = _context.Salas
                .Include(j => j.TipoSala);

            return View(await ReservasDeCineDbContext.ToListAsync());
        }

        //        [Authorize(Roles = nameof(Rol.Administrador))]

        public IActionResult Create()
        {
            ViewData["TipoSalaId"] = new SelectList(_context.TipoSalas, "Id", "Nombre");
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sala sala)
        {
            if (ModelState.IsValid)
            {
                 sala.Id = Guid.NewGuid();
                _context.Add(sala);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TipoSalaId"] = new SelectList(_context.TipoSalas, "Id", "Nombre",sala.TipoSalaId);
            return View(sala);
        }


        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sala = await _context.Salas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sala == null)
            {
                return NotFound();
            }

            return View(sala);
        }

        public async Task<IActionResult> EditAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sala = await _context.Salas.FindAsync(id);

            if (sala == null)
            {
                return NotFound();
            }

            //var tipoSalaId = sala.TipoSala.Id;

            ViewData["TipoSalaId"] = new SelectList(_context.TipoSalas, "Id", "Nombre", sala.TipoSalaId);

            return View(sala);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Sala sala)//, Guid tipoSala)
        {
            if (id != sala.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   // sala.TipoSalaId = tipoSala;
                    _context.Update(sala);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaExists(sala.Id))
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
            return View(sala);
        }

        private bool SalaExists(Guid id)
        {
            return _context.Salas.Any(e => e.Id == id);
        }

    }

}