using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;



namespace CineMas.Models
{
    public class ConexionBD
    {
        private static SqlConnection objConexion;
        private static string error; 

        public static SqlConnection getConexion()
        {
            if (objConexion != null)
                return objConexion;
            //se crea una nueva conexion
            objConexion = new SqlConnection();
            objConexion.ConnectionString = @"Data Source=.; Initial Catalog=CineMas; Integrated Security=True";
            //manejo de errores
            try
            {
                objConexion.Open();
                return objConexion;
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
        }

        public static void closeConexion()
        {
            if (objConexion != null) {
                objConexion.Close();
            }
        }
    }
}