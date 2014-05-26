using System;
using System.Linq;

namespace Quiniela.Dao
{
    public class Usuarios
    {
        private int idusuario;
        private string paterno;
        private string materno;
        private string nombre;
        private string usuario;
        private string mail;
        private int puntosObtenidos;
        private DateTime fechaRegistro;
        private DateTime fechaUltimoLogin;
        private string password;

        public int Idusuario
        {
            get
            {
                return this.idusuario;
            }
            set
            {
                this.idusuario = value;
            }
        }

        public string Paterno
        {
            get
            {
                return this.paterno;
            }
            set
            {
                this.paterno = value;
            }
        }

        public string Materno
        {
            get
            {
                return this.materno;
            }
            set
            {
                this.materno = value;
            }
        }

        public string Nombre
        {
            get
            {
                return this.nombre;
            }
            set
            {
                this.nombre = value;
            }
        }

        public string Usuario
        {
            get
            {
                return this.usuario;
            }
            set
            {
                this.usuario = value;
            }
        }

        public string Mail
        {
            get
            {
                return this.mail;
            }
            set
            {
                this.mail = value;
            }
        }

        public int PuntosObtenidos
        {
            get
            {
                return this.puntosObtenidos;
            }
            set
            {
                this.puntosObtenidos = value;
            }
        }

        public DateTime FechaRegistro
        {
            get
            {
                return this.fechaRegistro;
            }
            set
            {
                this.fechaRegistro = value;
            }
        }

        public DateTime FechaUltimoLogin
        {
            get
            {
                return this.fechaUltimoLogin;
            }
            set
            {
                this.fechaUltimoLogin = value;
            }
        }

        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }
    }
}