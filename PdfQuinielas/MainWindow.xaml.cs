using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using PdfQuinielas.Dao;
using PdfQuinielas.Models;
using PdfQuinielas.Reportes;
using PdfQuinielas.Singleton;
using Quiniela.Dao;

namespace PdfQuinielas
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Torneos selectedTorneo;
        private ObservableCollection<Usuarios> listaUsuarios;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            RCbxTorneos.DataContext = TorneosSingleton.Torneos;
        }

        private void RBtnResPersonal_Click(object sender, RoutedEventArgs e)
        {
            Usuarios user = RCbxUsuarios.SelectedItem as Usuarios;

            ResultadosEnPdf pdf = new ResultadosEnPdf();
            pdf.ResultadosPorUsuario(user,selectedTorneo);
        }

        private void RBtnConcentrado_Click(object sender, RoutedEventArgs e)
        {
            ResultadosEnPdf pdf = new ResultadosEnPdf();
            pdf.ConcentradoResultados(selectedTorneo);
        }

        private void RBtnConcenCompleto_Click(object sender, RoutedEventArgs e)
        {
            ResultadosConcentradoPdf pdf = new ResultadosConcentradoPdf();
            pdf.ResultadosPorUsuario(selectedTorneo);
        }

        private void RCbxTorneos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTorneo = RCbxTorneos.SelectedItem as Torneos;
            listaUsuarios = new UsuariosModel().GetUsuarios(selectedTorneo.IdTorneo);

            RCbxUsuarios.DataContext = listaUsuarios;

            LblNumParticipantes.Content = listaUsuarios.Count + " participantes";
        }

        private void RadRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            MailSender.MailNoAttachment();
        }

        private void BtnUpdateTorneo_Click(object sender, RoutedEventArgs e)
        {
            TournamentManager update = new TournamentManager(selectedTorneo);
            update.ShowDialog();
        }

        private void BtnAddTorneo_Click(object sender, RoutedEventArgs e)
        {
            TournamentManager add = new TournamentManager();
            add.ShowDialog();
        }

        private void BtnAgregaPartido_Click(object sender, RoutedEventArgs e)
        {
            AddPartidoPorTorneo add = new AddPartidoPorTorneo(selectedTorneo);
            add.ShowDialog();
        }

        private void BtnSetScore_Click(object sender, RoutedEventArgs e)
        {
            SetMatchScore score = new SetMatchScore(selectedTorneo);
            score.ShowDialog();
        }

        private void BtnEnviarConcentrado_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            bool? userClickOk = dialog.ShowDialog();

            if (userClickOk == true)
            {
                MailSender.EnviarConcentradoQuiniela(dialog.FileName, selectedTorneo);
            }
        }

        private void BtnParticipantes_Click(object sender, RoutedEventArgs e)
        {
            ListaParticipantes participantes = new ListaParticipantes(selectedTorneo);
            participantes.ResultadosPorUsuario();
        }

        private void BtnAddGrupo_Click(object sender, RoutedEventArgs e)
        {
            CrearGrupo create = new CrearGrupo(selectedTorneo);
            create.ShowDialog();
        }
    }
}
