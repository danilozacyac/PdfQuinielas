using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using PdfQuinielas.Dao;
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


        /// <summary>
        /// Obtiene la lista de partidos a los cuales aún no se les asigna resultado
        /// </summary>
        /// <param name="torneo">Torneo que se esta disputando</param>
        /// <returns></returns>
        public static ObservableCollection<Partidos> GetPartidosForSettingScore(Torneos torneo)
        {
            ObservableCollection<Partidos> listaPartidos = new ObservableCollection<Partidos>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string selstr = "Select P.*,Pa.Pais ELocal,Pa2.Pais EVisita from Partidos P INNER JOIN Paises Pa ON P.idPaisLocal = Pa.idPais " +
                                    " INNER JOIN Paises Pa2 On P.idPaisVisita = Pa2.idPais WHERE P.IdPaisGanador IS NULL AND idTorneo = @IdTorneo ORDER BY P.Fecha";
                    SqlCommand cmd = new SqlCommand(selstr, conn);
                    cmd.Parameters.AddWithValue("@IdTorneo", torneo.IdTorneo);
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
                        partido.PartidoString = partido.PaisLocal + " vs " + partido.PaisVisita;
                        

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

        /// <summary>
        /// Actualiza el marcador del partido
        /// </summary>
        /// <param name="partido"></param>
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

        /// <summary>
        /// Asigna un punto a cada uno de los participantes que acertaron al ganador del partido.
        /// Esto para los torneos donde no fue necesario pronosticar un marcador
        /// </summary>
        /// <param name="partido"></param>
        public static void UpdatePronosticosPuntosGanadosSinMarcador(Partidos partido)
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
        /// Asigna los puntos que obtiene por partido cada participante de acuerdo a su pronostico del ganador y del resultado.
        /// </summary>
        /// <param name="partido"></param>
        public static void UpdatePronosticosPuntosGanadosConMarcador(Partidos partido)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            Dictionary<int, Partidos> listaPronosticos = PartidosModel.GetPronosticoByPartidoByUser(partido.IdPartido);

            try
            {
                connection.Open();

                foreach (KeyValuePair<int, Partidos> pronostico in listaPronosticos)
                {
                    int puntosGanados = 0;

                    //Tipo de Acierto = 1 -> Gana 1 Punto
                    //                = 2 -> Gana 3 puntos
                    int tipoAcierto = 0; 

                    Partidos elPronostico = pronostico.Value as Partidos;

                    if (partido.IdPaisGanador == elPronostico.IdPaisGanador)
                    {
                        if (partido.GolesLocal == elPronostico.GolesLocal && partido.GolesVisita == elPronostico.GolesVisita)
                        {
                            tipoAcierto = 2;
                            puntosGanados = 3;
                        }
                        else
                        {
                            tipoAcierto = 1;
                            puntosGanados = 1;
                        }
                    }
                    else
                    {
                        puntosGanados = 0;
                        tipoAcierto = 0;
                    }


                    String query = "UPDATE Pronosticos Set PuntosGanados = @PuntosGanados, TipoAcierto = @TipoAcierto WHERE IdPartido = @IdPartido AND IdUsuario = @IdUsuario";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.Add("@PuntosGanados", SqlDbType.NChar);
                    command.Parameters["@PuntosGanados"].Value = puntosGanados;

                    command.Parameters.Add("@TipoAcierto", SqlDbType.NChar);
                    command.Parameters["@TipoAcierto"].Value = tipoAcierto;

                    command.Parameters.Add("@IdPartido", SqlDbType.NChar);
                    command.Parameters["@IdPartido"].Value = partido.IdPartido;

                    command.Parameters.Add("@IdUsuario", SqlDbType.NChar);
                    command.Parameters["@IdUsuario"].Value = pronostico.Key;

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

        public static Dictionary<int, Partidos> GetPronosticoByPartidoByUser(int idPartido)
        {
            Dictionary<int, Partidos> listaPronosticos = new Dictionary<int, Partidos>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string selstr = "Select * FROM Pronosticos WHERE IdPartido = @IdPartido ORDEr BY IdUsuario";
                    SqlCommand cmd = new SqlCommand(selstr, conn);
                    cmd.Parameters.AddWithValue("@IdPartido", idPartido);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Partidos pronostico = new Partidos();

                        pronostico.IdPartido = Convert.ToInt32(reader["IdPartido"]);
                        pronostico.GolesLocal = Convert.ToInt32(reader["GolesLocal"]);
                        pronostico.GolesVisita = Convert.ToInt32(reader["GolesVisita"]);
                        pronostico.IdPaisGanador = Convert.ToInt32(reader["IdPaisGanador"]);

                        int idUsuario = Convert.ToInt32(reader["IdUsuario"]);

                        listaPronosticos.Add(idUsuario, pronostico);
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

            return listaPronosticos;
        }



        /// <summary>
        /// Actualiza la información de los grupos de acuerdo a el resultado 
        /// del partido que se esta guardando
        /// </summary>
        /// <param name="partido">Resultado del partido jugado</param>
        /// <param name="tipo">Que equipo se actualizará 1. Local 2. Visita</param>
        /// <param name="idPais">Identificador del pais que se actualizara</param>
        /// <param name="torneo">Información del torneo que se esta jugando</param>
        public static void UpdateGruposInfo(Partidos partido, int tipo, int idPais,Torneos torneo)
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
                String query = "UPDATE Grupos Set Ganados = (SELECT Ganados + @Ganados FROM Grupos WHERE idPais = @IdPais AND IdTorneo = @IdTorneo), " +
                               "Perdidos = (SELECT Perdidos + @Perdidos FROM Grupos WHERE idPais = @IdPais AND IdTorneo = @IdTorneo), " +
                               "Empates = (SELECT Empates + @Empates FROM Grupos WHERE idPais = @IdPais AND IdTorneo = @IdTorneo), " +
                               "GolesFavor = (SELECT GolesFavor + @GolesFavor FROM Grupos WHERE idPais = @IdPais AND IdTorneo = @IdTorneo), " +
                               "GolesContra = (SELECT GolesContra + @GolesContra FROM Grupos WHERE idPais = @IdPais AND IdTorneo = @IdTorneo), " +
                               "Puntos = (SELECT Puntos + @Puntos FROM Grupos WHERE idPais = @IdPais AND IdTorneo = @IdTorneo) " +
                               "WHERE IdPais = @IdPais AND IdTorneo = @IdTorneo";

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

                command.Parameters.Add("@IdTorneo", SqlDbType.NChar);
                command.Parameters["@IdTorneo"].Value = torneo.IdTorneo;

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
        public static void UpdateDiferenciaGoles(Torneos torneo)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                String query = "UPDATE G SET Diferencia = t2.NuevaDiferencia 	FROM Grupos G JOIN " +
                                "(SELECT idPais, GolesFavor - GolesContra as NuevaDiferencia FROM Grupos t2 WHERE IdTorneo = @IdTorneo  GROUP BY idPais,GolesFavor,GolesContra) t2 " +
                                "ON G.IdPais = t2.IdPais WHERE IdTorneo = @IdTorneo ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdTorneo", torneo.IdTorneo);
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



        public static void SetNewPartido(Partidos partido)
        {
            SqlConnection thisConnection = new SqlConnection(connectionString);

            //Create Command object
            SqlCommand nonqueryCommand = thisConnection.CreateCommand();

            try
            {
                // Open Connection
                thisConnection.Open();
                Console.WriteLine("Connection Opened");

                // Create INSERT statement with named parameters
                nonqueryCommand.CommandText = "INSERT  INTO Partidos (Fecha, idPaisLocal,idPaisVisita,IdTorneo) VALUES (@Fecha, @idPaisLocal,@idPaisVisita,@IdTorneo)";

                // Add Parameters to Command Parameters collection
                nonqueryCommand.Parameters.Add("@Fecha", SqlDbType.DateTime, 0);
                nonqueryCommand.Parameters.Add("@idPaisLocal", SqlDbType.Int, 0);
                nonqueryCommand.Parameters.Add("@idPaisVisita", SqlDbType.Int, 0);
                nonqueryCommand.Parameters.Add("@IdTorneo", SqlDbType.Int, 0);

                // Prepare command for repeated execution
                nonqueryCommand.Prepare();

                // Data to be inserted
                nonqueryCommand.Parameters["@Fecha"].Value = partido.Fecha;
                nonqueryCommand.Parameters["@idPaisLocal"].Value = partido.IdPaisLocal;
                nonqueryCommand.Parameters["@idPaisVisita"].Value = partido.IdPaisVisita;
                nonqueryCommand.Parameters["@IdTorneo"].Value = partido.IdTorneo;

                Console.WriteLine("Executing {0}", nonqueryCommand.CommandText);
                Console.WriteLine("Number of rows affected : {0}", nonqueryCommand.ExecuteNonQuery());
            }
            catch (SqlException ex)
            {
                // Display error
                Console.WriteLine("Error: " + ex.ToString());
            }
            finally
            {
                // Close Connection
                thisConnection.Close();
                Console.WriteLine("Connection Closed");
            }


        }

    }
}