using System;
using System.Collections.ObjectModel;
using System.Linq;
using PdfQuinielas.Models;
using Quiniela.Dao;

namespace PdfQuinielas.Singleton
{
    public class EquiposSingleton
    {
        private static ObservableCollection<Equipos> equipos;

        private EquiposSingleton() { }

        public static ObservableCollection<Equipos> EquiposSin
        {
            get
            {
                if (equipos == null)
                    equipos = EquiposModel.GetEquipos();

                return equipos;
            }
        }
    }
}
