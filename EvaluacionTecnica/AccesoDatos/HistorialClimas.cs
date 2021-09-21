using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Modelo;

namespace AccesoDatos
{
    public class HistorialClimas
    {
        #region "PATRON SINGLETON"
        private static HistorialClimas historialClimas = null;
        private HistorialClimas() { }
        public static HistorialClimas getInstance()
        {
            if (historialClimas == null)
            {
                historialClimas = new HistorialClimas();
            }
            return historialClimas;
        }
        #endregion

        public void agregarClima(RegistroClima regClima)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;

            try
            {
                con = Conexion.getInstance().ConexionBD();

                cmd = new SqlCommand("spAgregarRegClima", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@idCiudad", regClima.ciudad.id);
                cmd.Parameters.Add("@temperatura", regClima.temperatura);
                cmd.Parameters.Add("@termica", regClima.termica);
                
                con.Open();

                int filas = cmd.ExecuteNonQuery();

                

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public List<RegistroClima> mostrarHistorial(int id)
        {
            List<RegistroClima> lista = new List<RegistroClima>();
            
            SqlConnection con = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            try
            {
                con = Conexion.getInstance().ConexionBD();
                con.Open();
                cmd = new SqlCommand("spHistorialPorCiudad", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@idCiudad", id);
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    RegistroClima reg = new RegistroClima();
                    reg.id = dr.GetInt32(0);
                    reg.ciudad = new Ciudad();
                    reg.ciudad.id = dr.GetInt32(1);
                    reg.ciudad.nombre = (dr.GetString(2).ToString());
                    reg.ciudad.pais = (dr.GetString(3).ToString());
                    reg.temperatura = (dr.GetString(4).ToString());
                    reg.termica = (dr.GetString(5).ToString());
                    lista.Add(reg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return lista;
        }

        public List<Ciudad> listarCiudades()
        {
            List<Ciudad> lista = new List<Ciudad>();

            SqlConnection con = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            try
            {
                con = Conexion.getInstance().ConexionBD();
                con.Open();
                cmd = new SqlCommand("spListarCiudades", con);
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Ciudad reg = new Ciudad();
                    reg.id = dr.GetInt32(0);
                    reg.nombre = dr.GetString(1).ToString();
                    lista.Add(reg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return lista;
        }

        public string[] buscarDatosCiudad(int id)
        {
            string[] arr = new string[2];
            SqlConnection con = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            try
            {
                con = Conexion.getInstance().ConexionBD();
                con.Open();
                cmd = new SqlCommand("spDevolverDatosCiudad", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@idCiudad", id);
                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    arr[0] = (dr.GetString(0).ToString());
                    arr[1] = (dr.GetString(1).ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return arr;
        }

    }
}
