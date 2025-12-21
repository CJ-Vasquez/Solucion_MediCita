using Microsoft.Data.SqlClient;
using System.Data;
using MediCita.Web.Entidades;
using MediCita.Web.Servicios.Contrato;

namespace MediCita.Web.Servicios.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IConfiguration _configuration;

        // Inyección de Dependencias: Recibimos la configuración para leer el appsettings.json

        public UsuarioService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Usuario> ValidarUsuario(string correo, string clave)
        {
            Usuario usuarioEncontrado = null;

            // Leemos la cadena de conexión que configuramos en el paso anterior
            string cadenaConexion = _configuration.GetConnectionString("CadenaSQL");

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                // Usamos el SP que creamos en SQL

                SqlCommand cmd = new SqlCommand("usp_ValidarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@Clave", clave);

                try
                {
                    await cn.OpenAsync();
                    // Ejecutamos la lectura
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            usuarioEncontrado = new Usuario()
                            {
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                NombreCompleto = dr["NombreCompleto"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                IdRol = Convert.ToInt32(dr["IdRol"]),
                                NombreRol = dr["NombreRol"].ToString()
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Aqui guardamos el error en un log
                    string error = ex.Message;
                    throw;
                }
            }
            return usuarioEncontrado;
        }

        public async Task<bool> RegistrarCliente(Usuario usuario)
        {
            bool respuesta = false;
            string cadenaConexion = _configuration.GetConnectionString("CadenaSQL");

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("usp_RegistrarCliente", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NombreCompleto", usuario.NombreCompleto);
                cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                cmd.Parameters.AddWithValue("@Clave", usuario.Clave);

                try
                {
                    await cn.OpenAsync();
                    int filasAfectadas = await cmd.ExecuteNonQueryAsync();
                    respuesta = filasAfectadas > 0;
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public async Task<bool> ExisteCorreo(string correo)
        {
            bool existe = false;
            string cadenaConexion = _configuration.GetConnectionString("CadenaSQL");

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tb_Usuarios WHERE Correo = @Correo", cn);
                cmd.Parameters.AddWithValue("@Correo", correo);

                try
                {
                    await cn.OpenAsync();
                    int count = (int)await cmd.ExecuteScalarAsync();
                    existe = count > 0;
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
            return existe;
        }
    }
}
