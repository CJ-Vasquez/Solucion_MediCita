using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediCita.Web.Entidades;
using MediCita.Web.Servicios.Contrato;

namespace MediCita.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        private readonly IMedicoService _medicoService;
        private readonly IEspecialidadService _especialidadService;
        private readonly IMedicamentoService _medicamentoService;
        private readonly ICitaService _citaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IRolService _rolService;

        public AdminController(
            IMedicoService medicoService,
            IEspecialidadService especialidadService,
            IMedicamentoService medicamentoService,
            ICitaService citaService,
            IUsuarioService usuarioService,
            IRolService rolService)
        {
            _medicoService = medicoService;
            _especialidadService = especialidadService;
            _medicamentoService = medicamentoService;
            _citaService = citaService;
            _usuarioService = usuarioService;
            _rolService = rolService;
        }

        // ========================================
        // DASHBOARD
        // ========================================
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.TotalMedicos = (await _medicoService.Listar()).Count;
            ViewBag.TotalMedicamentos = (await _medicamentoService.Listar()).Count;
            ViewBag.CitasHoy = await _citaService.ContarCitasDelDia();
            var citasPendientes = await _citaService.ListarCitasPendientes();
            return View(citasPendientes);
        }

        // ========================================
        // GESTIÓN DE MÉDICOS
        // ========================================
        public async Task<IActionResult> Medicos()
        {
            var medicos = await _medicoService.Listar();
            return View(medicos);
        }

        public async Task<IActionResult> CrearMedico()
        {
            ViewBag.Especialidades = await _especialidadService.Listar();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearMedico(Medico modelo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Especialidades = await _especialidadService.Listar();
                return View(modelo);
            }

            bool ok = await _medicoService.Guardar(modelo);
            if (ok)
            {
                TempData["Exito"] = "Médico registrado correctamente.";
                return RedirectToAction("Medicos");
            }

            TempData["Error"] = "No se pudo registrar el médico.";
            ViewBag.Especialidades = await _especialidadService.Listar();
            return View(modelo);
        }

        public async Task<IActionResult> EditarMedico(int id)
        {
            var medico = await _medicoService.Obtener(id);
            if (medico == null)
            {
                TempData["Error"] = "Médico no encontrado.";
                return RedirectToAction("Medicos");
            }
            ViewBag.Especialidades = await _especialidadService.Listar();
            return View(medico);
        }

        [HttpPost]
        public async Task<IActionResult> EditarMedico(Medico modelo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Especialidades = await _especialidadService.Listar();
                return View(modelo);
            }

            bool ok = await _medicoService.Editar(modelo);
            if (ok)
            {
                TempData["Exito"] = "Médico actualizado correctamente.";
                return RedirectToAction("Medicos");
            }

            TempData["Error"] = "No se pudo actualizar el médico.";
            ViewBag.Especialidades = await _especialidadService.Listar();
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarMedico(int id)
        {
            bool ok = await _medicoService.Eliminar(id);
            TempData[ok ? "Exito" : "Error"] = ok ? "Médico eliminado." : "No se pudo eliminar.";
            return RedirectToAction("Medicos");
        }

        // ========================================
        // GESTIÓN DE MEDICAMENTOS
        // ========================================
        public async Task<IActionResult> Medicamentos()
        {
            var medicamentos = await _medicamentoService.Listar();
            return View(medicamentos);
        }

        public IActionResult CrearMedicamento()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearMedicamento(Medicamento modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            bool ok = await _medicamentoService.Guardar(modelo);
            if (ok)
            {
                TempData["Exito"] = "Medicamento registrado correctamente.";
                return RedirectToAction("Medicamentos");
            }

            TempData["Error"] = "No se pudo registrar el medicamento.";
            return View(modelo);
        }

        public async Task<IActionResult> EditarMedicamento(int id)
        {
            var medicamento = await _medicamentoService.Obtener(id);
            if (medicamento == null)
            {
                TempData["Error"] = "Medicamento no encontrado.";
                return RedirectToAction("Medicamentos");
            }
            return View(medicamento);
        }

        [HttpPost]
        public async Task<IActionResult> EditarMedicamento(Medicamento modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            bool ok = await _medicamentoService.Editar(modelo);
            if (ok)
            {
                TempData["Exito"] = "Medicamento actualizado correctamente.";
                return RedirectToAction("Medicamentos");
            }

            TempData["Error"] = "No se pudo actualizar el medicamento.";
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarMedicamento(int id)
        {
            bool ok = await _medicamentoService.Eliminar(id);
            TempData[ok ? "Exito" : "Error"] = ok ? "Medicamento eliminado." : "No se pudo eliminar.";
            return RedirectToAction("Medicamentos");
        }

        // ========================================
        // GESTIÓN DE ESPECIALIDADES
        // ========================================
        public async Task<IActionResult> Especialidades()
        {
            var especialidades = await _especialidadService.Listar();
            return View(especialidades);
        }

        // ========================================
        // GESTIÓN DE USUARIOS (CORREGIDO)
        // ========================================
        public async Task<IActionResult> Usuarios()
        {
            var usuarios = await _usuarioService.Listar();
            return View(usuarios);
        }

        public async Task<IActionResult> CrearUsuario()
        {
            ViewBag.Roles = await _rolService.Listar();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearUsuario(Usuario modelo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _rolService.Listar();
                return View(modelo);
            }

            try
            {
                bool ok = await _usuarioService.Guardar(modelo);
                if (ok)
                {
                    TempData["Exito"] = "Usuario registrado correctamente.";
                    return RedirectToAction("Usuarios");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                ViewBag.Roles = await _rolService.Listar();
                return View(modelo);
            }

            TempData["Error"] = "No se pudo registrar el usuario.";
            ViewBag.Roles = await _rolService.Listar();
            return View(modelo);
        }

        public async Task<IActionResult> EditarUsuario(int id)
        {
            var usuario = await _usuarioService.Obtener(id);
            if (usuario == null)
            {
                TempData["Error"] = "Usuario no encontrado.";
                return RedirectToAction("Usuarios");
            }
            ViewBag.Roles = await _rolService.Listar();
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> EditarUsuario(Usuario modelo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _rolService.Listar();
                return View(modelo);
            }

            try
            {
                bool ok = await _usuarioService.Editar(modelo);
                if (ok)
                {
                    TempData["Exito"] = "Usuario actualizado correctamente.";
                    return RedirectToAction("Usuarios");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                ViewBag.Roles = await _rolService.Listar();
                return View(modelo);
            }

            TempData["Error"] = "No se pudo actualizar el usuario.";
            ViewBag.Roles = await _rolService.Listar();
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            try
            {
                bool ok = await _usuarioService.Eliminar(id);
                if (ok)
                {
                    TempData["Exito"] = "Usuario eliminado correctamente.";
                }
                else
                {
                    TempData["Error"] = "No se pudo eliminar el usuario.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Usuarios");
        }
    }
}
