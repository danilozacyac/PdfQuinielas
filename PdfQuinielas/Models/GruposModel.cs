using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PdfQuinielas.Dao;

namespace PdfQuinielas.Models
{
    public class GruposModel
    {

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["QuinielaMundialConnectionString"].ConnectionString;

        public void SetNewGrupo(Torneos torneo, char grupo, int idEquipo )
        {
            SqlConnection connection = new SqlConnection(connectionString);

            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Grupos WHERE IdTorneo = 0";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Grupos");

                dr = dataSet.Tables["Grupos"].NewRow();
                dr["Grupo"] = grupo;
                dr["IdPais"] = idEquipo;
                dr["Ganados"] = 0;
                dr["Perdidos"] = 0;
                dr["Empates"] = 0;
                dr["GolesFavor"] = 0;
                dr["GolesContra"] = 0;
                dr["Diferencia"] = 0;
                dr["Puntos"] = 0;
                dr["IdTorneo"] = torneo.IdTorneo;

                dataSet.Tables["Grupos"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();

                dataAdapter.InsertCommand.CommandText = "INSERT INTO Grupos VALUES (@Grupo,@IdPais,@Ganados,@Perdidos,@Empates,@GolesFavor,@GolesContra,@Diferencia,@Puntos,@IdTorneo)";
                dataAdapter.InsertCommand.Parameters.Add("@Grupo", SqlDbType.Char, 0, "Grupo");
                dataAdapter.InsertCommand.Parameters.Add("@IdPais", SqlDbType.Int, 0, "IdPais");
                dataAdapter.InsertCommand.Parameters.Add("@Ganados", SqlDbType.Int, 0, "Ganados");
                dataAdapter.InsertCommand.Parameters.Add("@Perdidos", SqlDbType.Int, 0, "Perdidos");
                dataAdapter.InsertCommand.Parameters.Add("@Empates", SqlDbType.Int, 0, "Empates");
                dataAdapter.InsertCommand.Parameters.Add("@GolesFavor", SqlDbType.Int, 0, "GolesFavor");
                dataAdapter.InsertCommand.Parameters.Add("@GolesContra", SqlDbType.Int, 0, "GolesContra");
                dataAdapter.InsertCommand.Parameters.Add("@Diferencia", SqlDbType.Int, 0, "Diferencia");
                dataAdapter.InsertCommand.Parameters.Add("@Puntos", SqlDbType.Int, 0, "Puntos");
                dataAdapter.InsertCommand.Parameters.Add("@IdTorneo", SqlDbType.Int, 0, "IdTorneo");

                dataAdapter.Update(dataSet, "Grupos");
                dataSet.Dispose();
                dataAdapter.Dispose();


            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButton.OK, MessageBoxImage.Warning);
                //ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButton.OK, MessageBoxImage.Warning);
                //ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
