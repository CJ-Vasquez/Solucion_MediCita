namespace MediCita.Web.Entidades
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public int IdPaciente { get; set; } 
        public decimal Total { get; set; }
        public DateTime FechaVenta { get; set; }

        public List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
    }
}
