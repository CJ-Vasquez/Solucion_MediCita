using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediCita.Web.Entidades;
using MediCita.Web.Servicios.Contrato;

namespace MediCita.Web.Controllers
{
    [Authorize]
    public class MedicosController : Controller
    {
        private readonly IMedicoService _medicoService;
        private readonly IEspecialidadService _especialidadService;

        public MedicosController(IMedicoService medicoService, IEspecialidadService especialidadService)
        {
            _medicoService = medicoService;
            _especialidadService = especialidadService;
        }

        // GET: Medicos
        public async Task<IActionResult> Index()
        {
            List<Medico> lista = await _medicoService.Listar();
            return View(lista);
        }

        // GET: Medicos/Crear
        public async Task<IActionResult> Crear()
        {
            ViewBag.Especialidades = await _especialidadService.Listar();
            return View();
        }

        // POST: Medicos/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Medico modelo)
        {
            if (ModelState.IsValid)
            {
                bool respuesta = await _medicoService.Guardar(modelo);
                if (respuesta)
                {
                    TempData["Mensaje"] = "Médico registrado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = "No se pudo registrar el médico";
                }
            }
            ViewBag.Especialidades = await _especialidadService.Listar();
            return View(modelo);
        }

        // GET: Medicos/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            Medico modelo = await _medicoService.Obtener(id);
            ViewBag.Especialidades = await _especialidadService.Listar();
            return View(modelo);
        }

        // POST: Medicos/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Medico modelo)
        {
            if (ModelState.IsValid)
            {
                bool respuesta = await _medicoService.Editar(modelo);
                if (respuesta)
                {
                    TempData["Mensaje"] = "Médico actualizado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = "No se pudo actualizar el médico";
                }
            }
            ViewBag.Especialidades = await _especialidadService.Listar();
            return View(modelo);
        }

        // POST: Medicos/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool respuesta = await _medicoService.Eliminar(id);
            if (respuesta)
            {
                TempData["Mensaje"] = "Médico eliminado exitosamente";
            }
            else
            {
                TempData["Error"] = "No se pudo eliminar el médico";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
