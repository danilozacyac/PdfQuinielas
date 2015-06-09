using System;
using System.Linq;
using System.Windows;
using PdfQuinielas.Dao;
using Quiniela.Dao;

namespace PdfQuinielas
{
    /// <summary>
    /// Interaction logic for AddPartidoPorTorneo.xaml
    /// </summary>
    public partial class AddPartidoPorTorneo
    {
        public AddPartidoPorTorneo()
        {
            InitializeComponent();
        }

        private void RBtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            Partidos partido = new Partidos();
            partido.Fecha = RdtFechaP.SelectedDate;
            partido.IdEquipoLocal = Convert.ToInt32(RCbxEquipoLocal.SelectedValue);
            partido.IdEquipoVisita = Convert.ToInt32(RCbxEquipoVisita.SelectedValue);
            partido.IdTorneo = Convert.ToInt32(RCbxTorneo.SelectedValue);

            Partidos.SetPartido(partido);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RCbxEquipoLocal.DataContext = Equipos.GetEquipos();
            RCbxEquipoVisita.DataContext = Equipos.GetEquipos();
            RCbxTorneo.DataContext = Torneos.GetEquipos();

        }
    }
}
