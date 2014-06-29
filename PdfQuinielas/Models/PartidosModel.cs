using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Quiniela.Dao;

namespace Quiniela.Models
{
    public class PartidosModel
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["QuinielaMundialConnectionString"].ConnectionString;

        /// <summary>
        /// Listado de partidos a jugarse durante la competición 
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<Partidos> GetPartidos()
        {
            ObservableCollection<Partidos> listaPartidos = new ObservableCollection<Partidos>();

            try
            {
                //string cstr = @"Data Source=WIN-KT1RP3JF2N6\MISERVER;Initial Catalog=QuinielaMundial;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string selstr = "Select P.*,Pa.Pais ELocal,Pa2.Pais EVisita from Partidos P INNER JOIN Paises Pa ON P.idPaisLocal = Pa.idPais " +
                                    " INNER JOIN Paises Pa2 On P.idPaisVisita = Pa2.idPais  Order By P.IdPartido";
                    SqlCommand cmd = new SqlCommand(selstr, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Partidos partido = new Partidos();

                        partido.IdPartido = reader["IdPartido"] as int? ?? -1;
                        partido.Fecha = Convert.ToDateTime(reader["Fecha"]);
                        partido.IdPaisLocal = reader["IdPaisLocal"] as int? ?? -1;
                        partido.IdPaisVisita = reader["IdPaisVisita"] as int? ?? -1;
                        partido.GolesLocal = reader["GolesLocal"] as int? ?? -1;
                        partido.GolesVisita = reader["GolesVisita"] as int? ?? -1;
                        partido.PaisLocal = reader["ELocal"].ToString();
                        partido.PaisVisita = reader["EVisita"].ToString();
                        partido.IdPaisGanador = -23;

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

        public static ObservableCollection<Partidos> GetPartidosByUser(int idusuario)
        {
            ObservableCollection<Partidos> listaPartidos = new ObservableCollection<Partidos>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string selstr = "Select * FROM Pronosticos WHERE IdUsuario = @IdUsuario ORDEr BY IdPartido";
                    SqlCommand cmd = new SqlCommand(selstr, conn);
                    cmd.Parameters.AddWithValue("@IdUsuario", idusuario);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Partidos partido = new Partidos();

                        partido.IdPartido = reader["IdPartido"] as int? ?? -1;
                        
                        partido.IdPaisGanador = reader["IdPaisGanador"] as int? ?? -1;
                        
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

        public static ObservableCollection<Partidos> GetPartidosForSettingScore()
        {
            ObservableCollection<Partidos> listaPartidos = new ObservableCollection<Partidos>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string selstr = "Select P.*,Pa.Pais ELocal,Pa2.Pais EVisita from Partidos P INNER JOIN Paises Pa ON P.idPaisLocal = Pa.idPais " +
                                    " INNER JOIN Paises Pa2 On P.idPaisVisita = Pa2.idPais WHERE P.IdPaisGanador IS NULL";
                    SqlCommand cmd = new SqlCommand(selstr, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Partidos partido = new Partidos();

                        partido.IdPartido = reader["IdPartido"] as int? ?? -1;
                        partido.Fecha = Convert.ToDateTime(reader["Fecha"]);
                        partido.IdPaisLocal = reader["IdPaisLocal"] as int? ?? -1;
                        partido.IdPaisVisita = reader["IdPaisVisita"] as int? ?? -1;
                        partido.GolesLocal = reader["GolesLocal"] as int? ?? -1;
                        partido.GolesVisita = reader["GolesVisita"] as int? ?? -1;
                        partido.PaisLocal = reader["ELocal"].ToString();
                        partido.PaisVisita = reader["EVisita"].ToString();
                        partido.IdPaisGanador = reader["IdPaisGanador"] as int? ?? -23;

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

        /// <summary>
        /// Guarda los resultados propuestos por los participantes para cada uno de los partidos
        /// </summary>
        /// <param name="listaPartidos"></param>
        /// <param name="idUser"></param>
        public static void SetNewPronosticos(ObservableCollection<Partidos> listaPartidos, int idUser)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                foreach (Partidos partido in listaPartidos)
                {
                    String query = "INSERT INTO Pronosticos (IdPartido,IdUsuario,GolesLocal,GolesVisita,IdPaisGanador,PuntosGanados) VALUES (@IdPartido,@IdUsuario,@GolesLocal,@GolesVisita,@IdPaisGanador,@PuntosGanados)";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.Add("@IdPartido", SqlDbType.NChar);
                    command.Parameters["@IdPartido"].Value = partido.IdPartido;

                    command.Parameters.Add("@IdUsuario", SqlDbType.NChar);
                    command.Parameters["@IdUsuario"].Value = idUser;

                    command.Parameters.Add("@GolesLocal", SqlDbType.NChar);
                    command.Parameters["@GolesLocal"].Value = partido.GolesLocal;

                    command.Parameters.Add("@GolesVisita", SqlDbType.NChar);
                    command.Parameters["@GolesVisita"].Value = partido.GolesVisita;

                    command.Parameters.Add("@IdPaisGanador", SqlDbType.NChar);
                    command.Parameters["@IdPaisGanador"].Value = (partido.IdPaisGanador == 0) ? 999 : partido.IdPaisGanador;

                    command.Parameters.Add("@PuntosGanados", SqlDbType.NChar);
                    command.Parameters["@PuntosGanados"].Value = 0;
                    
                    command.ExecuteNonQuery();

                    command.Dispose();
                    command = null;
                }
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

        public static void UpdatePartidosScore(Partidos partido)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                String query = "UPDATE Partidos Set GolesLocal = @GolesLocal, GolesVisita = @GolesVisita, IdPaisGanador = @IdPaisGanador WHERE IdPartido = @IdPartido ";

                SqlCommand command = new SqlCommand(query, connection);
                    
                command.Parameters.Add("@GolesLocal", SqlDbType.NChar);
                command.Parameters["@GolesLocal"].Value = partido.GolesLocal;

                command.Parameters.Add("@GolesVisita", SqlDbType.NChar);
                command.Parameters["@GolesVisita"].Value = partido.GolesVisita;

                command.Parameters.Add("@IdPaisGanador", SqlDbType.NChar);
                command.Parameters["@IdPaisGanador"].Value = partido.IdPaisGanador;

                command.Parameters.Add("@IdPartido", SqlDbType.NChar);
                command.Parameters["@IdPartido"].Value = partido.IdPartido;

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

        public static void UpdatePronosticosGainPoint(Partidos partido)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                String query = "UPDATE Pronosticos Set PuntosGanados = @PuntosGanados WHERE IdPartido = @IdPartido AND IdPaisGanador = @IdPaisGanador";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add("@PuntosGanados", SqlDbType.NChar);
                command.Parameters["@PuntosGanados"].Value = 1;

                command.Parameters.Add("@IdPartido", SqlDbType.NChar);
                command.Parameters["@IdPartido"].Value = partido.IdPartido;

                command.Parameters.Add("@IdPaisGanador", SqlDbType.NChar);
                command.Parameters["@IdPaisGanador"].Value = partido.IdPaisGanador;

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

        /// <summary>
        /// Actualiza la información de los grupos de acuerdo a el resultado 
        /// del partido que se esta guardando
        /// </summary>
        /// <param name="partido">Resultado del partido jugado</param>
        /// <param name="tipo">Que equipo se actualizará 1. Local 2. Visita</param>
        /// <param name="idPais">Identificador del pais que se actualizara</param>
        public static void UpdateGruposInfo(Partidos partido, int tipo, int idPais)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            int ganado = 0;
            int perdido = 0;
            int empate = 0;
            int puntosGanados = 0;
            int golesFavor = 0;
            int golesContra = 0;

            try
            {
                if (tipo == 1)
                {
                    if (partido.GolesLocal > partido.GolesVisita)
                    {
                        ganado = 1;
                        puntosGanados = 3;
                    }
                    else if (partido.GolesVisita > partido.GolesLocal)
                        perdido = 1;
                    else
                    {
                        empate = 1;
                        puntosGanados = 1;
                    }

                    golesFavor = partido.GolesLocal;
                    golesContra = partido.GolesVisita;
                }
                else if (tipo == 2)
                {
                    if (partido.GolesVisita > partido.GolesLocal)
                    {
                        ganado = 1;
                        puntosGanados = 3;
                    }
                    else if (partido.GolesLocal > partido.GolesVisita)
                        perdido = 1;
                    else
                    {
                        empate = 1;
                        puntosGanados = 1;
                    }

                    golesFavor = partido.GolesVisita;
                    golesContra = partido.GolesLocal;
                }

                connection.Open();
                String query = "UPDATE Grupos Set Ganados = (SELECT Ganados + @Ganados FROM Grupos WHERE idPais = @IdPais), " +
                               "Perdidos = (SELECT Perdidos + @Perdidos FROM Grupos WHERE idPais = @IdPais), " +
                               "Empates = (SELECT Empates + @Empates FROM Grupos WHERE idPais = @IdPais), " +
                               "GolesFavor = (SELECT GolesFavor + @GolesFavor FROM Grupos WHERE idPais = @IdPais), " +
                               "GolesContra = (SELECT GolesContra + @GolesContra FROM Grupos WHERE idPais = @IdPais), " +
                               "Puntos = (SELECT Puntos + @Puntos FROM Grupos WHERE idPais = @IdPais) " +
                               "WHERE IdPais = @IdPais";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add("@Ganados", SqlDbType.NChar);
                command.Parameters["@Ganados"].Value = ganado;

                command.Parameters.Add("@Perdidos", SqlDbType.NChar);
                command.Parameters["@Perdidos"].Value = perdido;

                command.Parameters.Add("@Empates", SqlDbType.NChar);
                command.Parameters["@Empates"].Value = empate;

                command.Parameters.Add("@GolesFavor", SqlDbType.NChar);
                command.Parameters["@GolesFavor"].Value = golesFavor;

                command.Parameters.Add("@GolesContra", SqlDbType.NChar);
                command.Parameters["@GolesContra"].Value = golesContra;

                command.Parameters.Add("@Puntos", SqlDbType.NChar);
                command.Parameters["@Puntos"].Value = puntosGanados;

                command.Parameters.Add("@IdPais", SqlDbType.NChar);
                command.Parameters["@IdPais"].Value = idPais;

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


        /// <summary>
        /// Actualiza la diferencia de goles de todos los partidos
        /// </summary>
        /// <param name="partido"></param>
        public static void UpdateDiferenciaGoles()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                String query = "UPDATE G SET Diferencia = t2.NuevaDiferencia 	FROM Grupos G JOIN " +
		                        "(SELECT idPais, GolesFavor - GolesContra as NuevaDiferencia FROM Grupos t2 GROUP BY idPais,GolesFavor,GolesContra) t2 " +
		                        "ON G.IdPais = t2.IdPais";

                SqlCommand command = new SqlCommand(query, connection);
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