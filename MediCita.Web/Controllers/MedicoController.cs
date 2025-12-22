using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediCita.Web.Servicios.Contrato;
using System.Security.Claims;

namespace MediCita.Web.Controllers
{
    [Authorize(Roles = "Medico")]
    public class MedicoController : Controller
    {
        private readonly ICitaService _citaService;
        private readonly IMedicoService _medicoService;

        public MedicoController(ICitaService citaService, IMedicoService medicoService)
        {
            _citaService = citaService;
            _medicoService = medicoService;
        }

        // =======================================================
        // DASHBOARD PRINCIPAL
        // =======================================================
        public async Task<IActionResult> Dashboard()
        {
            // Usar ClaimTypes.NameIdentifier
            int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            if (idUsuario == 0)
            {
                TempData["Error"] = "No se pudo identificar al usuario.";
                return RedirectToAction("Login", "Acceso");
            }

            // Obtener el médico asociado al usuario logueado
            var medico = await _medicoService.ObtenerPorUsuario(idUsuario);

            if (medico == null)
            {
                TempData["Error"] = "No se encontró información del médico asociada a este usuario.";
                return RedirectToAction("Login", "Acceso");
            }

            // Información del médico
            ViewBag.NombreMedico = medico.NombreCompleto;
            ViewBag.Especialidad = medico.Especialidad?.NombreEspec ?? "General";
            ViewBag.CMP = medico.CMP;

            // Obtener citas del médico
            var todasLasCitas = await _citaService.ListarCitasPorMedico(medico.IdMedico);

            // Estadísticas
            ViewBag.TotalCitas = todasLasCitas.Count;
            ViewBag.CitasHoy = todasLasCitas.Count(c => c.FechaCita.Date == DateTime.Today);
            ViewBag.CitasPendientes = todasLasCitas.Count(c => (c.Estado == "P" || c.Estado == "Pendiente") && c.FechaCita >= DateTime.Now);
            ViewBag.CitasAtendidas = todasLasCitas.Count(c => c.Estado == "C" || c.Estado == "Completada" || c.Estado == "Atendida");

            // Mostrar solo las citas de hoy en el dashboard
            var citasHoy = todasLasCitas
                .Where(c => c.FechaCita.Date == DateTime.Today)
                .OrderBy(c => c.FechaCita)
                .ToList();

            return View(citasHoy);
        }

        // =======================================================
        // TODAS LAS CITAS DEL MÉDICO
        // =======================================================
        public async Task<IActionResult> MisCitas(DateTime? fechaFiltro, string estado)
        {
            int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            if (idUsuario == 0)
            {
                TempData["Error"] = "No se pudo identificar al usuario.";
                return RedirectToAction("Dashboard");
            }

            var medico = await _medicoService.ObtenerPorUsuario(idUsuario);

            if (medico == null)
            {
                TempData["Error"] = "No se encontró información del médico.";
                return RedirectToAction("Dashboard");
            }

            ViewBag.NombreMedico = medico.NombreCompleto;

            var citas = await _citaService.ListarCitasPorMedico(medico.IdMedico);

            // Filtro por fecha
            if (fechaFiltro.HasValue)
            {
                citas = citas.Where(c => c.FechaCita.Date == fechaFiltro.Value.Date).ToList();
                ViewBag.FechaFiltro = fechaFiltro.Value.ToString("yyyy-MM-dd");
            }

            // Filtro por estado
            if (!string.IsNullOrEmpty(estado))
            {
                citas = citas.Where(c => c.Estado == estado).ToList();
                ViewBag.EstadoFiltro = estado;
            }

            // Ordenar por fecha descendente
            citas = citas.OrderByDescending(c => c.FechaCita).ToList();

            return View(citas);
        }

        // =======================================================
        // ATENDER CITA (Marcar como completada)
        // =======================================================
        [HttpPost]
        public async Task<IActionResult> AtenderCita(int idCita)
        {
            bool resultado = await _citaService.ActualizarEstadoCita(idCita, "Completada");

            if (resultado)
            {
                TempData["Exito"] = "✅ Cita marcada como atendida exitosamente.";
            }
            else
            {
                TempData["Error"] = "❌ No se pudo actualizar el estado de la cita.";
            }

            return RedirectToAction("Dashboard");
        }

        // =======================================================
        // CANCELAR CITA
        // =======================================================
        [HttpPost]
        public async Task<IActionResult> CancelarCita(int idCita)
        {
            bool resultado = await _citaService.CancelarCita(idCita);

            if (resultado)
            {
                TempData["Exito"] = "✅ Cita cancelada exitosamente.";
            }
            else
            {
                TempData["Error"] = "❌ No se pudo cancelar la cita.";
            }

            return RedirectToAction("MisCitas");
        }

        // =======================================================
        // MI PERFIL
        // =======================================================
        public async Task<IActionResult> MiPerfil()
        {
            int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            if (idUsuario == 0)
            {
                return RedirectToAction("Dashboard");
            }

            var medico = await _medicoService.ObtenerPorUsuario(idUsuario);

            if (medico == null)
            {
                TempData["Error"] = "No se encontró información del médico.";
                return RedirectToAction("Dashboard");
            }

            return View(medico);
        }

        // =======================================================
        // HORARIOS (Vista futura)
        // =======================================================
        public IActionResult MisHorarios()
        {
            return View();
        }
    }
}
