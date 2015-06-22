using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Quiniela.Dao
{
    public class GruposDao
    {
        private char grupo;
        private string pais;
        private int jGanados;
        private int jPerdidos;
        private int jEmpatados;
        private int gFavor;
        private int gContra;
        private int gDiferencia;
        private int puntos;

        public GruposDao() { }

        public GruposDao(char grupo)
        {
            this.grupo = grupo;
        }

        

        public char Grupo
        {
            get
            {
                return this.grupo;
            }
            set
            {
                this.grupo = value;
            }
        }

        public string Pais
        {
            get
            {
                return this.pais;
            }
            set
            {
                this.pais = value;
            }
        }

        public int JGanados
        {
            get
            {
                return this.jGanados;
            }
            set
            {
                this.jGanados = value;
            }
        }

        public int JPerdidos
        {
            get
            {
                return this.jPerdidos;
            }
            set
            {
                this.jPerdidos = value;
            }
        }

        public int JEmpatados
        {
            get
            {
                return this.jEmpatados;
            }
            set
            {
                this.jEmpatados = value;
            }
        }

        public int GFavor
        {
            get
            {
                return this.gFavor;
            }
            set
            {
                this.gFavor = value;
            }
        }

        public int GContra
        {
            get
            {
                return this.gContra;
            }
            set
            {
                this.gContra = value;
            }
        }

        public int GDiferencia
        {
            get
            {
                return this.gDiferencia;
            }
            set
            {
                this.gDiferencia = value;
            }
        }

        public int Puntos
        {
            get
            {
                return this.puntos;
            }
            set
            {
                this.puntos = value;
            }
        }

        ObservableCollection<GruposDao> listaGrupos ;
        public ObservableCollection<GruposDao> ListaGrupos
        {
            get
            {
                return this.listaGrupos;
            }
            set
            {
                this.listaGrupos = value;
            }
        }
        public ObservableCollection<GruposDao> GetGrupos()
        {
            listaGrupos = new ObservableCollection<GruposDao>();

            listaGrupos.Add(new GruposDao('A'));
            listaGrupos.Add(new GruposDao('B'));
            listaGrupos.Add(new GruposDao('C'));
            listaGrupos.Add(new GruposDao('D'));
            listaGrupos.Add(new GruposDao('E'));
            listaGrupos.Add(new GruposDao('F'));
            listaGrupos.Add(new GruposDao('G'));
            listaGrupos.Add(new GruposDao('H'));

            return listaGrupos;
        }
    
    }
}