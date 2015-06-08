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

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["QuinielaMundialConnectionString"].ConnectionString;


        /// <summary>
        /// Obtiene la lista de participantes que se han inscrito en al menos un torneo
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Usuarios> GetUsuarios()
        {
            ObservableCollection<Usuarios> listaUsuarios = new ObservableCollection<Usuarios>();

            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string selstr = "SELECT * FROM Usuarios Order By IdUsuario";
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
                        usuario.Usuario = reader["Nickname"].ToString();
                        usuario.Mail = reader["Mail"].ToString();
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


        /// <summary>
        /// Obtiene el listado de usuarios participantes en un torneo en particular
        /// </summary>
        /// <param name="idTorneo">Identificador del torneo</param>
        /// <returns></returns>
        public ObservableCollection<Usuarios> GetUsuarios(int idTorneo)
        {
            ObservableCollection<Usuarios> listaUsuarios = new ObservableCollection<Usuarios>();

            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string selstr = "SELECT * FROM usuarios WHERE idusuario IN (SELECT idUsuario FROM Pronosticos WHERE IdTorneo = @IdTorneo GROUP BY IdUsuario)";
                    SqlCommand cmd = new SqlCommand(selstr, conn);
                    cmd.Parameters.AddWithValue("@IdTorneo", idTorneo);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Usuarios usuario = new Usuarios();

                        usuario.Idusuario = reader["IdUsuario"] as int? ?? -1;
                        usuario.Paterno = reader["Paterno"].ToString();
                        usuario.Materno = reader["Materno"].ToString();
                        usuario.Nombre = reader["Nombre"].ToString();
                        usuario.NombreCompleto = usuario.Nombre + " " + usuario.Paterno + " " + usuario.Materno;
                        usuario.Usuario = reader["Nickname"].ToString();
                        usuario.Mail = reader["Mail"].ToString();
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
