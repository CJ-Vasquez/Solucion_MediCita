using MediCita.Web.Entidades;

namespace MediCita.Web.Servicios.Contrato
{
    public interface IUsuarioService
    {
        // Validar usuario existente (Login)
        Task<Usuario> ValidarUsuario(string correo, string clave);
        
        // Registrar nuevo usuario (Cliente/Paciente)
        Task<bool> RegistrarCliente(Usuario usuario);
        
        // Verificar si el correo ya existe
        Task<bool> ExisteCorreo(string correo);
    }
}
