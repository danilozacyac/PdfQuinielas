using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Quiniela.Dao;

namespace PdfQuinielas.Models
{
    public class UsuariosModel
    {

        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["QuinielaMundialConnectionString"].ConnectionString;

        public static ObservableCollection<Usuarios> GetUsuarios()
        {
            ObservableCollection<Usuarios> listaUsuarios = new ObservableCollection<Usuarios>();

            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string selstr = "SELECT idUsuario,Paterno,Materno,Nombre,CreateDate FROM Usuarios Order By IdUsuario";
                    SqlCommand cmd = new SqlCommand(selstr, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Usuarios usuario = new Usuarios();

                        usuario.Idusuario = reader["IdUsuario"] as int? ?? -1;
                        usuario.Paterno = reader["Paterno"].ToString();
                        usuario.Materno = reader["Materno"].ToString();
                        usuario.Nombre = reader["Nombre"].ToString();
                        usuario.NombreCompleto = usuario.Nombre + " " + usuario.Paterno + " " + usuario.Materno;
                        usuario.FechaRegistro = Convert.ToDateTime(reader["CreateDate"]);

                        listaUsuarios.Add(usuario);

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

            return listaUsuarios;
        }
    }
}
