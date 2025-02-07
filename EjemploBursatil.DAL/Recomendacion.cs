using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EjemploBursatil.DAL
{
    public class Recomendacion
    {
        private string texto;

        public string Texto
        {
            get { return texto; }
            set { texto = value; }
        }
        private int tendencia;

        public int Tendencia
        {
            get { return tendencia; }
            set {   
                tendencia = value;
                this.Icon = ObtenerIcono(value);
            }
        }

        private string ObtenerIcono(int resultado)
        {
            string str_ico = "";
            if (resultado < 0)
            {
                str_ico = "DOWN";
            }
            else
            {
                if (resultado >= 0 && resultado < 1)
                {
                    str_ico = "EQ";
                }
                else
                {
                    str_ico = "UP";
                }
            }

            return str_ico;
        }

        private string icon;

        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }
    }
}