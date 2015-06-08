using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfQuinielas.Models;
using Quiniela.Dao;

namespace PdfQuinielas.Singleton
{
    public class UsuariosSingleton
    {
        private static ObservableCollection<Usuarios> usuarios;

        private UsuariosSingleton() { }

        public static ObservableCollection<Usuarios> UsuariosSin
        {
            get
            {
                if (usuarios == null)
                    usuarios = new UsuariosModel().GetUsuarios();

                return usuarios;
            }
        }
    }
}
