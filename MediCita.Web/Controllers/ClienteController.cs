using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediCita.Web.Servicios.Contrato;
using System.Security.Claims;

namespace MediCita.Web.Controllers
{
    [Authorize(Roles = "Paciente")]
    public class ClienteController : Controller
    {
        private readonly ICitaService _citaService;
        private readonly IVentaService _ventaService;

        public ClienteController(ICitaService citaService, IVentaService ventaService)
        {
            _citaService = citaService;
            _ventaService = ventaService;
        }

        public async Task<IActionResult> Dashboard()
        {
            // Usar ClaimTypes.NameIdentifier
            int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            if (idUsuario == 0)
            {
                return RedirectToAction("Login", "Acceso");
            }

            // Obtener estadísticas del paciente
            var citas = await _citaService.ReporteCitasUsuario(idUsuario);
            var compras = await _ventaService.ListarVentasPorUsuario(idUsuario);

            ViewBag.TotalCitas = citas.Count;
            ViewBag.CitasPendientes = citas.Count(c => c.Estado == "P" || c.Estado == "Pendiente");
            ViewBag.TotalCompras = compras.Count;
            ViewBag.TotalGastado = compras.Sum(v => v.Total);

            // Filtrar solo citas próximas (futuras) y pasarlas al Model
            var citasProximas = citas
                .Where(c => c.FechaCita >= DateTime.Now)
                .OrderBy(c => c.FechaCita)
                .ToList();

            return View(citasProximas);
        }

        public async Task<IActionResult> MisCitas()
        {
            int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            if (idUsuario == 0)
            {
                return RedirectToAction("Login", "Acceso");
            }

            var citas = await _citaService.ReporteCitasUsuario(idUsuario);
            return View(citas);
        }

        public IActionResult MiPerfil()
        {
            return View();
        }

        public async Task<IActionResult> MisCompras()
        {
            int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            if (idUsuario == 0)
            {
                return RedirectToAction("Login", "Acceso");
            }

            var compras = await _ventaService.ListarVentasPorUsuario(idUsuario);
            return View(compras);
        }
    }
}
