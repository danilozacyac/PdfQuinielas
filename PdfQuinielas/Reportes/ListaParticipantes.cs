using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfQuinielas.Dao;
using PdfQuinielas.Models;
using Quiniela.Dao;

namespace PdfQuinielas.Reportes
{
    public class ListaParticipantes
    {
        private Torneos torneo;
        private iTextSharp.text.Document myDocument;

        public ListaParticipantes(Torneos torneo)
        {
            this.torneo = torneo;
        }

        public void ResultadosPorUsuario()
        {
            myDocument = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
            string documento = Path.GetTempFileName() + ".pdf";

            ObservableCollection<Usuarios>  listaUsuarios = new UsuariosModel().GetUsuarios(torneo.IdTorneo);

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(documento, FileMode.Create));

                myDocument.Open();


                Paragraph para = new Paragraph("Participantes Quiniela " + torneo.Torneo, Fuentes.Encabezados);
                para.Alignment = 1;
                myDocument.Add(para);

                para = new Paragraph(" ", Fuentes.OtrosDatos);
                para.Alignment = 0;
                myDocument.Add(para);

                //para = new Paragraph("Fecha de registro: " + usuario.FechaRegistro, Fuentes.OtrosDatos);
                para = new Paragraph(" ", Fuentes.OtrosDatos);
                para.Alignment = 0;
                myDocument.Add(para);

                myDocument.Add(this.GetUserResultTable(listaUsuarios));

                



            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                myDocument.Close();
                System.Diagnostics.Process.Start(documento);
            }
        }

        private PdfPTable GetUserResultTable(ObservableCollection<Usuarios> listaUsuarios)
        {
            PdfPTable table = new PdfPTable(3);
            //table.TotalWidth = 400;
            table.WidthPercentage = 100;

            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            string[] encabezado = {"#", "Partitipante", "Pagado" };
            PdfPCell cell;

            foreach (string cabeza in encabezado)
            {
                cell = new PdfPCell(new Phrase(cabeza, Fuentes.EncabezadoColumna));
                cell.Colspan = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);
            }

            int consecPartido = 1;
            foreach (Usuarios user in listaUsuarios)
            {


                cell = new PdfPCell(new Phrase(consecPartido.ToString(), Fuentes.OtrosDatos));
                cell.Colspan = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(user.NombreCompleto));
                cell.Colspan = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(" "));
                cell.Colspan = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                consecPartido++;
            }

            return table;
        }
    }
}
