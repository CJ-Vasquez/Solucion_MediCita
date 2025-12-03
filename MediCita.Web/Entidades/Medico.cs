namespace MediCita.Web.Entidades
{
    public class Medico
    {
        public int IdMedico { get; set; }
        public string NombreCompleto { get; set; }
        public string Especialidad { get; set; } // Para mostrar el nombre
        public string CMP { get; set; } // Colegio Médico
    }
}