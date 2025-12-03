using System.ComponentModel.DataAnnotations;

namespace MediCita.Web.Entidades
{
    public class Cita
    {
        public int IdCita { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        [Required]
        public DateTime FechaCita { get; set; }
        public string? Estado { get; set; }

        // Datos auxiliares para mostrar en la vista
        public string? NombreMedico { get; set; }
        public string? NombreEspecialidad { get; set; }
    }
}