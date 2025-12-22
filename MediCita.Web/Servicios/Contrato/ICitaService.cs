using MediCita.Web.Entidades;

namespace MediCita.Web.Servicios.Contrato
{
    public interface ICitaService
    {
        Task<List<Medico>> ListarMedicos(int idEspecialidad);
        Task<bool> RegistrarCita(Cita modelo);
        Task<List<Cita>> ReporteCitasUsuario(int idUsuario); // ✅ Cambio: idPaciente → idUsuario
        Task<List<Cita>> ListarCitasPorMedico(int idMedico);
        Task<int> ContarCitasDelDia();
        Task<List<Cita>> ListarCitasPendientes();
        Task<bool> ActualizarEstadoCita(int idCita, string nuevoEstado); // ✅ Nuevo
        Task<bool> CancelarCita(int idCita); // ✅ Nuevo
    }
}
