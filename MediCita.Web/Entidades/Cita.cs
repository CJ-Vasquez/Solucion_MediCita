using System.ComponentModel.DataAnnotations;

namespace MediCita.Web.Entidades
{
    public class Cita
    {
        public int IdCita { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public DateTime FechaCita { get; set; }
        public string? Estado { get; set; }
        public string? NombreMedico { get; set; }
        public string? NombreEspecialidad { get; set; }
        public string? NombrePaciente { get; set; } // Agregado para corregir CS011

    }
}