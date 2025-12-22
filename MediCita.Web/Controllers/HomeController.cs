using Microsoft.AspNetCore.Mvc;


namespace MediCita.Web.Controllers
{
  
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}