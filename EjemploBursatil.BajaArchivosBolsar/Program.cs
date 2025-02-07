using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploBursatil.BajaArchivosBolsar
{
    class Program
    {
        static void Main(string[] args)
        {
            //try
            //{
            Console.WriteLine("--------------------------------");
            Console.WriteLine("-    PROCESADOR CONSOLA v1     -");
            Console.WriteLine("--------------------------------");


            Console.WriteLine("Descargando de Archivos BOLSAR");
            ManagerBolsar mngr = new ManagerBolsar();

            mngr.DescargarXls();
            mngr.ProcesarXls();

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("--ERROR INESPERADO" + ex.Message.ToString());
            //    //TODO: Agregar un loging de errores inesperados
            //}
            Console.WriteLine("La ejecucion del proceso ha Finalizado");
#if DEBUG
            Console.ReadLine();
#endif
        }


    }
}
