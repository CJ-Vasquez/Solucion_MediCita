using MediCita.Web.Entidades;

namespace MediCita.Web.Servicios.Contrato
{
    public interface IUsuarioService
    {
        // Métodos existentes
        Task<Usuario> ValidarUsuario(string correo, string clave);
        Task<bool> RegistrarCliente(Usuario usuario);
        Task<bool> ExisteCorreo(string correo);

        // Nuevos métodos para CRUD Admin
        Task<List<Usuario>> Listar();
        Task<Usuario> Obtener(int id);
        Task<bool> Guardar(Usuario modelo);
        Task<bool> Editar(Usuario modelo);
        Task<bool> Eliminar(int id);
    }
}
