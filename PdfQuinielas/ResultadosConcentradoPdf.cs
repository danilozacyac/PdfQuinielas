using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using PdfQuinielas.Dao;
using PdfQuinielas.Models;
using Quiniela.Dao;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfQuinielas.Singleton;

namespace PdfQuinielas
{
    public class ResultadosConcentradoPdf
    {
        private iTextSharp.text.Document myDocument;

        public void ResultadosPorUsuario(Usuarios usuario)
        {
            myDocument = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
            string documento = Path.GetTempFileName() + ".pdf";

            ObservableCollection<UserResults> userResults = UserResultsModel.GetResultadosPorUsuario(usuario.Idusuario);

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(documento, FileMode.Create));

                myDocument.Open();


                Paragraph para = new Paragraph("Quiniela Brasil 2014", Fuentes.Encabezados);
                para.Alignment = 1;
                myDocument.Add(para);

                //para = new Paragraph(usuario.NombreCompleto, Fuentes.OtrosDatos);
                para = new Paragraph(" ", Fuentes.OtrosDatos);
                para.Alignment = 0;
                myDocument.Add(para);

                //para = new Paragraph("Fecha de registro: " + usuario.FechaRegistro, Fuentes.OtrosDatos);
                para = new Paragraph(" ", Fuentes.OtrosDatos);
                para.Alignment = 0;
                myDocument.Add(para);

                myDocument.Add(this.GetUserResultTable(userResults));

                myDocument.Add(this.GetResultadosMexico(userResults));



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
            PdfPTable table = new PdfPTable(49);
            //table.TotalWidth = 400;
            table.WidthPercentage = 100;

            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            PdfPCell cell;

            cell = new PdfPCell(new Phrase("Nombre", Fuentes.EncabezadoColumna));

            for(int x = 1; x < 48; x++)
            {
                cell = new PdfPCell(new Phrase(x.ToString(), Fuentes.EncabezadoColumna));
                cell.Colspan = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);
            }

            ObservableCollection<Usuarios> listaUsuarios = UsuariosSingleton.UsuariosSin;

            foreach (Usuarios user in listaUsuarios)
            {



            }



            int consecPartido = 1;
            foreach (UserResults user in userResults)
            {


                cell = new PdfPCell(new Phrase(consecPartido.ToString(), Fuentes.OtrosDatos));
                cell.Colspan = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(user.Fecha.ToShortDateString(), Fuentes.OtrosDatos));
                cell.Colspan = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(user.Local, Fuentes.OtrosDatos));
                cell.Colspan = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(user.Visitante, Fuentes.OtrosDatos));
                cell.Colspan = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(user.Ganador, this.GetWinnerFont(user)));
                //cell = new PdfPCell(new Phrase(" ", this.GetWinnerFont(user)));
                cell.Colspan = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);



                consecPartido++;
            }

            return table;
        }
    }
}
