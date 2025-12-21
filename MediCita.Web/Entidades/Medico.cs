namespace MediCita.Web.Entidades
{
    public class Medico
    {
        public int IdMedico { get; set; }
        public string NombreCompleto { get; set; }
        public int IdEspecialidad { get; set; }
        public Especialidad? Especialidad { get; set; } // Navegación a la entidad Especialidad
        public string CMP { get; set; } // Colegio Médico
        public string Correo { get; set; }
        public string Telefono { get; set; }
    }
}