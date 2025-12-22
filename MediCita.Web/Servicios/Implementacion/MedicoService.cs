using Microsoft.Data.SqlClient;
using System.Data;
using MediCita.Web.Entidades;
using MediCita.Web.Servicios.Contrato;

namespace MediCita.Web.Servicios.Implementacion
{
    public class MedicoService : IMedicoService
    {
        private readonly IConfiguration _configuration;

        public MedicoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // ========================================
        // LISTAR TODOS LOS MÉDICOS
        // ========================================
        public async Task<List<Medico>> Listar()
        {
            List<Medico> lista = new List<Medico>();
            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("usp_ListarMedicos", cn);
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
                            IdEspecialidad = Convert.ToInt32(dr["IdEspecialidad"]),
                            Especialidad = new Especialidad()
                            {
                                IdEspecialidad = Convert.ToInt32(dr["IdEspecialidad"]),
                                NombreEspec = dr["Especialidad"]?.ToString() ?? "General"
                            },
                            CMP = dr["CMP"]?.ToString() ?? "",
                            Correo = dr["Correo"]?.ToString() ?? "",
                            Telefono = dr["Telefono"]?.ToString() ?? ""
                        });
                    }
                }
            }
            return lista;
        }

        // ========================================
        // OBTENER MÉDICO POR ID
        // ========================================
        public async Task<Medico?> Obtener(int id)
        {
            Medico? objeto = null;
            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerMedico", cn);
                cmd.Parameters.AddWithValue("@IdMedico", id);
                cmd.CommandType = CommandType.StoredProcedure;
                await cn.OpenAsync();

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        objeto = new Medico()
                        {
                            IdMedico = Convert.ToInt32(dr["IdMedico"]),
                            NombreCompleto = dr["NombreCompleto"]?.ToString() ?? "",
                            IdEspecialidad = Convert.ToInt32(dr["IdEspecialidad"]),
                            CMP = dr["CMP"]?.ToString() ?? "",
                            Correo = dr["Correo"]?.ToString() ?? "",
                            Telefono = dr["Telefono"]?.ToString() ?? ""
                        };
                    }
                }
            }
            return objeto;
        }

        // ========================================
        // ✅ NUEVO: OBTENER MÉDICO POR ID DE USUARIO
        // ========================================
        public async Task<Medico?> ObtenerPorUsuario(int idUsuario)
        {
            Medico? medico = null;
            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                // Query directo porque probablemente no tienes SP para esto aún
                SqlCommand cmd = new SqlCommand(@"
                    SELECT m.IdMedico, m.IdUsuario, m.IdEspecialidad, m.NombreCompleto, 
                           m.CMP, m.Correo, m.Telefono, e.NombreEspec
                    FROM tb_Medicos m
                    INNER JOIN tb_Especialidades e ON m.IdEspecialidad = e.IdEspecialidad
                    WHERE m.IdUsuario = @IdUsuario", cn);

                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                cmd.CommandType = CommandType.Text;

                await cn.OpenAsync();

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        medico = new Medico()
                        {
                            IdMedico = Convert.ToInt32(dr["IdMedico"]),
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            NombreCompleto = dr["NombreCompleto"]?.ToString() ?? "",
                            IdEspecialidad = Convert.ToInt32(dr["IdEspecialidad"]),
                            Especialidad = new Especialidad()
                            {
                                IdEspecialidad = Convert.ToInt32(dr["IdEspecialidad"]),
                                NombreEspec = dr["NombreEspec"]?.ToString() ?? "General"
                            },
                            CMP = dr["CMP"]?.ToString() ?? "",
                            Correo = dr["Correo"]?.ToString() ?? "",
                            Telefono = dr["Telefono"]?.ToString() ?? ""
                        };
                    }
                }
            }
            return medico;
        }

        // ========================================
        // GUARDAR NUEVO MÉDICO
        // ========================================
        public async Task<bool> Guardar(Medico modelo)
        {
            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("usp_RegistrarMedico", cn);
                cmd.Parameters.AddWithValue("@NombreCompleto", modelo.NombreCompleto);
                cmd.Parameters.AddWithValue("@IdEspecialidad", modelo.IdEspecialidad);
                cmd.Parameters.AddWithValue("@CMP", modelo.CMP);
                cmd.Parameters.AddWithValue("@Correo", modelo.Correo);
                cmd.Parameters.AddWithValue("@Telefono", modelo.Telefono);
                cmd.CommandType = CommandType.StoredProcedure;

                await cn.OpenAsync();
                int filas = await cmd.ExecuteNonQueryAsync();
                return filas > 0;
            }
        }

        // ========================================
        // EDITAR MÉDICO
        // ========================================
        public async Task<bool> Editar(Medico modelo)
        {
            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("usp_EditarMedico", cn);
                cmd.Parameters.AddWithValue("@IdMedico", modelo.IdMedico);
                cmd.Parameters.AddWithValue("@NombreCompleto", modelo.NombreCompleto);
                cmd.Parameters.AddWithValue("@IdEspecialidad", modelo.IdEspecialidad);
                cmd.Parameters.AddWithValue("@CMP", modelo.CMP);
                cmd.Parameters.AddWithValue("@Correo", modelo.Correo);
                cmd.Parameters.AddWithValue("@Telefono", modelo.Telefono);
                cmd.CommandType = CommandType.StoredProcedure;

                await cn.OpenAsync();
                int filas = await cmd.ExecuteNonQueryAsync();
                return filas > 0;
            }
        }

        // ========================================
        // ELIMINAR MÉDICO
        // ========================================
        public async Task<bool> Eliminar(int id)
        {
            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("usp_EliminarMedico", cn);
                cmd.Parameters.AddWithValue("@IdMedico", id);
                cmd.CommandType = CommandType.StoredProcedure;

                await cn.OpenAsync();
                int filas = await cmd.ExecuteNonQueryAsync();
                return filas > 0;
            }
        }
    }
}
