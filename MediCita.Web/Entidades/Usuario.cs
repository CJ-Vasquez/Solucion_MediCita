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

        // Propiedad adicional para mostrar el nombre del rol (esto no esta en la tabla, pero sirve para listar)
        public string? NombreRol { get; set; }
    
    }

}
