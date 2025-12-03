using System.ComponentModel.DataAnnotations;
namespace MediCita.Web.Entidades
{
    public class Medicamento
    {
        public int IdMedicamento { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El laboratorio es obligatorio")]
        public string Laboratorio { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        public int Stock { get; set; }
    }
}
