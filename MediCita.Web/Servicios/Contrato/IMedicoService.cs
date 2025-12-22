using MediCita.Web.Entidades;

namespace MediCita.Web.Servicios.Contrato
{
    public interface IMedicoService
    {
        Task<List<Medico>> Listar();
        Task<Medico?> Obtener(int id);
        Task<Medico?> ObtenerPorUsuario(int idUsuario); // ✅ AGREGADO
        Task<bool> Guardar(Medico modelo);
        Task<bool> Editar(Medico modelo);
        Task<bool> Eliminar(int id);
    }
}
