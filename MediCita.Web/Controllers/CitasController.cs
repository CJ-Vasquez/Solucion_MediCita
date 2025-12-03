using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediCita.Web.Entidades;
using MediCita.Web.Servicios.Contrato;
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

        // PASO 1: Elegir Especialidad (Consume tu API)
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
                    lista = JsonConvert.DeserializeObject<List<Especialidad>>(apiResponse);
                }
            }
            return View(lista);
        }

        // PASO 2: Elegir Médico (Recibe el ID de la especialidad)
        public async Task<IActionResult> SeleccionarMedico(int idEspecialidad, string nombreEspecialidad)
        {
            ViewBag.Especialidad = nombreEspecialidad;
            var medicos = await _citaService.ListarMedicos(idEspecialidad);
            return View(medicos);
        }

        // PASO 3: Confirmar Fecha (Vista GET)
        public IActionResult Reservar(int idMedico, string nombreMedico, string especialidad)
        {
            Cita modelo = new Cita()
            {
                IdMedico = idMedico,
                NombreMedico = nombreMedico,
                NombreEspecialidad = especialidad,
                FechaCita = DateTime.Now.AddDays(1) // Sugerimos mañana por defecto
            };
            return View(modelo);
        }

        // PASO 4: Guardar Cita (POST)
        [HttpPost]
        public async Task<IActionResult> Reservar(Cita modelo)
        {
            // Asignamos el ID del usuario logueado (Paciente)
            // En un caso real se obtiene de los Claims. Usaremos 1 (Admin) como ejemplo.
            modelo.IdPaciente = 1;

            bool resultado = await _citaService.RegistrarCita(modelo);

            if (resultado)
            {
                TempData["Exito"] = "¡Cita reservada con éxito!";
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["Mensaje"] = "Error al reservar.";
                return View(modelo);
            }
        }
        // GET: Mis Citas (Reporte con Filtro y Paginación)
        public async Task<IActionResult> MisCitas(DateTime? fechaFiltro, int p = 1)
        {
            // 1. Obtener ID del usuario logueado (Usamos 1 como ejemplo fijo si no tienes Claims completos)
            int idPaciente = 1;

            // 2. Traer TODA la lista del servicio
            var listaCompleta = await _citaService.ReporteCitasUsuario(idPaciente);

            // 3. Aplicar FILTRO por fecha (Si el usuario seleccionó una)
            if (fechaFiltro.HasValue)
            {
                listaCompleta = listaCompleta
                                .Where(x => x.FechaCita.Date == fechaFiltro.Value.Date)
                                .ToList();
            }

            // 4. Aplicar PAGINACIÓN (5 registros por página)
            int registrosPorPagina = 5;
            int totalRegistros = listaCompleta.Count();

            // Calculamos cuántas páginas salen (Fórmula del Lab 2.3)
            int totalPaginas = (totalRegistros % registrosPorPagina == 0) ?
                                totalRegistros / registrosPorPagina :
                                (totalRegistros / registrosPorPagina) + 1;

            // Pasamos datos a la Vista para dibujar los botones
            ViewBag.PaginaActual = p;
            ViewBag.TotalPaginas = totalPaginas;
            ViewBag.FechaFiltro = fechaFiltro?.ToString("yyyy-MM-dd"); // Para mantener el filtro en la caja de texto

            // Cortamos la lista usando Skip y Take
            var listaPaginada = listaCompleta.Skip((p - 1) * registrosPorPagina).Take(registrosPorPagina).ToList();

            return View(listaPaginada);
        }
    }
}