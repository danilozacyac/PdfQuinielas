using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfQuinielas.Dao
{
    public class UserResults
    {
        private int idUsuario;
        private DateTime fecha;
        private string local;
        private string visitante;
        private string ganador;
        private int golesLocal;
        private int golesVisita;

        public int IdUsuario
        {
            get
            {
                return this.idUsuario;
            }
            set
            {
                this.idUsuario = value;
            }
        }

        public DateTime Fecha
        {
            get
            {
                return this.fecha;
            }
            set
            {
                this.fecha = value;
            }
        }

        public string Local
        {
            get
            {
                return this.local;
            }
            set
            {
                this.local = value;
            }
        }

        public string Visitante
        {
            get
            {
                return this.visitante;
            }
            set
            {
                this.visitante = value;
            }
        }

        public string Ganador
        {
            get
            {
                return this.ganador;
            }
            set
            {
                this.ganador = value;
            }
        }

        public int GolesLocal
        {
            get
            {
                return this.golesLocal;
            }
            set
            {
                this.golesLocal = value;
            }
        }

        public int GolesVisita
        {
            get
            {
                return this.golesVisita;
            }
            set
            {
                this.golesVisita = value;
            }
        }
    }
}
