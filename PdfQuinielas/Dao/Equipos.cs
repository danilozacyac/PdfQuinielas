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

        public Equipos(int idEquipo, string equipo)
        {
            this.idEquipo = idEquipo;
            this.equipo = equipo;
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
    }
}