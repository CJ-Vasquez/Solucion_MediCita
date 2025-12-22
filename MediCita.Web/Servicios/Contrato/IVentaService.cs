using MediCita.Web.Entidades;

namespace MediCita.Web.Servicios.Contrato
{
    public interface IVentaService
    {
        Task<bool> Registrar(Venta modelo);
        Task<List<Venta>> ListarVentasPorUsuario(int idUsuario);
        Task<decimal> ObtenerTotalVentasDelDia();
        Task<int> ContarVentasDelDia();
    }
}