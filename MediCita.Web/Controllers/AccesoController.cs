using Microsoft.AspNetCore.Mvc;
using MediCita.Web.Entidades;
using MediCita.Web.Servicios.Contrato;

// Librerías obligatorias para la autenticación por Cookies
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

        // GET: Acceso/Login - Solo para acceso directo (opcional)
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            // Si alguien accede directamente, mostrar la página de login completa
            return View();
        }

        // POST: Procesar el inicio de sesión (desde modal o página)
        [HttpPost]
        public async Task<IActionResult> Login(string correo, string clave, string returnUrl = null)
        {
            // 1. Validar credenciales con el servicio (Base de Datos)
            Usuario usuario_encontrado = await _usuarioService.ValidarUsuario(correo, clave);

            // 2. Si no existe el usuario, devolvemos error
            if (usuario_encontrado == null)
            {
                TempData["ErrorLogin"] = "Correo o contraseña incorrectos. Por favor, intente de nuevo.";
                
                // Si viene de un modal, redirigir a la página anterior
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                
                // Si es acceso directo, volver a la vista de login
                ViewData["Mensaje"] = "Correo o contraseña incorrectos. Por favor, intente de nuevo.";
                return View();
            }

            // 3. Si existe, creamos la identidad del usuario (Carnet virtual)
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario_encontrado.NombreCompleto),
                new Claim(ClaimTypes.Email, usuario_encontrado.Correo),
                new Claim(ClaimTypes.Role, usuario_encontrado.NombreRol),
                new Claim("IdUsuario", usuario_encontrado.IdUsuario.ToString())
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true
            };

            // 4. Guardar la Cookie de sesión en el navegador
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            // 5. Mensaje de éxito
            TempData["Success"] = $"¡Bienvenido de vuelta, {usuario_encontrado.NombreCompleto}!";

            // 6. Redirigir al Dashboard
            return RedirectToAction("Dashboard", "Admin");
        }

        // GET: Acceso/Registrarse - Vista completa (opcional)
        public IActionResult Registrarse()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            return View();
        }

        // POST: Registrar nuevo usuario (desde modal o página)
        [HttpPost]
        public async Task<IActionResult> Registrarse(string nombreCompleto, string dni, string correo, 
                                                      string telefono, string clave, string confirmarClave, 
                                                      string returnUrl = null)
        {
            // Validaciones básicas
            if (string.IsNullOrEmpty(nombreCompleto) || string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(clave))
            {
                TempData["ErrorRegistro"] = "Por favor, complete todos los campos obligatorios.";
                return string.IsNullOrEmpty(returnUrl) ? View() : Redirect(returnUrl);
            }

            if (clave != confirmarClave)
            {
                TempData["ErrorRegistro"] = "Las contraseñas no coinciden.";
                return string.IsNullOrEmpty(returnUrl) ? View() : Redirect(returnUrl);
            }

            if (clave.Length < 6)
            {
                TempData["ErrorRegistro"] = "La contraseña debe tener al menos 6 caracteres.";
                return string.IsNullOrEmpty(returnUrl) ? View() : Redirect(returnUrl);
            }

            // TODO: Aquí deberías implementar la lógica de registro real
            // Por ejemplo:
            // 1. Verificar que el correo no exista
            // 2. Hashear la contraseña
            // 3. Guardar en la base de datos
            // 4. Enviar email de confirmación

            // Por ahora, simulamos un registro exitoso
            TempData["SuccessRegistro"] = "¡Cuenta creada exitosamente! Por favor, inicia sesión.";
            TempData["Info"] = "El sistema de registro estará disponible próximamente. Por ahora puedes usar las credenciales de prueba.";
            
            return string.IsNullOrEmpty(returnUrl) ? RedirectToAction("Login") : Redirect(returnUrl);
        }

        // GET: Cerrar Sesión
        public async Task<IActionResult> Salir()
        {
            // Borramos la cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Limpiamos la sesión (por si quedó algo del carrito)
            HttpContext.Session.Clear();

            // Mensaje de despedida
            TempData["Info"] = "Has cerrado sesión correctamente. ¡Hasta pronto!";

            // Redirigimos a la página pública
            return RedirectToAction("Index", "Home");
        }
    }
}