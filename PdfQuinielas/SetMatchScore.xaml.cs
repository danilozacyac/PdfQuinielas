using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PdfQuinielas.Dao;
using Quiniela.Dao;
using Quiniela.Models;
using Telerik.Windows.Controls;

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
            PartidosModel.UpdateDiferenciaGoles();
        }
    }
}
