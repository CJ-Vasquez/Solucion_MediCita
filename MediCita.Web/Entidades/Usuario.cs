using System.ComponentModel.DataAnnotations;

namespace MediCita.Web.Entidades
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El Nombre es obligatorio")]
        public string? NombreCompleto { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        public string? Correo {  get; set; }
        public string? Clave { get; set; }
        public int IdRol {  get; set; }

        
        public string? NombreRol { get; set; }
    
    }

}
