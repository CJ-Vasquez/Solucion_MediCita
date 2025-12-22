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

                using (SqlTransaction transaction = cn.BeginTransaction())
                {
                    try
                    {
                        // PASO 1: Insertar Cabecera (Venta)
                        SqlCommand cmdVenta = new SqlCommand("usp_RegistrarVenta", cn, transaction);
                        cmdVenta.CommandType = CommandType.StoredProcedure;
                        cmdVenta.Parameters.AddWithValue("@IdPaciente", modelo.IdPaciente); 
                        cmdVenta.Parameters.AddWithValue("@Total", modelo.Total);

                        int idVentaGenerado = Convert.ToInt32(await cmdVenta.ExecuteScalarAsync());

                        // PASO 2: Insertar Detalles
                        foreach (DetalleVenta item in modelo.Detalles)
                        {
                            SqlCommand cmdDetalle = new SqlCommand("usp_RegistrarDetalle", cn, transaction);
                            cmdDetalle.CommandType = CommandType.StoredProcedure;
                            cmdDetalle.Parameters.AddWithValue("@IdVenta", idVentaGenerado);
                            cmdDetalle.Parameters.AddWithValue("@IdMedicamento", item.IdMedicamento);
                            cmdDetalle.Parameters.AddWithValue("@Cantidad", item.Cantidad);
                            cmdDetalle.Parameters.AddWithValue("@PrecioUnitario", item.PrecioUnitario); 
                            cmdDetalle.Parameters.AddWithValue("@SubTotal", item.SubTotal);

                            await cmdDetalle.ExecuteNonQueryAsync();
                        }


                        transaction.Commit();
                        exito = true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        exito = false;
                        throw;
                    }
                }
            }
            return exito;
        }

        public async Task<List<Venta>> ListarVentasPorUsuario(int idUsuario)
        {
            List<Venta> lista = new List<Venta>();
            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                
                SqlCommand cmd = new SqlCommand(
                    "SELECT IdVenta, IdPaciente, Total, FechaVenta " +
                    "FROM tb_Ventas " +
                    "WHERE IdPaciente = @IdPaciente " +
                    "ORDER BY FechaVenta DESC", cn);

                cmd.Parameters.AddWithValue("@IdPaciente", idUsuario);
                await cn.OpenAsync();

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Venta()
                        {
                            IdVenta = Convert.ToInt32(dr["IdVenta"]),
                            IdPaciente = Convert.ToInt32(dr["IdPaciente"]), 
                            Total = Convert.ToDecimal(dr["Total"]),
                            FechaVenta = dr["FechaVenta"] != DBNull.Value
                                ? Convert.ToDateTime(dr["FechaVenta"])
                                : DateTime.Now
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<decimal> ObtenerTotalVentasDelDia()
        {
            decimal total = 0;
            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT ISNULL(SUM(Total), 0) " +
                    "FROM tb_Ventas " +
                    "WHERE CAST(FechaVenta AS DATE) = CAST(GETDATE() AS DATE)", cn);

                await cn.OpenAsync();
                total = Convert.ToDecimal(await cmd.ExecuteScalarAsync());
            }
            return total;
        }

        public async Task<int> ContarVentasDelDia()
        {
            int count = 0;
            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("CadenaSQL")))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) " +
                    "FROM tb_Ventas " +
                    "WHERE CAST(FechaVenta AS DATE) = CAST(GETDATE() AS DATE)", cn);

                await cn.OpenAsync();
                count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            return count;
        }
    }
}
