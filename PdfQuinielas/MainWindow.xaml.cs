using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PdfQuinielas.Dao;
using PdfQuinielas.Models;
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
            pdf.ResultadosPorUsuario();
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
    }
}
