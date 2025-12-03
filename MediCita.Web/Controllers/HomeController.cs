using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Librería para proteger la vista

namespace MediCita.Web.Controllers
{
   // [Authorize] // <--- ¡Esto protege todo el controlador!
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}