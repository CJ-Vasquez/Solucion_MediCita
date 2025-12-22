using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediCita.Web.Entidades;
using MediCita.Web.Servicios.Contrato;
using System.Security.Claims;
using Newtonsoft.Json;

namespace MediCita.Web.Controllers
{
    [Authorize]
    public class CitasController : Controller
    {
        private readonly ICitaService _citaService;

        public CitasController(ICitaService citaService)
        {
            _citaService = citaService;
        }

        // ========================================
        // PASO 1: Elegir Especialidad
        // ========================================
        public async Task<IActionResult> Index()
        {
            List<Especialidad> lista = new List<Especialidad>();
            using (var httpClient = new HttpClient())
            {
                var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}/api/Especialidades";
                var response = await httpClient.GetAsync(baseUrl);

                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    lista = JsonConvert.DeserializeObject<List<Especialidad>>(apiResponse) ?? new List<Especialidad>();
                }
            }
            return View(lista);
        }

        // ========================================
        // PASO 2: Elegir Médico
        // ========================================
        public async Task<IActionResult> SeleccionarMedico(int idEspecialidad, string nombreEspecialidad)
        {
            ViewBag.Especialidad = nombreEspecialidad;
            var medicos = await _citaService.ListarMedicos(idEspecialidad);
            return View(medicos);
        }

        // ========================================
        // PASO 3: Confirmar Fecha (GET)
        // ========================================
        public IActionResult Reservar(int idMedico, string nombreMedico, string especialidad)
        {
            Cita modelo = new Cita()
            {
                IdMedico = idMedico,
                NombreMedico = nombreMedico,
                NombreEspecialidad = especialidad,
                FechaCita = DateTime.Now.AddDays(1)
            };
            return View(modelo);
        }

        // ========================================
        // PASO 4: Guardar Cita (POST)
        // ========================================
        [HttpPost]
        public async Task<IActionResult> Reservar(Cita modelo)
        {
            // Obtener IdUsuario del usuario logueado
            int idPaciente = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            if (idPaciente == 0)
            {
                return RedirectToAction("Login", "Acceso");
            }

            // Validar que no exista una cita duplicada (mismo médico, misma fecha/hora, estado Pendiente)
            var citasExistentes = await _citaService.ReporteCitasUsuario(idPaciente);
            bool citaDuplicada = citasExistentes.Any(c =>
                c.IdMedico == modelo.IdMedico &&
                c.FechaCita.Date == modelo.FechaCita.Date &&
                c.FechaCita.Hour == modelo.FechaCita.Hour &&
                (c.Estado == "P" || c.Estado == "Pendiente")
            );

            if (citaDuplicada)
            {
                TempData["Error"] = "⚠️ Ya tienes una cita programada con este médico en esa fecha y hora.";
                return View(modelo);
            }

            modelo.IdPaciente = idPaciente;
            bool resultado = await _citaService.RegistrarCita(modelo);

            if (resultado)
            {
                TempData["Exito"] = "✅ ¡Cita reservada con éxito! Te esperamos en la fecha programada.";
                return RedirectToAction("Dashboard", "Cliente"); // Redirigir al Dashboard
            }
            else
            {
                TempData["Error"] = "❌ Error al reservar la cita. Intente nuevamente.";
                return View(modelo);
            }
        }

        // ========================================
        // Cancelar Cita
        // ========================================
        [HttpPost]
        public async Task<IActionResult> CancelarCita(int idCita)
        {
            bool resultado = await _citaService.CancelarCita(idCita);

            if (resultado)
            {
                TempData["Exito"] = "✅ Cita cancelada correctamente.";
            }
            else
            {
                TempData["Error"] = "❌ No se pudo cancelar la cita.";
            }

            return RedirectToAction("MisCitas", "Cliente");
        }
    }
}
