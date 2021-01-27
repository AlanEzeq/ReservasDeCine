using ReservasDeCine.Database;
using ReservasDeCine.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace ReservasDeCine.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ReservasDeCineDbContext _context;
        public HomeController(ReservasDeCineDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Cliente = new SelectList(_context.Clientes, nameof(Cliente.Id), nameof(Cliente.Email));
            //ViewBag.Categorias = new SelectList(_context.Categorias, nameof(Categoria.Id), nameof(Categoria.Descripcion));
            ViewBag.Generos = new SelectList(_context.Generos, nameof(Genero.Id), nameof(Genero.Nombre));

            return View();
        }
    }
}
