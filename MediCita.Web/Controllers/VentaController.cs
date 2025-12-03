using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediCita.Web.Entidades;
using MediCita.Web.Servicios.Contrato;
using MediCita.Web.Utilidades; // Para usar nuestra extensión de sesión
using System.Security.Claims; // Para leer el ID del usuario logueado

namespace MediCita.Web.Controllers
{
    [Authorize]
    public class VentaController : Controller
    {
        private readonly IMedicamentoService _medicamentoService;
        private readonly IVentaService _ventaService;

        public VentaController(IMedicamentoService medicamentoService, IVentaService ventaService)
        {
            _medicamentoService = medicamentoService;
            _ventaService = ventaService;
        }

        // 1. Catálogo de Productos (Para elegir)
        public async Task<IActionResult> Catalogo()
        {
            var lista = await _medicamentoService.Listar();
            return View(lista);
        }

        // 2. Agregar producto al Carrito (Acción oculta)
        [HttpPost]
        public async Task<IActionResult> AgregarCarrito(int idMedicamento, int cantidad)
        {
            // Obtenemos los datos reales del producto
            var producto = await _medicamentoService.Obtener(idMedicamento);
            if (producto == null) return NotFound();

            // Recuperamos el carrito actual de la sesión (o creamos uno nuevo)
            List<DetalleVenta> carrito = HttpContext.Session.GetObject<List<DetalleVenta>>("CarritoCompra");
            if (carrito == null) carrito = new List<DetalleVenta>();

            // Creamos el item
            DetalleVenta item = new DetalleVenta()
            {
                IdMedicamento = producto.IdMedicamento,
                NombreMedicamento = producto.Nombre,
                Precio = producto.Precio,
                Cantidad = cantidad,
                Total = producto.Precio * cantidad
            };

            // Lo agregamos y guardamos en sesión
            carrito.Add(item);
            HttpContext.Session.SetObject("CarritoCompra", carrito);

            TempData["Mensaje"] = "Producto agregado al carrito";
            return RedirectToAction("Catalogo");
        }

        // 3. Ver el Carrito (Resumen)
        public IActionResult Carrito()
        {
            List<DetalleVenta> carrito = HttpContext.Session.GetObject<List<DetalleVenta>>("CarritoCompra");
            if (carrito == null) carrito = new List<DetalleVenta>();

            return View(carrito);
        }

        // 4. Procesar la Venta (Transacción Final)
        [HttpPost]
        public async Task<IActionResult> TerminarVenta()
        {
            List<DetalleVenta> carrito = HttpContext.Session.GetObject<List<DetalleVenta>>("CarritoCompra");
            if (carrito == null || carrito.Count == 0) return RedirectToAction("Catalogo");

            // Armamos el objeto Venta Maestra
            Venta venta = new Venta()
            {
                // En un caso real, obtenemos el ID del usuario logueado desde los Claims. 
                // Como ejemplo usaremos 1 (Admin) si no lo encuentras.
                IdUsuario = 1,
                Total = carrito.Sum(x => x.Total),
                Detalles = carrito
            };

            bool respuesta = await _ventaService.Registrar(venta);

            if (respuesta)
            {
                // Limpiamos el carrito
                HttpContext.Session.Remove("CarritoCompra");
                TempData["Exito"] = "¡Compra registrada correctamente! ID de transacción generado.";
                return RedirectToAction("Catalogo");
            }
            else
            {
                TempData["Error"] = "Ocurrió un error al procesar la venta. Intente de nuevo.";
                return RedirectToAction("Carrito");
            }
        }
    }
}