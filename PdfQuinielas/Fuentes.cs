using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;

namespace PdfQuinielas
{
    public class Fuentes
    {
        private static readonly BaseColor black = new BaseColor(0, 0, 0);
        private static readonly BaseColor red = new BaseColor(200, 0, 0);
        private static readonly BaseColor green = new BaseColor(0, 200, 0);
        private static readonly BaseColor blue = new BaseColor(0, 0, 200);

        public static Font Encabezados
        {
            get
            {
                Font font = FontFactory.GetFont("Arial", 18, Font.BOLD, black);
                return font;
            }
        }

        
        public static Font EncabezadoColumna
        {
            get
            {
                Font font = FontFactory.GetFont("Arial", 12, Font.BOLD, black);
                return font;
            }
        }

        public static Font EncabezadoColumnaConcentrado
        {
            get
            {
                Font font = FontFactory.GetFont("Arial", 6, Font.BOLD, black);
                return font;
            }
        }



        /// <summary>
        /// Fuentes IUS
        /// </summary>


        public static Font OtrosDatos
        {
            get
            {
                Font font = FontFactory.GetFont("Arial", 10, Font.NORMAL, black);
                return font;
            }
        }

        public static Font OtrosDatosConcen
        {
            get
            {
                Font font = FontFactory.GetFont("Arial", 6, Font.NORMAL, black);
                return font;
            }
        }

        public static Font GanadorLocal
        {
            get
            {
                Font font = FontFactory.GetFont("Arial", 10, Font.NORMAL, green);
                return font;
            }
        }

        public static Font GanadorVisita
        {
            get
            {
                Font font = FontFactory.GetFont("Arial", 10, Font.NORMAL, red);
                return font;
            }
        }

        public static Font Empate
        {
            get
            {
                Font font = FontFactory.GetFont("Arial", 10, Font.NORMAL, blue);
                return font;
            }
        }

        



    }
}