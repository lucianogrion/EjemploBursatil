using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Configuration;
using System.Xml;
using EjemploBursatil.DAL;
using System.IO;

namespace EjemploBursatil.BajaArchivosBolsar
{
    sealed class ManagerBolsar
    {
        public void DescargarXls()
        {
            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
            service.FirefoxBinaryPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
            FirefoxOptions ffo = new FirefoxOptions();

            //Preferencias para que no abra y baje directo el archivo sin intervencion del usuario

            ffo.SetPreference("browser.download.dir", ConfigurationManager.AppSettings["PathDescarga"]);
            ffo.SetPreference("browser.download.folderList", 2);
            //Set Preference to not show file download confirmation dialogue using MIME types Of different file extension types.
            ffo.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/xls,text/csv,application/octet-stream,application/x-msexcel,application/excel,application/x-excel,application/vnd.ms-excel,image/png,image/jpeg,text/html,text/plain,application/msword,application/xml");

            ffo.SetPreference("browser.helperApps.alwaysAsk.force", false);
            ffo.SetPreference("browser.download.manager.useWindow", false);
            ffo.SetPreference("browser.download.manager.showAlertOnComplete", false);
            ffo.SetPreference("browser.download.manager.closeWhenDone", true);

            //Intento evitar popup
            ffo.SetPreference("browser.download.manager.showWhenStarting", false);
            ffo.SetPreference("dom.disable_open_during_load", true);
            ffo.SetPreference("pdfjs.disabled", true);


            FirefoxDriver
                driver = new FirefoxDriver(ffo);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));


            driver.Navigate().GoToUrl("https://www.bolsar.com/VistasDL/PaginaPrincipal.aspx");


            wait.Until<IWebElement>((d => d.FindElement(By.Id("txtUsuario")))); //in case you want to wait for a particular element to appear on the page.`

            IWebElement element = driver.FindElement(By.Id("txtUsuario"));
            element.SendKeys("tanos2210");

            element = driver.FindElement(By.Id("txtPassword"));
            element.SendKeys("vicent31");

            wait.Until<IWebElement>((d => d.FindElement(By.Id("btnOk"))));
            element = driver.FindElement(By.Id("btnOk"));
            element.Click();


            wait.Until<IWebElement>((d => d.FindElement(By.Id("ctl00_btnLogIO")))); //in case you want to wait for a particular element to appear on the page.`


            driver.Navigate().GoToUrl("https://www.bolsar.com/Vistas/Herramientas/PaginaDescargaSeriesHistoricas.aspx");

            //wait.Until<IWebElement>((d => d.FindElement(By.Id("ctl00_ContentPlaceHolder1_fechaDesde"))));

            element = driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_fechaDesde"));
            element.SendKeys(@"01/01/2018"); //TODO: Ver que esta fecha la genero yo

            element = driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_fechaHasta"));

            element.SendKeys(string.Format("{0}/{1}/{2}", DateTime.Today.Day.ToString(), DateTime.Today.Month.ToString(), DateTime.Today.Year.ToString())); //TODO: Ver que esta fecha la genero yo


            element = driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_chkAcciones"));
            element.Click();

            element = driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_btnAgregar"));
            element.Click();

            element = driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_btnDescargar"));
            element.Click();

            element = driver.FindElement(By.Id("ctl00_btnLogIO"));
            element.Click();

            //SendKeys.SendWait(@"{Enter});
            driver.Close();

