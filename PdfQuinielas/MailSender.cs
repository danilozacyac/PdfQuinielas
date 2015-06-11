using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using PdfQuinielas.Dao;
using PdfQuinielas.Models;
using PdfQuinielas.Singleton;
using Quiniela.Dao;

namespace PdfQuinielas
{
   public class MailSender
    {
       public static void MailNoAttachment()
       {

           foreach (Usuarios usuario in UsuariosSingleton.UsuariosSin)
           {


               MailMessage mailMessage = new MailMessage();
               mailMessage.From = new MailAddress("luisalbertove80@hotmail.com");
               //mailMessage.From = new MailAddress("donotreplyquiniela@outlook.com");

               //receiver email adress
               mailMessage.To.Add(usuario.Mail);

               //subject of the email
               mailMessage.Subject = "Invitación a la quiniela de la Copa América 2015";

               //attach the file
               //mailMessage.Attachments.Add(new Attachment(@"C:\\attachedfile.jpg"));
               mailMessage.Body = "Este mensaje fue enviado por el administrador de La Web de Danilo \n\n\n *** POR FAVOR NO RESPONDA A ESTE MENSAJE *** \n\n\n " +
                                  " Te invitamos a participar en la quiniela de la Copa América 2015 a realizarse en Chile, esta invitación se te extiende debido a tu participación en la quiniela del Mundial Brasil 2014. " +
                                  " En esta ocasión la puntuación será de acuerdo a lo siguiente: \n\n\n " +
                                  " 1. 1 punto por acertar al ganador del partido \n\n\n" + 
                                  " 2. 1 punto + 2 puntos extra por acertar al marcador del partido \n\n\n " +
                                  " La cuota de participación será de $200 pesos y el ganador obtendrá el total del monto recaudado" + 
                                  "Te recordamos que tu usuario de participación es: " + usuario.Usuario + "\n\n\n" +
                                  "Si no recuerdas tu contraseña te sugerimos ingresar a la sección olvidaste tu contraseña donde se te guiará en el proceso de recuperación. " +
                                  " \n\n\n Gracias por ser parte de La Web de Danilo. \n\n\n Saludos";
               mailMessage.IsBodyHtml = false;

               //SMTP client
               SmtpClient smtpClient = new SmtpClient("smtp.live.com");
               //port number for Hot mail
               smtpClient.Port = 25;
               //credentials to login in to hotmail account
               smtpClient.Credentials = new NetworkCredential("luisalbertove80@hotmail.com", "m33tm3h41f");
               //smtpClient.Credentials = new NetworkCredential("donotreplyquiniela@outlook.com", "m33tm3h41f");
               //enabled SSL
               smtpClient.EnableSsl = true;
               //Send an email
               smtpClient.Send(mailMessage);
           }
       }



       public static void EnviarConcentradoQuiniela(string filePath,Torneos torneo)
       {
           ObservableCollection<Usuarios> listaUsuarios = new UsuariosModel().GetUsuarios(torneo.IdTorneo);

           string correos = String.Empty;

           foreach (Usuarios usuario in listaUsuarios)
           {
               correos += usuario.Mail + " ";
           }


           foreach (Usuarios usuario in listaUsuarios)
           {
               try
               {

                   MailMessage mailMessage = new MailMessage();
                   mailMessage.From = new MailAddress("luisalbertove80@hotmail.com");
                   //mailMessage.From = new MailAddress("donotreplyquiniela@outlook.com");

                   //receiver email adress
                   mailMessage.To.Add(usuario.Mail);

                   //subject of the email
                   mailMessage.Subject = "Pronósticos Concentrados de la Copa América 2015";

                   //attach the file
                   //mailMessage.Attachments.Add(new Attachment(@"C:\\attachedfile.jpg"));
                   mailMessage.Body = "Este mensaje fue enviado por el administrador de La Web de Danilo \n\n\n *** POR FAVOR NO RESPONDA A ESTE MENSAJE *** \n\n\n " +
                                      " Agradecemos tu participaciónen la quiniela de la Copa América 2015 a realizarse en Chile, adjunto a este mensaje encontraras un PDF con los resultados de cada uno de los " + listaUsuarios.Count + " participantes en esta quiniela \n\n\n" +
                                      " En esta ocasión la puntuación será de acuerdo a lo siguiente: \n\n\n " +
                                      " 1. 1 punto por acertar al ganador del partido \n\n\n" +
                                      " 2. 1 punto + 2 puntos extra por acertar al marcador del partido, es decir el MÁXIMO de puntos por partido será 3 \n\n\n " +
                                      " ACLARACIÓN: Si tu pronostico del ganador o el empate es incorrecto, aunque el marcador sea el correcto no recibirás ningún punto" +
                                      "La bolsa acumulada en esta ocasión es de $" + (listaUsuarios.Count * 200) + " y como se mencionó en las bases de participación será entregada al participante con mayor número de puntos al finalizar la competencia.\n\n\n\n" +
                                      "Agradecemos nos haga llegar las sugerencias que tenga o cualquier asunto concerniente a esta quiniela al correo danilo@lawebdedanilo.net " +
                                      " \n\n\n Gracias por ser parte de La Web de Danilo. \n\n\n Saludos";

                   mailMessage.IsBodyHtml = false;

                   // Create  the file attachment for this e-mail message.
                   Attachment data = new Attachment(filePath, MediaTypeNames.Application.Octet);
                   // Add time stamp information for the file.
                   ContentDisposition disposition = data.ContentDisposition;
                   disposition.CreationDate = System.IO.File.GetCreationTime(filePath);
                   disposition.ModificationDate = System.IO.File.GetLastWriteTime(filePath);
                   disposition.ReadDate = System.IO.File.GetLastAccessTime(filePath);
                   // Add the file attachment to this e-mail message.
                   mailMessage.Attachments.Add(data);

                   //SMTP client
                   SmtpClient smtpClient = new SmtpClient("smtp.live.com");
                   //port number for Hot mail
                   smtpClient.Port = 25;
                   //credentials to login in to hotmail account
                   smtpClient.Credentials = new NetworkCredential("luisalbertove80@hotmail.com", "m33tm3h41f");
                   //smtpClient.Credentials = new NetworkCredential("donotreplyquiniela@outlook.com", "m33tm3h41f");
                   //enabled SSL
                   smtpClient.EnableSsl = true;
                   //Send an email
                   smtpClient.Send(mailMessage);
               }
               catch (Exception ex)
               {

               }

               break;
           }
       }
    }
}
