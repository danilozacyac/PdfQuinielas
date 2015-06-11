using System;
using System.Linq;
using System.Windows;
using PdfQuinielas.Dao;
using Quiniela.Dao;
using Quiniela.Models;

namespace PdfQuinielas
{
    /// <summary>
    /// Interaction logic for SetMatchScore.xaml
    /// </summary>
    public partial class SetMatchScore
    {
        private Torneos torneo;

        public SetMatchScore(Torneos torneo)
        {
            InitializeComponent();
            this.torneo = torneo;
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RCbxPartido.DataContext = PartidosModel.GetPartidosForSettingScore(torneo);
        }

        private void BtnResultado_Click(object sender, RoutedEventArgs e)
        {
            Partidos partido = RCbxPartido.SelectedItem as Partidos;

            partido.GolesLocal = Convert.ToInt16(TxtLocal.Text);
            partido.GolesVisita = Convert.ToInt16(TxtVisitante.Text);

            if (partido.GolesLocal > partido.GolesVisita)
                partido.IdPaisGanador = partido.IdPaisLocal;
            else if (partido.GolesVisita > partido.GolesLocal)
                partido.IdPaisGanador = partido.IdPaisVisita;
            else
                partido.IdPaisGanador = 999;

            PartidosModel.UpdatePartidosScore(partido);
            PartidosModel.UpdatePronosticosPuntosGanadosConMarcador(partido);
            PartidosModel.UpdateGruposInfo(partido, 1, partido.IdPaisLocal, torneo);
            PartidosModel.UpdateGruposInfo(partido, 2, partido.IdPaisVisita, torneo);
            PartidosModel.UpdateDiferenciaGoles(torneo);
        }
    }
}
