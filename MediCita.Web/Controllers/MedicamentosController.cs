using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediCita.Web.Entidades;
using MediCita.Web.Servicios.Contrato;

namespace MediCita.Web.Controllers
{
    [Authorize] // Solo usuarios logueados pueden entrar aquí
    public class MedicamentosController : Controller
    {
        private readonly IMedicamentoService _servicio;

        public MedicamentosController(IMedicamentoService servicio)
        {
            _servicio = servicio;
        }

        // GET: Listar todos
        public async Task<IActionResult> Index()
        {
            var lista = await _servicio.Listar();
            return View(lista);
        }

        // GET: Mostrar formulario de Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Guardar el nuevo medicamento
        [HttpPost]
        public async Task<IActionResult> Crear(Medicamento modelo)
        {
            if (!ModelState.IsValid)
                return View(modelo);

            bool respuesta = await _servicio.Guardar(modelo);

            if (respuesta)
                return RedirectToAction("Index");
            else
            {
                ViewData["Mensaje"] = "No se pudo crear el medicamento.";
                return View(modelo);
            }
        }

        // GET: Mostrar formulario de Editar con los datos cargados
        public async Task<IActionResult> Editar(int id)
        {
            Medicamento modelo = await _servicio.Obtener(id);
            return View(modelo);
        }

        // POST: Guardar los cambios
        [HttpPost]
        public async Task<IActionResult> Editar(Medicamento modelo)
        {
            if (!ModelState.IsValid)
                return View(modelo);

            bool respuesta = await _servicio.Editar(modelo);

            if (respuesta)
                return RedirectToAction("Index");
            else
            {
                ViewData["Mensaje"] = "No se pudo actualizar.";
                return View(modelo);
            }
        }

        // GET: Eliminar (Usaremos un método GET simple por facilidad didáctica)
        public async Task<IActionResult> Eliminar(int id)
        {
            var respuesta = await _servicio.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}