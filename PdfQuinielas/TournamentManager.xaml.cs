using System;
using System.Linq;
using System.Windows;
using PdfQuinielas.Dao;
using PdfQuinielas.Models;
using PdfQuinielas.Singleton;

namespace PdfQuinielas
{
    /// <summary>
    /// Interaction logic for TournamentManager.xaml
    /// </summary>
    public partial class TournamentManager
    {
        private Torneos torneo;
        private bool isUpdating;

        public TournamentManager()
        {
            InitializeComponent();
            torneo = new Torneos();
        }

        public TournamentManager(Torneos torneo)
        {
            InitializeComponent();
            this.torneo = torneo;
            isUpdating = true;
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = torneo;
        }

        private void RBtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (isUpdating)
            {
                new TorneosModel().UpdateTorneo(torneo);
            }
            else
            {
                new TorneosModel().SetNewTorneo(torneo);
                TorneosSingleton.Torneos.Add(torneo);
            }
            this.Close();
        }
    }
}
