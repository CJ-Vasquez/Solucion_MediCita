namespace MediCita.Web.Entidades
{
    public class DetalleVenta
    {
        public int IdDetalle { get; set; }
        public int IdVenta { get; set; }
        public int IdMedicamento { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal SubTotal { get; set; }

        
        public decimal Total => SubTotal;

      
        public string NombreMedicamento { get; set; } = string.Empty;
    }
}
