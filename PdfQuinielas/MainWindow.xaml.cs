using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PdfQuinielas.Singleton;
using Quiniela.Dao;

namespace PdfQuinielas
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RCbxUsuarios.DataContext = UsuariosSingleton.UsuariosSin;
        }

        private void RBtnResPersonal_Click(object sender, RoutedEventArgs e)
        {
            Usuarios user = RCbxUsuarios.SelectedItem as Usuarios;

            ResultadosEnPdf pdf = new ResultadosEnPdf();
            pdf.ResultadosPorUsuario(user);
        }

        private void RBtnConcentrado_Click(object sender, RoutedEventArgs e)
        {
            ResultadosEnPdf pdf = new ResultadosEnPdf();
            pdf.ConcentradoResultados();
        }

        private void RBtnConcenCompleto_Click(object sender, RoutedEventArgs e)
        {
            ResultadosConcentradoPdf pdf = new ResultadosConcentradoPdf();
            pdf.ResultadosPorUsuario();
        }
    }
}
