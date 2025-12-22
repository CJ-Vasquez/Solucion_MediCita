using Microsoft.AspNetCore.Mvc;
using MediCita.Web.Entidades;
using MediCita.Web.Servicios.Contrato;

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

        // Botica pública
        public async Task<IActionResult> Medicamentos()
        {
            try
            {
                var lista = await _medicamentoService.Listar();
                return View("Medicamentos", lista);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar medicamentos: " + ex.Message;
                return View("Medicamentos", new List<Medicamento>());
            }
        }

        // Compatibilidad con enlaces antiguos
        public IActionResult Catalogo() => RedirectToAction(nameof(Medicamentos));

        // Médicos
        public async Task<IActionResult> Medicos(string? especialidad = null)
        {
            try
            {
                var medicos = await _medicoService.Listar();

                if (!string.IsNullOrEmpty(especialidad))
                {
                    medicos = medicos
                        .Where(m => (m.Especialidad?.NombreEspec ?? "").Equals(especialidad, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    ViewBag.EspecialidadFiltro = especialidad;
                }

                var especialidades = await _especialidadService.Listar();
                ViewBag.Especialidades = especialidades.Select(e => e.NombreEspec).ToList();
                return View(medicos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar médicos: " + ex.Message;
                return View(new List<Medico>());
            }
        }

        // Especialidades
        public async Task<IActionResult> Especialidades()
        {
            try
            {
                var especialidades = await _especialidadService.Listar();
                return View(especialidades);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar especialidades: " + ex.Message;
                return View(new List<Especialidad>());
            }
        }
    }
}
