using ReservasDeCine.Database;
using ReservasDeCine.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Index()
        {
            return View(await _context.PeliculasCartel.ToListAsync());
        }
    }
}
