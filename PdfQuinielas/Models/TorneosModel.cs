using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using PdfQuinielas.Dao;
using ScjnUtilities;

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

        public void SetNewTorneo(Torneos torneo)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                torneo.IdTorneo = DataBaseUtilities.GetNextIdForUse("Torneos", "IdTorneo", connection);


                string sqlCadena = "SELECT * FROM Torneos WHERE IdTorneo = 0";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Torneos");

                dr = dataSet.Tables["Torneos"].NewRow();
                dr["IdTorneo"] = torneo.IdTorneo;
                dr["Torneo"] = torneo.Torneo;
                dr["Year"] = torneo.FechaInicio.Value.Year;
                dr["FechaInicio"] = torneo.FechaInicio;


                dataSet.Tables["Torneos"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();

                dataAdapter.InsertCommand.CommandText = "INSERT INTO Torneos VALUES (@IdTorneo,@Torneo,@Year,@FechaInicio)";
                dataAdapter.InsertCommand.Parameters.Add("@IdTorneo", SqlDbType.Int, 0, "IdTorneo");
                dataAdapter.InsertCommand.Parameters.Add("@Torneo", SqlDbType.VarChar, 0, "Torneo");
                dataAdapter.InsertCommand.Parameters.Add("@Year", SqlDbType.Int, 0, "Year");
                dataAdapter.InsertCommand.Parameters.Add("@FechaInicio", SqlDbType.Date, 0, "FechaInicio");

                dataAdapter.Update(dataSet, "Torneos");
                dataSet.Dispose();
                dataAdapter.Dispose();

             
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButton.OK, MessageBoxImage.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButton.OK, MessageBoxImage.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                connection.Close();
            }
        }

        public void UpdateTorneo(Torneos torneo)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Torneos WHERE IdTorneo = " + torneo.IdTorneo;

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);
                dataAdapter.Fill(dataSet, "Torneos");

                dr = dataSet.Tables["Torneos"].Rows[0];
                dr.BeginEdit();
                dr["Torneo"] = torneo.Torneo;
                dr["Year"] = torneo.FechaInicio.Value.Year;
                dr["FechaInicio"] = torneo.FechaInicio;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();

                dataAdapter.UpdateCommand.CommandText = "UPDATE Torneos SET Torneo = @Torneo, Year = @Year, FechaInicio = @FechaInicio WHERE IdTorneo = @IdTorneo";

                dataAdapter.UpdateCommand.Parameters.Add("@Torneo", SqlDbType.VarChar, 0, "Torneo");
                dataAdapter.UpdateCommand.Parameters.Add("@Year", SqlDbType.Int, 0, "Year");
                dataAdapter.UpdateCommand.Parameters.Add("@FechaInicio", SqlDbType.Date, 0, "FechaInicio");
                dataAdapter.UpdateCommand.Parameters.Add("@IdTorneo", SqlDbType.Int, 0, "IdTorneo");

                dataAdapter.Update(dataSet, "Torneos");
                dataSet.Dispose();
                dataAdapter.Dispose();


            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButton.OK, MessageBoxImage.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButton.OK, MessageBoxImage.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
