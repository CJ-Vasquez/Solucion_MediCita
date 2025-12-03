using MediCita.Web.Entidades;

namespace MediCita.Web.Servicios.Contrato
{
    public interface ICitaService
    {
        Task<List<Medico>> ListarMedicos(int idEspecialidad);
        Task<bool> RegistrarCita(Cita modelo);
        Task<List<Cita>> ReporteCitasUsuario(int idPaciente);
    }
}