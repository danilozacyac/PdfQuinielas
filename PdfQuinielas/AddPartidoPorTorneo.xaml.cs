using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using PdfQuinielas.Dao;
using PdfQuinielas.Models;
using Quiniela.Dao;
using Quiniela.Models;

namespace PdfQuinielas
{
    /// <summary>
    /// Interaction logic for AddPartidoPorTorneo.xaml
    /// </summary>
    public partial class AddPartidoPorTorneo
    {
        private Torneos selectedTorneo;

        public AddPartidoPorTorneo(Torneos selectedTorneo)
        {
            InitializeComponent();
            this.selectedTorneo = selectedTorneo;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Equipos> listaEquipos = EquiposModel.GetEquipos();

            RCbxEquipoLocal.DataContext = listaEquipos;
            RCbxEquipoVisita.DataContext = listaEquipos;
        }

        private void RBtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            Partidos partido = new Partidos();
            partido.Fecha = RdtFechaP.SelectedDate;
            partido.IdPaisLocal = Convert.ToInt32(RCbxEquipoLocal.SelectedValue);
            partido.IdPaisVisita = Convert.ToInt32(RCbxEquipoVisita.SelectedValue);
            partido.IdTorneo = selectedTorneo.IdTorneo;

            PartidosModel.SetNewPartido(partido);

            this.Close();
        }

        private void RBtnContinuar_Click(object sender, RoutedEventArgs e)
        {
            Partidos partido = new Partidos();
            partido.Fecha = RdtFechaP.SelectedDate;
            partido.IdPaisLocal = Convert.ToInt32(RCbxEquipoLocal.SelectedValue);
            partido.IdPaisVisita = Convert.ToInt32(RCbxEquipoVisita.SelectedValue);
            partido.IdTorneo = selectedTorneo.IdTorneo;

            PartidosModel.SetNewPartido(partido);

            RCbxEquipoLocal.SelectedIndex = -1;
            RCbxEquipoVisita.SelectedIndex = -1;
        }
    }
}
