using Microsoft.Data.SqlClient;
using System.Data;
using MediCita.Web.Entidades;
using MediCita.Web.Servicios.Contrato;

namespace MediCita.Web.Servicios.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IConfiguration _configuration;

        public UsuarioService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // ===== MÉTODOS EXISTENTES =====
        public async Task<Usuario> ValidarUsuario(string correo, string clave)
        {
            Usuario usuarioEncontrado = null;
            string cadenaConexion = _configuration.GetConnectionString("CadenaSQL");

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("usp_ValidarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@Clave", clave);

                try
                {
                    await cn.OpenAsync();
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

        // ===== NUEVOS MÉTODOS CRUD =====
        public async Task<List<Usuario>> Listar()
        {
            List<Usuario> lista = new List<Usuario>();
            string cadenaConexion = _configuration.GetConnectionString("CadenaSQL");

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("usp_ListarUsuarios", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                await cn.OpenAsync();
                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Usuario
                        {
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            NombreCompleto = dr["NombreCompleto"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            IdRol = Convert.ToInt32(dr["IdRol"]),
                            NombreRol = dr["NombreRol"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public async Task<Usuario> Obtener(int id)
        {
            Usuario usuario = null;
            string cadenaConexion = _configuration.GetConnectionString("CadenaSQL");

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", id);

                await cn.OpenAsync();
                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        usuario = new Usuario
                        {
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            NombreCompleto = dr["NombreCompleto"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            Clave = dr["Clave"].ToString(),
                            IdRol = Convert.ToInt32(dr["IdRol"]),
                            NombreRol = dr["NombreRol"].ToString()
                        };
                    }
                }
            }

            return usuario;
        }

        // ===== 1. GUARDAR (CREAR)  =====
        public async Task<bool> Guardar(Usuario modelo)
        {
            bool respuesta = false;
            string cadenaConexion = _configuration.GetConnectionString("CadenaSQL");

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("usp_CrearUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreCompleto", modelo.NombreCompleto);
                cmd.Parameters.AddWithValue("@Correo", modelo.Correo);
                cmd.Parameters.AddWithValue("@Clave", modelo.Clave);
                cmd.Parameters.AddWithValue("@IdRol", modelo.IdRol);

                try
                {
                    await cn.OpenAsync();

                    // Leer el resultado que devuelve el SP
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            int resultado = Convert.ToInt32(dr["Resultado"]);

                            if (resultado == -1)
                            {
                                throw new Exception("El correo ya está registrado");
                            }
                            else if (resultado == 1)
                            {
                                respuesta = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return respuesta;
        }

        // ===== 2. EDITAR =====
        public async Task<bool> Editar(Usuario modelo)
        {
            bool respuesta = false;
            string cadenaConexion = _configuration.GetConnectionString("CadenaSQL");

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("usp_EditarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", modelo.IdUsuario);
                cmd.Parameters.AddWithValue("@NombreCompleto", modelo.NombreCompleto);
                cmd.Parameters.AddWithValue("@Correo", modelo.Correo);
                cmd.Parameters.AddWithValue("@Clave", modelo.Clave ?? "");
                cmd.Parameters.AddWithValue("@IdRol", modelo.IdRol);

                try
                {
                    await cn.OpenAsync();

                    // Leer el resultado que devuelve el SP
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            int resultado = Convert.ToInt32(dr["Resultado"]);

                            if (resultado == -1)
                            {
                                throw new Exception("El correo ya está registrado por otro usuario");
                            }
                            else if (resultado == 0)
                            {
                                throw new Exception("Usuario no encontrado");
                            }
                            else if (resultado == 1)
                            {
                                respuesta = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return respuesta;
        }

        // ===== 3. ELIMINAR =====
        public async Task<bool> Eliminar(int id)
        {
            bool respuesta = false;
            string cadenaConexion = _configuration.GetConnectionString("CadenaSQL");

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("usp_EliminarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", id);

                try
                {
                    await cn.OpenAsync();

                    // Leer el resultado que devuelve el SP
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            int resultado = Convert.ToInt32(dr["Resultado"]);

                            if (resultado == 0)
                            {
                                throw new Exception("Usuario no encontrado");
                            }
                            else if (resultado == 1)
                            {
                                respuesta = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return respuesta;
        }
    }
}
