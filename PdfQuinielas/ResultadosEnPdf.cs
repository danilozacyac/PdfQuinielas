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

namespace PdfQuinielas
{
    public class ResultadosEnPdf
    {
        private iTextSharp.text.Document myDocument;
        //private Paragraph para;


        public void ResultadosPorUsuario(int idUsuario)
        {
            myDocument = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
            string documento = Path.GetTempFileName() + ".pdf";

            ObservableCollection<UserResults> userResults = UserResultsModel.GetResultadosPorUsuario(idUsuario);

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(documento, FileMode.Create));

                myDocument.Open();


                myDocument.Add(this.GetUserResultTable(userResults));

                
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


        private PdfPTable GetUserResultTable(ObservableCollection<UserResults> userResults)
        {
            PdfPTable table = new PdfPTable(5);
            //table.TotalWidth = 400;
            table.WidthPercentage = 100;

            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            string[] encabezado = { "Partido", "Fecha", "Local", "Visitante", "Ganador"};
            PdfPCell cell;

            foreach (string cabeza in encabezado)
            {
                cell = new PdfPCell(new Phrase(cabeza, Fuentes.EncabezadoColumna));
                cell.Colspan = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);
            }

            int consecPartido = 1;
            foreach (UserResults user in userResults)
            {
                string[] descs = { consecPartido.ToString(), user.Fecha.ToShortDateString(), user.Local, user.Visitante, user.Ganador};

                foreach (string desc in descs)
                {
                    cell = new PdfPCell(new Phrase(desc, this.GetWinnerFont(user)));
                    cell.Colspan = 0;
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell);
                }

                consecPartido++;
            }

            return table;
        }

        private Font GetWinnerFont(UserResults user)
        {
            if (user.Ganador.Equals(user.Local))
                return Fuentes.GanadorLocal;
            else if (user.Ganador.Equals(user.Visitante))
                return Fuentes.GanadorVisita;
            else
                return Fuentes.Empate;

        }

    }
}
