using MediCita.Web.Entidades;

namespace MediCita.Web.Servicios.Contrato
{
    public interface IUsuarioService
    {
        // Definimos el método que usaremos. 
        // Usamos Task<> para que sea asíncrono (async/await), como piden las buenas prácticas modernas.
        Task<Usuario> ValidarUsuario(string correo, string clave);
    }
}
