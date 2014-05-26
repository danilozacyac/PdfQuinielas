using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Quiniela.Dao;

namespace PdfQuinielas.Models
{
    public class EquiposModel
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["QuinielaMundialConnectionString"].ConnectionString;

        public static ObservableCollection<Equipos> GetEquipos()
        {
            ObservableCollection<Equipos> equipos = new ObservableCollection<Equipos>();

            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string selstr = "SELECT * FROM Paises ORDER BY idPais";
                    SqlCommand cmd = new SqlCommand(selstr, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        equipos.Add(new Equipos(Convert.ToInt32(rdr["IdPais"]), rdr["Pais"].ToString()));

                    }
                    conn.Close();
                }
            }
            catch (SqlException)
            {

            }
            finally
            {

            }

            return equipos;
        }

    }
}
