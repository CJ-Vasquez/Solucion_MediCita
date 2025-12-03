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

        // GET: Acceso/Login
        // Como ahora el login es un Modal en el Home, si alguien intenta entrar
        // directamente por URL a /Acceso/Login, lo redirigimos al Home.
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Procesar el inicio de sesión
        [HttpPost]
        public async Task<IActionResult> Login(string correo, string clave)
        {
            // 1. Validar credenciales con el servicio (Base de Datos)
            Usuario usuario_encontrado = await _usuarioService.ValidarUsuario(correo, clave);

            // 2. Si no existe el usuario, devolvemos error al Modal
            if (usuario_encontrado == null)
            {
                // Usamos TempData porque sobrevive a la redirección
                TempData["ErrorLogin"] = "Correo o contraseña incorrectos. Intente de nuevo.";

                // Redirigimos al Home (donde vive el Modal)
                return RedirectToAction("Index", "Home");
            }

            // 3. Si existe, creamos la identidad del usuario (Carnet virtual)
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario_encontrado.NombreCompleto),
                new Claim(ClaimTypes.Email, usuario_encontrado.Correo),
                new Claim(ClaimTypes.Role, usuario_encontrado.NombreRol),
                // Puedes guardar el ID del usuario también si lo necesitas luego
                new Claim("IdUsuario", usuario_encontrado.IdUsuario.ToString())
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true // Mantiene la sesión abierta aunque cierres el navegador
            };

            // 4. Guardar la Cookie de sesión en el navegador
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            // 5. Redirigir al Panel Administrativo
            return RedirectToAction("Dashboard", "Admin");
        }

        // GET: Cerrar Sesión
        public async Task<IActionResult> Salir()
        {
            // Borramos la cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Limpiamos la sesión (por si quedó algo del carrito)
            HttpContext.Session.Clear();

            // Redirigimos a la página pública
            return RedirectToAction("Index", "Home");
        }
    }
}