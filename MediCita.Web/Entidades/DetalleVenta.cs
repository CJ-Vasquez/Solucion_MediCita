namespace MediCita.Web.Entidades
{
    public class DetalleVenta
    {
        public int IdMedicamento { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Total { get; set; }

        // Propiedad auxiliar para mostrar el nombre en la vista
        public string? NombreMedicamento { get; set; }
    }
}