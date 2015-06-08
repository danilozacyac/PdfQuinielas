using System;
using System.Collections.ObjectModel;
using System.Linq;
using PdfQuinielas.Dao;
using PdfQuinielas.Models;

namespace PdfQuinielas.Singleton
{
    public class TorneosSingleton
    {

        private static ObservableCollection<Torneos> torneos;

        private TorneosSingleton() { }

        public static ObservableCollection<Torneos> Torneos
        {
            get
            {
                if (torneos == null)
                    torneos = new TorneosModel().GetTorneos();

                return torneos;
            }
        }
    }
}
