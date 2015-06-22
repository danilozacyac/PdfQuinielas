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
                        equipos.Add(new Equipos(Convert.ToInt32(rdr["IdPais"]),
                            rdr["Pais"].ToString(),
                            Convert.ToInt32(rdr["IdConfederacion"]),
                            Convert.ToInt32(rdr["Tipo"])));
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

        public static void SetNewPronosticos(Equipos equipo)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                String query = "INSERT INTO Paises (IdPais,Pais,IdConfederacion,Tipo) VALUES (@IdPais,@Pais,@IdConfederacion,@Tipo)";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@IdPartido", equipo.IdEquipo);
                command.Parameters.AddWithValue("@Pais", equipo.IdEquipo);
                command.Parameters.AddWithValue("@IdConfederacion", equipo.IdEquipo);
                command.Parameters.AddWithValue("@Tipo", equipo.IdEquipo);

                command.ExecuteNonQuery();

                command.Dispose();
                command = null;
                connection.Close();
            }
            catch (SqlException)
            {
            }
            catch (Exception)
            {
            }
            finally
            {
                connection.Close();
            }
        }
    }
}