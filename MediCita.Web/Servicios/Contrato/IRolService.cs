using MediCita.Web.Entidades;

namespace MediCita.Web.Servicios.Contrato
{
    public interface IRolService
    {
        Task<List<Rol>> Listar();
    }
}
