using Microsoft.Data.SqlClient;
using System.Data;
using MediCita.Web.Entidades;
using MediCita.Web.Servicios.Contrato;

namespace MediCita.Web.Servicios.Implementacion
{
    public class CitaService : ICitaService
    {
        private readonly IConfiguration _configuration;

        public CitaService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Medico>> ListarMedicos(int idEspecialidad)
        {
            List<Medico> lista = new List<Medico>();
            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("usp_ListarMedicosPorEspecialidad", cn);
                cmd.Parameters.AddWithValue("@IdEspecialidad", idEspecialidad);
                cmd.CommandType = CommandType.StoredProcedure;
                await cn.OpenAsync();
                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Medico()
                        {
                            IdMedico = Convert.ToInt32(dr["IdMedico"]),
                            NombreCompleto = dr["NombreCompleto"].ToString(),
                            Especialidad = dr["Especialidad"].ToString(),
                            CMP = dr["CMP"].ToString()
                        });
                    }
                }
            }
            return lista;
        }
        public async Task<List<Cita>> ReporteCitasUsuario(int idPaciente)
        {
            List<Cita> lista = new List<Cita>();
            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("usp_ListarCitasPorUsuario", cn);
                cmd.Parameters.AddWithValue("@IdPaciente", idPaciente);
                cmd.CommandType = CommandType.StoredProcedure;
                await cn.OpenAsync();
                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Cita()
                        {
                            IdCita = Convert.ToInt32(dr["IdCita"]),
                            FechaCita = Convert.ToDateTime(dr["FechaCita"]),
                            NombreMedico = dr["NombreMedico"].ToString(),
                            NombreEspecialidad = dr["NombreEspecialidad"].ToString(),
                            Estado = dr["Estado"].ToString()
                        });
                    }
                }
            }
            return lista;
        }
        public async Task<bool> RegistrarCita(Cita modelo)
        {
            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("usp_RegistrarCita", cn);
                cmd.Parameters.AddWithValue("@IdPaciente", modelo.IdPaciente);
                cmd.Parameters.AddWithValue("@IdMedico", modelo.IdMedico);
                cmd.Parameters.AddWithValue("@FechaCita", modelo.FechaCita);
                cmd.CommandType = CommandType.StoredProcedure;
                await cn.OpenAsync();
                int filas = await cmd.ExecuteNonQueryAsync();
                return filas > 0;
            }
        }
    }
}