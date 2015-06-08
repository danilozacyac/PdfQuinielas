using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using PdfQuinielas.Dao;

namespace PdfQuinielas.Models
{
    public class TorneosModel
    {

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["QuinielaMundialConnectionString"].ConnectionString;

        public ObservableCollection<Torneos> GetTorneos()
        {
            ObservableCollection<Torneos> listaPartidos = new ObservableCollection<Torneos>();

            try
            {
                //string cstr = @"Data Source=WIN-KT1RP3JF2N6\MISERVER;Initial Catalog=QuinielaMundial;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string selstr = "Select * FROM Torneos ORDER BY IdTorneo";
                    SqlCommand cmd = new SqlCommand(selstr, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Torneos partido = new Torneos();

                        partido.IdTorneo = Convert.ToInt32(reader["IdTorneo"]);
                        partido.Torneo = reader["Torneo"].ToString();
                        partido.Year = Convert.ToInt32(reader["Year"]);

                        listaPartidos.Add(partido);
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

            return listaPartidos;
        }
    }
}
