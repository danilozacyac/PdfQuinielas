using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using PdfQuinielas.Dao;
using PdfQuinielas.Models;
using PdfQuinielas.Singleton;
using Quiniela.Dao;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfQuinielas
{
    public class ResultadosEnPdf
    {
        private iTextSharp.text.Document myDocument;
        //private Paragraph para;


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

        public void ConcentradoResultados()
        {
            myDocument = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
            string documento = Path.GetTempFileName() + ".pdf";

            

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(documento, FileMode.Create));

                myDocument.Open();

                foreach (Usuarios usuario in UsuariosSingleton.UsuariosSin)
                {

                    ObservableCollection<UserResults> userResults = UserResultsModel.GetResultadosPorUsuario(usuario.Idusuario);

                    if (userResults.Count() > 0)
                    {
                        Paragraph para = new Paragraph("Quiniela Brasil 2014", Fuentes.Encabezados);
                        para.Alignment = 1;
                        myDocument.Add(para);

                        para = new Paragraph(usuario.NombreCompleto, Fuentes.OtrosDatos);
                        para.Alignment = 0;
                        myDocument.Add(para);

                        para = new Paragraph("Fecha de registro: " + usuario.FechaRegistro, Fuentes.OtrosDatos);
                        para.Alignment = 0;
                        myDocument.Add(para);

                        myDocument.Add(this.GetUserResultTable(userResults));

                        myDocument.Add(this.GetResultadosMexico(userResults));

                        myDocument.NewPage();
                    }
                }

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

        private Font GetWinnerFont(UserResults user)
        {
            if (user.Ganador.Equals(user.Local))
                return Fuentes.GanadorLocal;
            else if (user.Ganador.Equals(user.Visitante))
                return Fuentes.GanadorVisita;
            else
                return Fuentes.Empate;

        }


        private PdfPTable GetResultadosMexico(ObservableCollection<UserResults> resultados)
        {
            PdfPTable table = new PdfPTable(3);
            table.HorizontalAlignment = 1;
            //table.TotalWidth = 400;
            table.WidthPercentage = 75;
            table.DefaultCell.Border = 0;

            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            var mexico = (from n in resultados
                          where n.Local == "México" || n.Visitante == "México"
                          select n).ToList();

            foreach (UserResults user in mexico)
            {


                PdfPCell cell = new PdfPCell(new Phrase(user.Local, Fuentes.OtrosDatos));
                cell.Colspan = 0;
                cell.Border = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("vs.", Fuentes.OtrosDatos));
                cell.Colspan = 0;
                cell.Border = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(user.Visitante, Fuentes.OtrosDatos));
                cell.Colspan = 0;
                cell.Border = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(user.GolesLocal.ToString(), Fuentes.OtrosDatos));
                //cell = new PdfPCell(new Phrase(" ", Fuentes.OtrosDatos));
                cell.Colspan = 0;
                cell.Border = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(" ", Fuentes.OtrosDatos));
                cell.Colspan = 0;
                cell.Border = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(user.GolesVisita.ToString(), Fuentes.OtrosDatos));
                //cell = new PdfPCell(new Phrase(" ", Fuentes.OtrosDatos));
                cell.Colspan = 0;
                cell.Border = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

            }

            return table;
        }

    }
}
