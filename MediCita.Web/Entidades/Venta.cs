namespace MediCita.Web.Entidades
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public int IdUsuario { get; set; } // El paciente que compra
        public decimal Total { get; set; }
        public List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
    }
}