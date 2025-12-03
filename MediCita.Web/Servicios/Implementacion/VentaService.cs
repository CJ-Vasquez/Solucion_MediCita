using Microsoft.Data.SqlClient;
using System.Data;
using MediCita.Web.Entidades;
using MediCita.Web.Servicios.Contrato;

namespace MediCita.Web.Servicios.Implementacion
{
    public class VentaService : IVentaService
    {
        private readonly IConfiguration _configuration;

        public VentaService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> Registrar(Venta modelo)
        {
            bool exito = false;

            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                await cn.OpenAsync();

                // INICIO DE LA TRANSACCIÓN
               
                using (SqlTransaction transaction = cn.BeginTransaction())
                {
                    try
                    {
                        // PASO 1: Insertar Cabecera (Venta)
                        SqlCommand cmdVenta = new SqlCommand("usp_RegistrarVenta", cn, transaction);
                        cmdVenta.CommandType = CommandType.StoredProcedure;
                        cmdVenta.Parameters.AddWithValue("@IdUsuario", modelo.IdUsuario);
                        cmdVenta.Parameters.AddWithValue("@Total", modelo.Total);

                        // Ejecutamos y obtenemos el ID generado
                        int idVentaGenerado = Convert.ToInt32(await cmdVenta.ExecuteScalarAsync());

                        // PASO 2: Insertar Detalles (Bucle)
                        foreach (DetalleVenta item in modelo.Detalles)
                        {
                            SqlCommand cmdDetalle = new SqlCommand("usp_RegistrarDetalle", cn, transaction);
                            cmdDetalle.CommandType = CommandType.StoredProcedure;
                            cmdDetalle.Parameters.AddWithValue("@IdVenta", idVentaGenerado);
                            cmdDetalle.Parameters.AddWithValue("@IdMedicamento", item.IdMedicamento);
                            cmdDetalle.Parameters.AddWithValue("@Cantidad", item.Cantidad);
                            cmdDetalle.Parameters.AddWithValue("@Precio", item.Precio);
                            cmdDetalle.Parameters.AddWithValue("@SubTotal", item.Total);

                            await cmdDetalle.ExecuteNonQueryAsync();
                        }

                        // Si llegamos aquí sin errores, CONFIRMAMOS la transacción
                        transaction.Commit();
                        exito = true;
                    }
                    catch
                    {
                        // Si algo falló, DESHACEMOS todo (Rollback)
                        transaction.Rollback();
                        exito = false;
                        throw; // Re-lanzamos el error para verlo en consola
                    }
                }
            }
            return exito;
        }
    }
}