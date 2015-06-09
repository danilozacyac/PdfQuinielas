using System;
using System.Linq;

namespace PdfQuinielas.Dao
{
    public class Torneos
    {
        private int idTorneo;
        private string torneo;
        private int year;
        private DateTime? fechaInicio;

        public int IdTorneo
        {
            get
            {
                return this.idTorneo;
            }
            set
            {
                this.idTorneo = value;
            }
        }

        public string Torneo
        {
            get
            {
                return this.torneo;
            }
            set
            {
                this.torneo = value;
            }
        }

        public int Year
        {
            get
            {
                return this.year;
            }
            set
            {
                this.year = value;
            }
        }

        public DateTime? FechaInicio
        {
            get
            {
                return this.fechaInicio;
            }
            set
            {
                this.fechaInicio = value;
            }
        }

        
    }
}