            service.Dispose();
        }

        /// <summary>
        /// Busca el pathDescarga todos los xls y los procesa
        /// </summary>
        public void ProcesarXls()
        {
            Console.WriteLine(ConfigurationManager.AppSettings["PathDescarga"].ToString() + @"\*.xls");
            DirectoryInfo di = new DirectoryInfo(ConfigurationManager.AppSettings["PathDescarga"].ToString());

            foreach (var fi in di.GetFiles("*.xls"))
            {
                ProcesarArchivoXLS(fi.FullName);
            }


        }

        private void ProcesarArchivoXLS(string filename)
        {
            XmlDocument xDoc = new XmlDocument();

            Console.WriteLine("Procesando :" + filename);

            //Sacarle espacios inutiles y que despues no dejan parsear al parser
            if (File.Exists(filename))
            {
                // Create a file to write to.
                string createText = File.ReadAllText(filename).Trim();

                File.WriteAllText(filename, createText);
            }

            xDoc.Load(filename);

            XmlNodeList xWorksheet = xDoc.GetElementsByTagName("Worksheet");

            foreach (var item in xWorksheet)
            {
                //Buscar hasta procesar la hoja especies que es la accion
                XmlElement element = (XmlElement)item;
                if (element.GetAttribute("ss:Name") == "Especies") // "Indices"  "Especies"
                {
                    ProcesarWorkSheet(element);
                }
            }
            xDoc = null;
            FileInfo fi = new FileInfo(filename);
            fi.Delete();
            fi = new FileInfo(filename + ".tmp");
            fi.Delete();
        }

        private void ProcesarWorkSheet(XmlElement xWorksheet)
        {
            List<Indices> auxTipado = new List<Indices>();
            DataIndices data = new DataIndices();

            data.Borrar();

            XmlNodeList xRows = xWorksheet.GetElementsByTagName("Row");
            int i = 0;
            //List<RowCandleStick> rows = new List<RowCandleStick>();
            //ArrayList lista = new ArrayList();
            List<Indices> listaIndices = new List<Indices>();

            foreach (XmlElement nodo in xRows)
            {
                XmlNodeList xCells = ((XmlElement)nodo).GetElementsByTagName("Cell");
                //nodo.InnerText --> ['07/06/2009', 138.7, 139.68, 135.18, 135.4]


                if (xCells.Item(0).InnerText.ToUpper().Trim() != "ESPECIE")
                {
                    Indices row = new Indices();

                    row.Especie = xCells.Item(0).InnerText.ToString();

                    row.Fecha = DateTime.Today;
                    string strFecha = xCells.Item(1).InnerText;
                    string[] matFecha = strFecha.Split('/');
                    if (matFecha.Count() == 3)
                    {
                        row.Fecha = new DateTime(int.Parse(matFecha[2]), int.Parse(matFecha[1]), int.Parse(matFecha[0]));
                    }
                    //row.Fecha = matFecha[1] + "/" + matFecha[0] + "/" + matFecha[2]; // m-d-y
                    //row.Fecha = matFecha[1] + "/" + matFecha[0] + "/" + matFecha[2];// d-m-y

                    row.Apertura = double.Parse("0" +  xCells.Item(6).InnerText.Replace(".", ","));

                    row.Mínimo = double.Parse("0" + xCells.Item(8).InnerText.Replace(".", ","));
                    row.Máximo = double.Parse("0" + xCells.Item(7).InnerText.Replace(".", ","));

                    row.Último = double.Parse("0" + xCells.Item(4).InnerText.Replace(".", ","));

                    row.Volumen = double.Parse("0" + xCells.Item(9).InnerText.Replace(".", ","));

                    row.Stock3 = 0; //Lleno de 0 por si hay un NaN (que no deberia)
                    row.Stock6 = 0;
                    row.Tendencia = 0;
                    row.RSI = 0;

                    listaIndices.Add(row);
                }
                i++;
            }

            //Setear los campos calculados
            listaIndices = CargarStock(listaIndices);
            listaIndices = CargarStock3(listaIndices);
            listaIndices = CargarStock6(listaIndices);

            listaIndices = CargarTendencia(listaIndices);
            listaIndices = CargarSerieRSI(listaIndices);

            //auxTipado.Add(row);
            //Promedio movil3 agregar que si es 0 no lo agregue
            //if (row.Stock3.Value != 0)
            //{
            //    listaAnteriores.Add(row.Stock3.Value);
            //}
            //row.SerieProm1 = CalcularRSI(VC, row.Último, ref SAnteriores, ref BAnteriores);
            //row.SerieProm2 = CalcularSerieProm2();

            //row.Stock3 = CalcularStock3(VC, row.Mínimo, row.Máximo);
            //row.Stock6 = CalcularStock6(listaAnteriores);


            ////Tengo que dejar dis
            ////VC = double.Parse(xCells.Item(5).InnerText);

            int pos = 0;
            foreach (Indices item in listaIndices)
            {
                Console.WriteLine(string.Format("AGREGANDO {0} INDICE Stock3 {1} Máximo {2}",pos, item.Stock3, item.Máximo));
                pos++;
                data.AgregarIndice(item);
            }




            //return auxTipado;
        }

        

        private List<Indices> CargarTendencia(List<Indices> listaIndices)
        {
            int n = listaIndices.Count;
            int pos = 1;

            //todos los totales
            double xy = 0;
            double xx = 0;
            double x = 0;
            double y = 0;


            foreach (Indices item in listaIndices)
            {
                xy = xy + (pos * item.Último);
                xx = xx + (pos * pos);
                x = x + pos;
                y = y + item.Último;
                pos++;
            }

            double b= ( (n * xy)  - (x * y) ) / ( n* xx - (x*x) );

            double a = (y - b * x) /n;
            pos = 0;
            foreach (Indices item in listaIndices)
            {
                item.Tendencia = (pos) * b + a;
                pos++;
            }

            return listaIndices;

        }

        private List<Indices> CargarSerieRSI(List<Indices> listaIndices)
        {
            double VC = 0;
            List<double> listas= new List<double>();
            List<double> listab = new List<double>();
            int pos = 0;

            foreach (Indices item in listaIndices)
            {
                if (VC == 0)
                {
                    item.RSI = 1;   //IdeA: para que quede bien la escala tengo que poner el primer item al 100
                }
                else
                {
                    item.RSI = CalcularRSI(listaIndices, pos);
                }
                   
                VC = item.Último;
                pos++;
            }
            return listaIndices;

        }

        private List<Indices> CargarStock3(List<Indices> listaIndices)
        {
            int posActual = 0;
            foreach (Indices item in listaIndices)
            {
                //Obtener los valores en un array
                double[] arrayValues = listaIndices.Select(d => d.Stock).ToArray<double>();

                item.Stock3 = CalcularPromedioMovil(arrayValues, posActual, 3);
                posActual++;

            }
            return listaIndices;

        }
        private List<Indices> CargarStock6(List<Indices> listaIndices)
        {
            int pos = 0;

            foreach (Indices item in listaIndices)
            {
                double[] arrayValues = listaIndices.Select(d => d.Stock).ToArray<double>();
                item.Stock6 = CalcularPromedioMovil(arrayValues, pos, 6);
                pos++;
            }

            return listaIndices;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matriz">Es donde vienen los valores</param>
        /// <param name="pos">la posicion en la que estoy</param>
        /// <param name="cantidad">la cantidad de pasos del promedio movil</param>
        /// <returns></returns>
        private double CalcularPromedioMovil( double[] matriz, int pos, int cantidad)
        {
            double total = 0;

            if ((pos + 1) >= cantidad)
            {
                //Acumular los n anteriores
                for (int i = 0; i < cantidad; i++)
                {
                    total = total + matriz[pos - i];
                }
                total = total / cantidad;
            }

            if (total == double.NaN)
            {
                return 0;
            }

            return total;
        }

        private List<Indices> CargarStock(List<Indices> listaIndices)
        {
            //double VC = 0;
            int pos = 0;
            foreach (Indices item in listaIndices)
            {
                item.Stock = CalcularValorStock( listaIndices, pos);
                //VC = item.Último;
                pos++;
            }

            
            
            return listaIndices;
        }

        private double CalcularValorStock( List<Indices> listaValores, int pos)
        {
            if (pos<4)
            {
                return 0;
            }

            double min = double.MaxValue;
            double max = 0;
            //Obtener el minimo de los 5ultMinimos
            for (int i = 0; i < 5; i++)
            {
                if (listaValores[pos-i].Máximo>max)
                {
                    max = listaValores[pos - i].Máximo;
                }
                if (listaValores[pos-i].Mínimo<min)
                {
                    min = listaValores[pos-i].Mínimo;
                }
            }
            //Obtener el maximo de los ultimos 5 maximos
            //Calcular el Stockastico

            double stock = (double)100 * ((listaValores[pos].Último - min) / (max - min));

            stock = Math.Round(stock, 0);

            if (stock > 100)
            {
                return 100;
            }

            if (stock < 0)
            {
                return 0;
            }

            if (stock == double.NaN)
            {
                return 0;
            }
            return stock;
        }



        private double CalcularRSI(List<Indices> listavalores, int pos)
        {

            if (pos<13)
            {
                return 0;
            }

             double s = 0;
            double b = 0;
            
            for (int i = 0; i < 13; i++)
			{
                double CierreHoy = listavalores[pos - i].Último;
                double CierreAyer = listavalores[pos- 1 - i].Último;
                
                if (CierreAyer != CierreHoy)
                {
                    if (CierreHoy > CierreAyer)
                    {
                        s = s + ( CierreHoy - CierreAyer);
                        
                    }
                    else
                    {
                        b = b + ( CierreAyer - CierreHoy);
                        
                    }
                }
			}

            //Calcular S Y B
            //double RS = PromMovil14(SAnteriores) / PromMovil14(BAnteriores);
            s = s / 14;
            b = b / 14;


            //100-(100/(1+(E21/F21)))
            double total = (double)(100- (100 / (1 + (s/b) )));
            if (total == double.NaN)
            {
                return 0;
            }
            total = Math.Round(total/100, 2);

            return total;

            // 100 – 100 x ( 1 / (1 + RS))

        }

        private double PromMovil14(List<double> ListaValores)
        {
            if (ListaValores.Count > 14)
            {
                ListaValores = ListaValores.Take(14).Reverse().ToList();
            } 

            return CalcularPromedioTotal(ListaValores);

        }



        private double? CalcularStock6(List<double> listaAnteriores)
        {

            if (listaAnteriores.Count() > 3)
            {

                listaAnteriores = listaAnteriores.Take(3).Reverse().ToList();
            }


            double promedio = CalcularPromedioTotal(listaAnteriores);
            return promedio;

        }

        private double CalcularPromedioTotal(List<double> listaValores)
        {
            int count = 0;
            double total = 0;

            foreach (var item in listaValores)
            {
                count++;
                total = total + item;
            }

            if (count == 0)
            {
                return 0;
            }
            return total / count;
        }












        private double? CalcularSerieProm2()
        {
            return 0.3;
        }

        private double? CalcularSerieProm1()
        {
            return 4;
        }
    }
}
