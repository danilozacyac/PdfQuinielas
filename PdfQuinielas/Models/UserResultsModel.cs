using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using PdfQuinielas.Dao;
using Quiniela.Dao;

namespace PdfQuinielas.Models
{
    public class UserResultsModel
    {

        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["QuinielaMundialConnectionString"].ConnectionString;

        public static ObservableCollection<UserResults> GetResultadosGenerales()
        {
            ObservableCollection<UserResults> resultados = new ObservableCollection<UserResults>();

            SqlConnection conn = new SqlConnection(connectionString);
            try
            {

                using (conn)
                {
                    conn.Open();

                    string selstr = "SELECT * FROM PdfUsuarios ";
                    SqlCommand cmd = new SqlCommand(selstr, conn);
                    
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        UserResults user = new UserResults();
                        user.IdUsuario = reader["IdUsuario"] as int? ?? -1;
                        user.Fecha = Convert.ToDateTime(reader["fecha"]);
                        user.Local = reader["Local"].ToString();
                        user.Visitante = reader["Visitante"].ToString();
                        user.Ganador = reader["Ganador"].ToString();
                        user.GolesLocal = reader["GolesLocal"] as int? ?? -1;
                        user.GolesVisita = reader["GolesVisita"] as int? ?? -1;


                        resultados.Add(user);

                    }
                    conn.Close();
                }
            }
            catch (SqlException)
            {

            }
            finally
            {
                conn.Close();
            }

            return resultados;
        }

        public static ObservableCollection<UserResults> GetResultadosPorUsuario(int idUsuario,Torneos torneo)
        {
            ObservableCollection<UserResults> userResults = new ObservableCollection<UserResults>();
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {

                using (conn)
                {
                    conn.Open();

                    string selstr = "SELECT * FROM PdfUsuarios Where IdUsuario = @IdUsuario AND IdTorneo = @IdTorneo Order By Fecha";
                    SqlCommand cmd = new SqlCommand(selstr, conn);
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@IdTorneo", torneo.IdTorneo);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        UserResults user = new UserResults();
                        user.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
                        user.Fecha = Convert.ToDateTime(reader["fecha"]);
                        user.Local = reader["Local"].ToString();
                        user.Visitante = reader["Visitante"].ToString();
                        user.Ganador = reader["Ganador"].ToString();
                        user.GolesLocal = Convert.ToInt32(reader["GolesLocal"]);
                        user.GolesVisita = Convert.ToInt32(reader["GolesVisita"]);

                        userResults.Add(user);

                    }
                    conn.Close();
                }
            }
            catch (SqlException)
            {

            }
            finally
            {
                conn.Close();
            }

            return userResults;
        }
    }
}
