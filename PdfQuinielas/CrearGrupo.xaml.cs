using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using PdfQuinielas.Dao;
using PdfQuinielas.Models;
using Quiniela.Dao;

namespace PdfQuinielas
{
    /// <summary>
    /// Lógica de interacción para CrearGrupo.xaml
    /// </summary>
    public partial class CrearGrupo : Window
    {
        private Torneos torneo;
        private ObservableCollection<Equipos> listaEquipos;

        public CrearGrupo(Torneos torneo)
        {
            InitializeComponent();
            this.torneo = torneo;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RCbxGrupo.DataContext = new GruposDao().GetGrupos();

            listaEquipos = EquiposModel.GetEquipos();

            RCbxEquipo1.DataContext = listaEquipos;
            RCbxEquipo2.DataContext = listaEquipos;
            RCbxEquipo3.DataContext = listaEquipos;
            RCbxEquipo4.DataContext = listaEquipos;
            RCbxEquipo5.DataContext = listaEquipos;
            RCbxEquipo6.DataContext = listaEquipos;

        }

        private void RBtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (RCbxEquipo1.SelectedIndex != -1)
                new GruposModel().SetNewGrupo(torneo, Convert.ToChar(RCbxGrupo.SelectedValue), Convert.ToInt32(RCbxEquipo1.SelectedValue));
            if (RCbxEquipo2.SelectedIndex != -1)
                new GruposModel().SetNewGrupo(torneo, Convert.ToChar(RCbxGrupo.SelectedValue), Convert.ToInt32(RCbxEquipo2.SelectedValue));
            if (RCbxEquipo3.SelectedIndex != -1)
                new GruposModel().SetNewGrupo(torneo, Convert.ToChar(RCbxGrupo.SelectedValue), Convert.ToInt32(RCbxEquipo3.SelectedValue));
            if (RCbxEquipo4.SelectedIndex != -1)
                new GruposModel().SetNewGrupo(torneo, Convert.ToChar(RCbxGrupo.SelectedValue), Convert.ToInt32(RCbxEquipo4.SelectedValue));
            if (RCbxEquipo5.SelectedIndex != -1)
                new GruposModel().SetNewGrupo(torneo, Convert.ToChar(RCbxGrupo.SelectedValue), Convert.ToInt32(RCbxEquipo5.SelectedValue));
            if (RCbxEquipo6.SelectedIndex != -1)
                new GruposModel().SetNewGrupo(torneo, Convert.ToChar(RCbxGrupo.SelectedValue), Convert.ToInt32(RCbxEquipo6.SelectedValue));

            this.Close();
        }
    }
}
