namespace MediCita.Web.Entidades
{
    public class Medico
    {
        public int IdMedico { get; set; }
        public int IdUsuario { get; set; } 
        public int IdEspecialidad { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string CMP { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;

        
        public Especialidad? Especialidad { get; set; }
    }
}
