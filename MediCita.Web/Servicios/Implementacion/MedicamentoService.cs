using Microsoft.Data.SqlClient;
using System.Data;
using MediCita.Web.Entidades;
using MediCita.Web.Servicios.Contrato;

namespace MediCita.Web.Servicios.Implementacion
{
    public class MedicamentoService : IMedicamentoService
    {
        private readonly IConfiguration _configuration;
        public MedicamentoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // 1. LISTAR
        public async Task<List<Medicamento>> Listar()
        {
            List<Medicamento> lista = new List<Medicamento>();
            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("usp_ListarMedicamentos", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                await cn.OpenAsync();
                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Medicamento()
                        {
                            IdMedicamento = Convert.ToInt32(dr["IdMedicamento"]),
                            Nombre = dr["Nombre"].ToString(),
                            Laboratorio = dr["Laboratorio"].ToString(),
                            Precio = Convert.ToDecimal(dr["Precio"]),
                            Stock = Convert.ToInt32(dr["Stock"])
                        });
                    }
                }
            }
            return lista;
        }

        // 2. OBTENER (Para editar)
        public async Task<Medicamento> Obtener(int id)
        {
            Medicamento objeto = null;
            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerMedicamento", cn);
                cmd.Parameters.AddWithValue("@IdMedicamento", id);
                cmd.CommandType = CommandType.StoredProcedure;
                await cn.OpenAsync();
                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        objeto = new Medicamento()
                        {
                            IdMedicamento = Convert.ToInt32(dr["IdMedicamento"]),
                            Nombre = dr["Nombre"].ToString(),
                            Laboratorio = dr["Laboratorio"].ToString(),
                            Precio = Convert.ToDecimal(dr["Precio"]),
                            Stock = Convert.ToInt32(dr["Stock"])
                        };
                    }
                }
            }
            return objeto;
        }

        // 3. GUARDAR
        public async Task<bool> Guardar(Medicamento modelo)
        {
            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("usp_RegistrarMedicamento", cn);
                cmd.Parameters.AddWithValue("@Nombre", modelo.Nombre);
                cmd.Parameters.AddWithValue("@Laboratorio", modelo.Laboratorio);
                cmd.Parameters.AddWithValue("@Precio", modelo.Precio);
                cmd.Parameters.AddWithValue("@Stock", modelo.Stock);
                cmd.CommandType = CommandType.StoredProcedure;
                await cn.OpenAsync();
                int filas = await cmd.ExecuteNonQueryAsync();
                return filas > 0;
            }
        }

        // 4. EDITAR
        public async Task<bool> Editar(Medicamento modelo)
        {
            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand("usp_EditarMedicamento", cn);
                cmd.Parameters.AddWithValue("@IdMedicamento", modelo.IdMedicamento);
                cmd.Parameters.AddWithValue("@Nombre", modelo.Nombre);
                cmd.Parameters.AddWithValue("@Laboratorio", modelo.Laboratorio);
                cmd.Parameters.AddWithValue("@Precio", modelo.Precio);
                cmd.Parameters.AddWithValue("@Stock", modelo.Stock);
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
                SqlCommand cmd = new SqlCommand("usp_EliminarMedicamento", cn);
                cmd.Parameters.AddWithValue("@IdMedicamento", id);
                cmd.CommandType = CommandType.StoredProcedure;
                await cn.OpenAsync();
                int filas = await cmd.ExecuteNonQueryAsync();
                return filas > 0;
            }
        }
    }
}