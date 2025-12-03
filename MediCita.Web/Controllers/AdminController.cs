using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MediCita.Web.Controllers
{
    [Authorize] // ¡Solo usuarios logueados entran aquí!
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            // Aquí podrías inyectar servicios para contar cuántos usuarios, citas o ventas hay hoy
            // Por ahora, enviaremos datos "dummy" para el diseño
            ViewBag.TotalPacientes = 120;
            ViewBag.CitasHoy = 15;
            ViewBag.VentasDia = "S/. 450.00";

            return View();
        }
    }
}