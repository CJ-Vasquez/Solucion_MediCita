using Microsoft.AspNetCore.Mvc;
using MediCita.Web.Entidades;
using MediCita.Web.Servicios.Contrato;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace MediCita.Web.Controllers
{
    public class AccesoController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public AccesoController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // ========================================
        // LOGIN (VISTA DEDICADA)
        // ========================================
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                if (User.IsInRole("Administrador")) return RedirectToAction("Dashboard", "Admin");
                if (User.IsInRole("Paciente")) return RedirectToAction("Dashboard", "Cliente");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string correo, string clave, string? returnUrl = null)
        {
            Usuario? usuario_encontrado = await _usuarioService.ValidarUsuario(correo, clave);

            if (usuario_encontrado == null)
            {
                TempData["ErrorLogin"] = "Correo o contraseña incorrectos.";
                ViewData["Mensaje"] = "Correo o contraseña incorrectos.";
                return View();
            }

            await CrearCookieSesion(usuario_encontrado);

            // Si hay returnUrl, redirigir ahí
            if (!string.IsNullOrEmpty(returnUrl)) return Redirect(returnUrl);

            // Redirección según rol
            if (usuario_encontrado.NombreRol == "Administrador")
                return RedirectToAction("Dashboard", "Admin");
            else if (usuario_encontrado.NombreRol == "Paciente")
                return RedirectToAction("Dashboard", "Cliente");

            return RedirectToAction("Index", "Home");
        }

        // ========================================
        // LOGIN (DESDE EL MODAL)
        // ========================================
        [HttpPost]
        public async Task<IActionResult> LoginModal(string correo, string clave)
        {
            Usuario? usuario_encontrado = await _usuarioService.ValidarUsuario(correo, clave);

            if (usuario_encontrado == null)
            {
                TempData["ErrorLogin"] = "Credenciales incorrectas.";
                return Redirect(Request.Headers["Referer"].ToString());
            }

            await CrearCookieSesion(usuario_encontrado);

            // Si tiene carrito, llevarlo directo ahí
            int cantidadCarrito = HttpContext.Session.GetInt32("CantidadCarrito") ?? 0;
            if (cantidadCarrito > 0 && usuario_encontrado.NombreRol == "Paciente")
            {
                return RedirectToAction("Carrito", "Venta");
            }

            // Redirección según rol
            if (usuario_encontrado.NombreRol == "Administrador")
                return RedirectToAction("Dashboard", "Admin");
            else if (usuario_encontrado.NombreRol == "Paciente")
                return RedirectToAction("Dashboard", "Cliente");
            else
                return RedirectToAction("Index", "Home");
        }

        // ========================================
        // REGISTRO DE CLIENTE (DESDE EL MODAL)
        // ========================================
        [HttpPost]
        public async Task<IActionResult> RegistrarCliente(string nombreCompleto, string correo,
            string clave, string confirmarClave)
        {
            try
            {
                // Validaciones básicas
                if (string.IsNullOrEmpty(nombreCompleto) || string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(clave))
                {
                    TempData["ErrorRegistro"] = "Faltan datos obligatorios.";
                    return Redirect(Request.Headers["Referer"].ToString());
                }

                if (clave != confirmarClave)
                {
                    TempData["ErrorRegistro"] = "Las contraseñas no coinciden.";
                    return Redirect(Request.Headers["Referer"].ToString());
                }

                // Verificar si ya existe el correo
                if (await _usuarioService.ExisteCorreo(correo))
                {
                    TempData["ErrorRegistro"] = "El correo ya está registrado.";
                    return Redirect(Request.Headers["Referer"].ToString());
                }

                // Crear nuevo usuario
                Usuario nuevoUsuario = new Usuario()
                {
                    NombreCompleto = nombreCompleto,
                    Correo = correo,
                    Clave = clave,
                    IdRol = 3 // Paciente
                };

                bool respuesta = await _usuarioService.RegistrarCliente(nuevoUsuario);

                if (respuesta)
                {
                    TempData["SuccessRegistro"] = "¡Cuenta creada con éxito! Inicia sesión para continuar.";
                    TempData["MostrarLogin"] = true;
                }
                else
                {
                    TempData["ErrorRegistro"] = "No se pudo crear la cuenta. Intente nuevamente.";
                }

                return Redirect(Request.Headers["Referer"].ToString());
            }
            catch (Exception ex)
            {
                TempData["ErrorRegistro"] = $"Error al registrar: {ex.Message}";
                return Redirect(Request.Headers["Referer"].ToString());
            }
        }

        // ========================================
        // CERRAR SESIÓN
        // ========================================
        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // ========================================
        // MÉTODO AUXILIAR PARA CREAR COOKIES
        // ========================================
        private async Task CrearCookieSesion(Usuario usuario)
        {
            List<Claim> claims = new List<Claim>()
            {
                // ✅ ClaimTypes.NameIdentifier = ID del usuario (lo que busca ClienteController)
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                
                // ✅ Datos del usuario
                new Claim(ClaimTypes.Name, usuario.NombreCompleto ?? "Usuario"),
                new Claim(ClaimTypes.Email, usuario.Correo ?? ""),
                new Claim(ClaimTypes.Role, usuario.NombreRol ?? "Paciente")
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24) // ✅ Cookie válida por 24 horas
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );
        }
    }
}
