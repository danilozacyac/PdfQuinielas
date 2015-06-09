using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using PdfQuinielas.Dao;
using Quiniela.Dao;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfQuinielas.Singleton;
using Quiniela.Models;

namespace PdfQuinielas
{
    public class ResultadosConcentradoPdf
    {
        private iTextSharp.text.Document myDocument;

        public void ResultadosPorUsuario(Torneos torneo)
        {
            Document myDocument = new Document(new Rectangle(288f, 144f), 10, 10, 10, 10);
            myDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

            //myDocument = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
            string documento = Path.GetTempFileName() + ".pdf";

            //ObservableCollection<UserResults> userResults = UserResultsModel.GetResultadosPorUsuario(usuario.Idusuario);

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(documento, FileMode.Create));

                myDocument.Open();


                Paragraph para = new Paragraph("Quiniela " + torneo.Torneo, Fuentes.Encabezados);
                para.Alignment = 1;
                myDocument.Add(para);


                myDocument.Add(this.GetUserResultTable());

                myDocument.NewPage();

                para = new Paragraph("Participantes", Fuentes.Encabezados);
                para.Alignment = 1;
                myDocument.Add(para);

                foreach (Usuarios user in UsuariosSingleton.UsuariosSin)
                {
                    para = new Paragraph(user.Idusuario + "   " + user.NombreCompleto, Fuentes.OtrosDatosConcen);
                    para.Alignment = 0;
                    myDocument.Add(para);
                }

                myDocument.NewPage();

                para = new Paragraph("Partidos", Fuentes.Encabezados);
                para.Alignment = 1;
                myDocument.Add(para);

                foreach (Partidos partido in PartidosModel.GetPartidos())
                {
                    para = new Paragraph(partido.IdPartido + "    " + partido.PaisLocal + "  vs  " + partido.PaisVisita, Fuentes.OtrosDatosConcen);
                    para.Alignment = 0;
                    myDocument.Add(para);
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

       private PdfPTable GetUserResultTable()
        {
            PdfPTable table = new PdfPTable(49);
            //table.TotalWidth = 400;
            table.WidthPercentage = 100;

            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            PdfPCell cell;

            cell = new PdfPCell(new Phrase("  ", Fuentes.EncabezadoColumnaConcentrado));
            cell.Colspan = 0;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);

            for(int x = 1; x < 49; x++)
            {
                cell = new PdfPCell(new Phrase(x.ToString(), Fuentes.EncabezadoColumna));
                cell.Colspan = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);
            }

            ObservableCollection<Usuarios> listaUsuarios = UsuariosSingleton.UsuariosSin;
            ObservableCollection<Partidos> listaPartidos = PartidosModel.GetPartidos();

            foreach (Usuarios user in listaUsuarios)
            {
                ObservableCollection<Partidos> resultadosusuarios = PartidosModel.GetPartidosByUser(user.Idusuario);

                cell = new PdfPCell(new Phrase(user.Idusuario.ToString(), Fuentes.OtrosDatosConcen));
                cell.Colspan = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                for (int indexPartido = 0; indexPartido < 48; indexPartido++)
                {
                    if (listaPartidos[indexPartido].IdPartido == resultadosusuarios[indexPartido].IdPartido &&
                        listaPartidos[indexPartido].IdPaisLocal == resultadosusuarios[indexPartido].IdPaisGanador)
                    {
                        cell = new PdfPCell(new Phrase(" L ", Fuentes.OtrosDatosConcen));
                        cell.Colspan = 0;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                    }
                    else if (listaPartidos[indexPartido].IdPartido == resultadosusuarios[indexPartido].IdPartido &&
                        listaPartidos[indexPartido].IdPaisVisita == resultadosusuarios[indexPartido].IdPaisGanador)
                    {
                        cell = new PdfPCell(new Phrase(" V ", Fuentes.OtrosDatosConcen));
                        cell.Colspan = 0;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                    }
                    else if (listaPartidos[indexPartido].IdPartido == resultadosusuarios[indexPartido].IdPartido &&
                        resultadosusuarios[indexPartido].IdPaisGanador == 999)
                    {
                        cell = new PdfPCell(new Phrase(" E ", Fuentes.OtrosDatosConcen));
                        cell.Colspan = 0;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                    }
                    else
                    {
                        MessageBox.Show("Ooops! algo anda mal");
                    }
                }


            }

            return table;
        }
    }
}
