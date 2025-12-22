using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediCita.Web.Entidades;
using MediCita.Web.Servicios.Contrato;
using MediCita.Web.Utilidades;
using System.Security.Claims;

namespace MediCita.Web.Controllers
{
    public class VentaController : Controller
    {
        private readonly IMedicamentoService _medicamentoService;
        private readonly IVentaService _ventaService;

        public VentaController(IMedicamentoService medicamentoService, IVentaService ventaService)
        {
            _medicamentoService = medicamentoService;
            _ventaService = ventaService;
        }

        // 1. Catálogo con "Resta Visual" de Stock
        public async Task<IActionResult> Catalogo()
        {
            // A. Traemos el stock real de la Base de Datos
            var lista = await _medicamentoService.Listar();

            // B. Traemos lo que ya tienes en el carrito
            List<DetalleVenta>? carrito = HttpContext.Session.GetObject<List<DetalleVenta>>("CarritoCompra");

            // C. MATEMÁTICA: Si tienes productos en el carrito, se los restamos a la vista
            if (carrito != null && carrito.Count > 0)
            {
                foreach (var producto in lista)
                {
                    // Buscamos si este producto ya está en tu carrito
                    var itemEnCarrito = carrito.FirstOrDefault(x => x.IdMedicamento == producto.IdMedicamento);

                    if (itemEnCarrito != null)
                    {
                        // Restamos visualmente para que veas cuánto stock QUEDA realmente
                        producto.Stock = producto.Stock - itemEnCarrito.Cantidad;

                        // Protección para que no salgan negativos
                        if (producto.Stock < 0) producto.Stock = 0;
                    }
                }
            }

            return View(lista);
        }

        // 2. Agregar al Carrito (Con lógica de suma y contador)
        [HttpPost]
        public async Task<IActionResult> AgregarCarrito(int idMedicamento, int cantidad)
        {
            var producto = await _medicamentoService.Obtener(idMedicamento);
            if (producto == null) return NotFound();

            // Recuperar carrito
            List<DetalleVenta>? carrito = HttpContext.Session.GetObject<List<DetalleVenta>>("CarritoCompra");
            if (carrito == null) carrito = new List<DetalleVenta>();

            // Validar si ya existe el producto en el carrito para no repetir filas
            var itemExistente = carrito.FirstOrDefault(x => x.IdMedicamento == idMedicamento);

            // Validar que no superes el stock real
            int cantidadActualEnCarrito = itemExistente != null ? itemExistente.Cantidad : 0;
            if (cantidadActualEnCarrito + cantidad > producto.Stock)
            {
                TempData["Error"] = $"Stock insuficiente. Solo quedan {producto.Stock} unidades.";
                return Redirect(Request.Headers["Referer"].ToString());
            }

            if (itemExistente != null)
            {
                // SI YA EXISTE: Solo aumentamos la cantidad
                itemExistente.Cantidad += cantidad;
                itemExistente.SubTotal = itemExistente.PrecioUnitario * itemExistente.Cantidad;
            }
            else
            {
                // SI ES NUEVO: Creamos el item
                DetalleVenta item = new DetalleVenta()
                {
                    IdMedicamento = producto.IdMedicamento,
                    PrecioUnitario = producto.Precio,
                    Cantidad = cantidad,
                    SubTotal = producto.Precio * cantidad
                };
                carrito.Add(item);
            }

            // ✅ GUARDAR CARRITO Y CONTADOR
            HttpContext.Session.SetObject("CarritoCompra", carrito);
            HttpContext.Session.SetInt32("CantidadCarrito", carrito.Sum(x => x.Cantidad));

            TempData["Mensaje"] = "¡Agregado al carrito!";

            // Volvemos a la misma página
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // 3. Ver Carrito
        public IActionResult Carrito()
        {
            List<DetalleVenta>? carrito = HttpContext.Session.GetObject<List<DetalleVenta>>("CarritoCompra");
            if (carrito == null) carrito = new List<DetalleVenta>();

            return View(carrito);
        }

        // 4. Terminar Venta (Requiere Login)
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> TerminarVenta()
        {
            List<DetalleVenta>? carrito = HttpContext.Session.GetObject<List<DetalleVenta>>("CarritoCompra");
            if (carrito == null || carrito.Count == 0) return RedirectToAction("Catalogo");

            int idPaciente = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            if (idPaciente == 0)
            {
                TempData["Error"] = "Debe iniciar sesión para completar la compra.";
                return RedirectToAction("Login", "Acceso");
            }

            Venta venta = new Venta()
            {
                IdPaciente = idPaciente,
                Total = carrito.Sum(x => x.SubTotal),
                Detalles = carrito
            };

            bool respuesta = await _ventaService.Registrar(venta);

            if (respuesta)
            {
                // Limpiamos carrito y contador
                HttpContext.Session.Remove("CarritoCompra");
                HttpContext.Session.Remove("CantidadCarrito");

                TempData["Exito"] = "¡Compra registrada correctamente!";
                return RedirectToAction("Catalogo");
            }
            else
            {
                TempData["Error"] = "Ocurrió un error al procesar la venta.";
                return RedirectToAction("Carrito");
            }
        }

        // ✅ NUEVO: Alias para ProcesarCompra (llama a TerminarVenta)
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ProcesarCompra()
        {
            return await TerminarVenta();
        }

        // 5. Eliminar del Carrito
        [HttpPost]
        public IActionResult EliminarDelCarrito(int idMedicamento)
        {
            List<DetalleVenta>? carrito = HttpContext.Session.GetObject<List<DetalleVenta>>("CarritoCompra");

            if (carrito != null)
            {
                var item = carrito.FirstOrDefault(x => x.IdMedicamento == idMedicamento);
                if (item != null)
                {
                    carrito.Remove(item);
                    HttpContext.Session.SetObject("CarritoCompra", carrito);
                    HttpContext.Session.SetInt32("CantidadCarrito", carrito.Sum(x => x.Cantidad));
                    TempData["Mensaje"] = "Producto eliminado del carrito.";
                }
            }

            return RedirectToAction("Carrito");
        }
    }
}
