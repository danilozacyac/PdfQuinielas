using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiniela.Dao
{
    public class Equipos
    {
        private int idEquipo;
        private string equipo;
        private int idConfederacion;
        private int tipo;
        
        public Equipos(int idEquipo, string equipo, int idConfederacion, int tipo)
        {
            this.idEquipo = idEquipo;
            this.equipo = equipo;
            this.idConfederacion = idConfederacion;
            this.tipo = tipo;
        }

        public int IdEquipo
        {
            get
            {
                return this.idEquipo;
            }
            set
            {
                this.idEquipo = value;
            }
        }

        public string Equipo
        {
            get
            {
                return this.equipo;
            }
            set
            {
                this.equipo = value;
            }
        }

        public int IdConfederacion
        {
            get
            {
                return this.idConfederacion;
            }
            set
            {
                this.idConfederacion = value;
            }
        }

        public int Tipo
        {
            get
            {
                return this.tipo;
            }
            set
            {
                this.tipo = value;
            }
        }
    }
}