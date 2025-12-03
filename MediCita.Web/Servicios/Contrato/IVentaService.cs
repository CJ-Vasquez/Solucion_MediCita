using MediCita.Web.Entidades;

namespace MediCita.Web.Servicios.Contrato
{
    public interface IVentaService
    {
        Task<bool> Registrar(Venta modelo);
    }
}