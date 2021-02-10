using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservasDeCine.Database;
using ReservasDeCine.Models;
using ReservasDeCine.Extensions;
using Microsoft.AspNetCore.Authorization;
//using ReservasDeCine.Models.Enums;

namespace ReservasDeCine.Controllers
{
    [Authorize]
    public class EmpleadosController : Controller
    {
        private readonly ReservasDeCineDbContext _context;

        public EmpleadosController(ReservasDeCineDbContext context)
        {
            _context = context;
        }

        //       [Authorize(Roles = nameof(Rol.Administrador))]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Empleados.ToListAsync());
        }

        //        [Authorize(Roles = nameof(Rol.Administrador))]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
              .FirstOrDefaultAsync(m => m.Id == id);

            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // AR acordarse que los nombres deben coincidir con el View para conectarlos
        public IActionResult Create(Empleado empleado, string Password)
        {
            try
            {
                Password.ValidarPassword();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Empleado.Password), ex.Message);
            }


            if (ModelState.IsValid)
            {
                empleado.Id = Guid.NewGuid();
                empleado.FechaAlta = DateTime.Now;
                empleado.Password = Password.Encriptar();
                empleado.Legajo = Guid.NewGuid();

                _context.Add(empleado);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
        }

        //    [Authorize(Roles = nameof(Rol.Administrador))]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        //    [Authorize(Roles = nameof(Rol.Administrador))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Empleado empleado, string Password)
        {
            if (!string.IsNullOrWhiteSpace(Password))
            {
                try
                {
                    Password.ValidarPassword();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(Empleado.Password), ex.Message);
                }
            }

            if (id != empleado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var empleadoeDb = _context.Empleados.FirstOrDefault(empleado => empleado.Id == id);

                    empleadoeDb.Nombre = empleado.Nombre;
                    empleadoeDb.Apellido = empleado.Apellido;
                    empleadoeDb.Email = empleado.Email;
                    empleadoeDb.FechaAlta = empleado.FechaAlta;
                    empleadoeDb.Password = empleado.Password;
                    empleadoeDb.DNI = empleado.DNI;
                    empleadoeDb.Telefono = empleado.Telefono;
                    empleadoeDb.Direccion = empleado.Direccion;

                    if (!string.IsNullOrWhiteSpace(Password))
                    {
                        empleadoeDb.Password = Password.Encriptar();
                    }

                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.Id))
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
            return View(empleado);
        }

        //       [Authorize(Roles = nameof(Rol.Administrador))]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        //       [Authorize(Roles = nameof(Rol.Administrador))]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(Guid id)
        {
            return _context.Empleados.Any(e => e.Id == id);
        }
    }
}