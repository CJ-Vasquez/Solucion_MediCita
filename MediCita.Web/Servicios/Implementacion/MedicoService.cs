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

        // 1. LISTAR
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
                            NombreCompleto = dr["NombreCompleto"].ToString(),
                            IdEspecialidad = Convert.ToInt32(dr["IdEspecialidad"]),
                            Especialidad = new Especialidad() 
                            { 
                                IdEspecialidad = Convert.ToInt32(dr["IdEspecialidad"]),
                                NombreEspec = dr["Especialidad"].ToString() ?? ""
                            },
                            CMP = dr["CMP"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            Telefono = dr["Telefono"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        // 2. OBTENER POR ID
        public async Task<Medico> Obtener(int id)
        {
            Medico objeto = new Medico();
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
                            NombreCompleto = dr["NombreCompleto"].ToString(),
                            IdEspecialidad = Convert.ToInt32(dr["IdEspecialidad"]),
                            CMP = dr["CMP"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            Telefono = dr["Telefono"].ToString()
                        };
                    }
                }
            }
            return objeto;
        }

        // 3. GUARDAR
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

        // 4. EDITAR
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

        // 5. ELIMINAR
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
