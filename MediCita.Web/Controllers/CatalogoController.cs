using Microsoft.AspNetCore.Mvc;
using MediCita.Web.Servicios.Contrato;
using MediCita.Web.Entidades;

namespace MediCita.Web.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly IMedicamentoService _medicamentoService;
        private readonly IMedicoService _medicoService;
        private readonly IEspecialidadService _especialidadService;

        public CatalogoController(
            IMedicamentoService medicamentoService, 
            IMedicoService medicoService,
            IEspecialidadService especialidadService)
        {
            _medicamentoService = medicamentoService;
            _medicoService = medicoService;
            _especialidadService = especialidadService;
        }

        // GET: Catálogo de Medicamentos (Botica)
        public async Task<IActionResult> Medicamentos()
        {
            try
            {
                var medicamentos = await _medicamentoService.Listar();
                return View(medicamentos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar el catálogo: " + ex.Message;
                return View(new List<Medicamento>());
            }
        }

        // GET: Catálogo de Médicos (con filtro opcional por especialidad)
        public async Task<IActionResult> Medicos(string especialidad = null)
        {
            try
            {
                var medicos = await _medicoService.Listar();
                
                // Filtrar por especialidad si se proporciona
                if (!string.IsNullOrEmpty(especialidad))
                {
                    medicos = medicos.Where(m => m.Especialidad != null && 
                                                  m.Especialidad.NombreEspec.Equals(especialidad, StringComparison.OrdinalIgnoreCase))
                                     .ToList();
                    ViewBag.EspecialidadFiltro = especialidad;
                }
                
                // Obtener especialidades únicas - enfoque simple
                var listaEspecialidades = new List<string>();
                var todosLosMedicos = await _medicoService.Listar();
                if (todosLosMedicos != null)
                {
                    foreach (var m in todosLosMedicos)
                    {
                        if (m != null && m.Especialidad != null)
                        {
                            var nombreEsp = m.Especialidad.NombreEspec;
                            if (nombreEsp != null && !listaEspecialidades.Contains(nombreEsp))
                            {
                                listaEspecialidades.Add(nombreEsp);
                            }
                        }
                    }
                    listaEspecialidades.Sort();
                }
                
                ViewBag.Especialidades = listaEspecialidades;
                return View(medicos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los médicos: " + ex.Message;
                ViewBag.Especialidades = new List<string>();
                return View(new List<Medico>());
            }
        }

        // GET: Catálogo de Especialidades
        public async Task<IActionResult> Especialidades()
        {
            try
            {
                var especialidades = await _especialidadService.Listar();
                
                // Obtener cantidad de médicos por especialidad
                var medicos = await _medicoService.Listar();
                var medicosPorEspecialidad = new Dictionary<int, int>();
                
                foreach (var esp in especialidades)
                {
                    var cantidad = medicos.Count(m => m.IdEspecialidad == esp.IdEspecialidad);
                    medicosPorEspecialidad[esp.IdEspecialidad] = cantidad;
                }
                
                ViewBag.MedicosPorEspecialidad = medicosPorEspecialidad;
                return View(especialidades);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar las especialidades: " + ex.Message;
                ViewBag.MedicosPorEspecialidad = new Dictionary<int, int>();
                return View(new List<Especialidad>());
            }
        }
    }
}
