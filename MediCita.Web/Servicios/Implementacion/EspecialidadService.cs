using Microsoft.Data.SqlClient;
using System.Data;
using MediCita.Web.Entidades;
using MediCita.Web.Servicios.Contrato;

namespace MediCita.Web.Servicios.Implementacion
{
    public class EspecialidadService : IEspecialidadService
    {
        private readonly IConfiguration _configuration;

        public EspecialidadService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Especialidad>> Listar()
        {
            List<Especialidad> lista = new List<Especialidad>();

            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                // Consulta directa para agilizar (o usa un SP si ya lo creaste)
                SqlCommand cmd = new SqlCommand("SELECT IdEspecialidad, NombreEspec, Descripcion FROM tb_Especialidades", cn);
                cmd.CommandType = CommandType.Text;

                await cn.OpenAsync();
                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Especialidad()
                        {
                            IdEspecialidad = Convert.ToInt32(dr["IdEspecialidad"]),
                            NombreEspec = dr["NombreEspec"].ToString(),
                            Descripcion = dr["Descripcion"].ToString()
                        });
                    }
                }
            }
            return lista;
        }
    }
}