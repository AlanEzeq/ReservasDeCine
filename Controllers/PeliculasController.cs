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
  //  [Authorize(Roles = nameof(Rol.Administrador))]
    public class PeliculasController : Controller
    {
        private readonly ReservasDeCineDbContext _context;

        public PeliculasController(ReservasDeCineDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ReservasDeCineDbContext = _context.Peliculas
                .Include(j => j.Generos)
                    .ThenInclude(i => i.Genero);

            return View(await ReservasDeCineDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas
                .Include(pelicula => pelicula.Generos).ThenInclude(PeliculaGenero => PeliculaGenero.Genero)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            //ViewData[nameof(Genero)] = new MultiSelectList(_context.Generos, nameof(Genero.Id), nameof(Genero.Nombre), generoIds);

            return View(pelicula);
        }
        // ESTOY ACA
        public IActionResult Create()
        {
            ViewData[nameof(Genero)] = new MultiSelectList(_context.Generos, nameof(Genero.Id), nameof(Genero.Nombre));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pelicula pelicula, Guid[] generoIds)
        {
            if (ModelState.IsValid)
            {
                pelicula.Id = Guid.NewGuid();

                foreach (Guid generoId in generoIds)
                {
                    var PeliculaGenero = new PeliculaGenero()
                    {
                        Id = Guid.NewGuid(),
                        GeneroId = generoId,
                        PeliculaId = pelicula.Id
                    };

                    _context.Add(PeliculaGenero);
                }

                _context.Add(pelicula);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewData[nameof(Genero)] = new MultiSelectList(_context.Generos, nameof(Genero.Id), nameof(Genero.Nombre), generoIds);

            return View(pelicula);
        }

        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = _context.Peliculas
                .Include(pelicula => pelicula.Generos)
                .FirstOrDefault(pelicula => pelicula.Id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            var generoIds = pelicula.Generos.Select(PeliculaGenero => PeliculaGenero.GeneroId);

            ViewData[nameof(Genero)] = new MultiSelectList(_context.Generos, nameof(Genero.Id), nameof(Genero.Nombre), generoIds);

            return View(pelicula);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Pelicula pelicula, Guid[] generoIds)
        {
            if (id != pelicula.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var peliculaDb = _context.Peliculas
                        .Include(pelicula => pelicula.Generos)
                        .FirstOrDefault(pelicula => pelicula.Id == id);

                    peliculaDb.Generos.Clear();

                    foreach (Guid generoId in generoIds)
                    {
                        var PeliculaGenero = new PeliculaGenero()
                        {
                            Id = Guid.NewGuid(),
                            GeneroId = generoId,
                            PeliculaId = id
                        };

                        _context.Add(PeliculaGenero);
                    }

                    peliculaDb.FechaLanzamiento = pelicula.FechaLanzamiento;
                    peliculaDb.Titulo = pelicula.Titulo;
                    peliculaDb.Descripcion = pelicula.Descripcion;


                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeliculaExists(pelicula.Id))
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
            return View(pelicula);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas
               .FirstOrDefaultAsync(m => m.Id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);
            _context.Peliculas.Remove(pelicula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //[AllowAnonymous]
        //[HttpGet]
        //public IActionResult Buscar(string titulo, Guid? consolaId, Guid? categoriaId, Guid? generoId)
        //{
        //    var juegos = _context
        //        .Juegos
        //        .Include(x => x.Generos).ThenInclude(x => x.Genero)
        //        .Include(x => x.Consola)
        //        .Include(x => x.Categoria)
        //        .Where(x => (string.IsNullOrWhiteSpace(titulo) || EF.Functions.Like(x.Titulo, $"%{titulo}%"))
        //                    && (!consolaId.HasValue || x.ConsolaId == consolaId.Value)
        //                    && (!categoriaId.HasValue || x.CategoriaId == categoriaId.Value)
        //                    && (!generoId.HasValue || x.Generos.Any(genero => genero.GeneroId == generoId.Value)))
        //        .ToList();

        //    ViewBag.Consolas = new SelectList(_context.Consolas, nameof(Consola.Id), nameof(Consola.Descripcion), consolaId);
        //    ViewBag.Categorias = new SelectList(_context.Categorias, nameof(Categoria.Id), nameof(Categoria.Descripcion), categoriaId);
        //    ViewBag.Generos = new SelectList(_context.Generos, nameof(Genero.Id), nameof(Genero.Descripcion), generoId);
        //    ViewBag.Titulo = titulo;

        //    return View(juegos);
        //}


        private bool PeliculaExists(Guid id)
        {
            return _context.Peliculas.Any(e => e.Id == id);
        }
    }
}
