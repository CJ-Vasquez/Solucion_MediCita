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

        // ========================================
        // Listar Médicos por Especialidad
        // ========================================
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
                            NombreCompleto = dr["NombreCompleto"]?.ToString() ?? "",
                            Especialidad = new Especialidad()
                            {
                                NombreEspec = dr["Especialidad"]?.ToString() ?? ""
                            },
                            CMP = dr["CMP"]?.ToString() ?? ""
                        });
                    }
                }
            }

            return lista;
        }

        // ========================================
        // Registrar Cita
        // ========================================
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

        // ========================================
        // Reporte de Citas por Usuario (Paciente)
        // ========================================
        public async Task<List<Cita>> ReporteCitasUsuario(int idUsuario)
        {
            List<Cita> lista = new List<Cita>();

            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("usp_ListarCitasPorUsuario", cn);
                cmd.Parameters.AddWithValue("@IdPaciente", idUsuario);
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
                            NombreMedico = dr["NombreMedico"]?.ToString() ?? "",
                            NombreEspecialidad = dr["NombreEspecialidad"]?.ToString() ?? "",
                            Estado = dr["Estado"]?.ToString() ?? "Pendiente"
                        });
                    }
                }
            }

            return lista;
        }

        // ========================================
        // Listar Citas por Médico
        // ========================================
        public async Task<List<Cita>> ListarCitasPorMedico(int idMedico)
        {
            List<Cita> lista = new List<Cita>();

            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("usp_ListarCitasPorMedico", cn);
                cmd.Parameters.AddWithValue("@IdMedico", idMedico);
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
                            NombrePaciente = dr["NombrePaciente"]?.ToString() ?? "",
                            NombreEspecialidad = dr["NombreEspecialidad"]?.ToString() ?? "",
                            Estado = dr["Estado"]?.ToString() ?? "Pendiente"
                        });
                    }
                }
            }

            return lista;
        }

        // ========================================
        // Actualizar Estado de Cita
        // ========================================
        public async Task<bool> ActualizarEstadoCita(int idCita, string nuevoEstado)
        {
            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("usp_ActualizarEstadoCita", cn);
                cmd.Parameters.AddWithValue("@IdCita", idCita);
                cmd.Parameters.AddWithValue("@NuevoEstado", nuevoEstado);
                cmd.CommandType = CommandType.StoredProcedure;

                await cn.OpenAsync();
                int filas = await cmd.ExecuteNonQueryAsync();
                return filas > 0;
            }
        }

        // ========================================
        // Cancelar Cita
        // ========================================
        public async Task<bool> CancelarCita(int idCita)
        {
            return await ActualizarEstadoCita(idCita, "Cancelada");
        }

        // ========================================
        // Contar Citas del Día (sin SP)
        // ========================================
        public async Task<int> ContarCitasDelDia()
        {
            int count = 0;

            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                // Consulta directa para evitar dependencia de SP inexistente
                string sql = "SELECT COUNT(*) FROM tb_Citas WHERE CAST(FechaCita AS DATE) = CAST(GETDATE() AS DATE)";
                SqlCommand cmd = new SqlCommand(sql, cn);

                await cn.OpenAsync();
                count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }

            return count;
        }

        // ========================================
        // Listar Citas Pendientes
        // ========================================
        public async Task<List<Cita>> ListarCitasPendientes()
        {
            List<Cita> lista = new List<Cita>();

            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("usp_ListarCitasPendientes", cn);
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
                            NombrePaciente = dr["NombrePaciente"]?.ToString() ?? "",
                            NombreMedico = dr["NombreMedico"]?.ToString() ?? "",
                            NombreEspecialidad = dr["NombreEspecialidad"]?.ToString() ?? "",
                            Estado = dr["Estado"]?.ToString() ?? "Pendiente"
                        });
                    }
                }
            }

            return lista;
        }
    }
}
