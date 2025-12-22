using Microsoft.Data.SqlClient;
using System.Data;
using MediCita.Web.Entidades;
using MediCita.Web.Servicios.Contrato;

namespace MediCita.Web.Servicios.Implementacion
{
    public class RolService : IRolService
    {
        private readonly IConfiguration _configuration;

        public RolService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Rol>> Listar()
        {
            List<Rol> lista = new List<Rol>();
            string cadenaConexion = _configuration.GetConnectionString("CadenaSQL");

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("usp_ListarRoles", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                await cn.OpenAsync();
                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Rol
                        {
                            IdRol = Convert.ToInt32(dr["IdRol"]),
                            NombreRol = dr["NombreRol"].ToString()
                        });
                    }
                }
            }

            return lista;
        }
    }
}
