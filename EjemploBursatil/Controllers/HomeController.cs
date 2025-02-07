using EjemploBursatil.DAL;
using EjemploBursatil.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EjemploBursatil.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Example()
        {
            DataIndices dat = new DataIndices();

            List<Indices> listaIndices= dat.TraerTodos();

            //UP EQ DOWN

            List<Recomendacion> recomendaciones = dat.ObtenerRecomendaciones(listaIndices);
            ViewBag.Message = recomendaciones;
           

            ViewBag.CandleStick = JsonConvert.SerializeObject(ObtenerCandleStrickBag(listaIndices));
            //ViewBag.CandleStick = ObtenerCandleStrickBagCSV(listaIndices);

            ViewBag.Tendencia = JsonConvert.SerializeObject(ObtenerTendeciaBag(listaIndices));
            
            ViewBag.Stock = JsonConvert.SerializeObject(ObtenerStockBag(listaIndices));
            ViewBag.Stock3 = JsonConvert.SerializeObject(ObtenerStock3Bag(listaIndices));
            ViewBag.Stock6 = JsonConvert.SerializeObject(ObtenerStock6Bag(listaIndices));

            ViewBag.RSI = JsonConvert.SerializeObject(ObtenerRSIBag(listaIndices));


            ViewBag.Volumen = JsonConvert.SerializeObject(ObtenerVolumenBag(listaIndices));

            //JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            //return Content(JsonConvert.SerializeObject(dataPoints, _jsonSetting), "application/json");


            return View();
        }

        private List<DataPoint> ObtenerVolumenBag(List<Indices> listaIndices)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            double max = listaIndices.Max(m => m.Volumen);

            foreach (Indices item in listaIndices)
            {
                dataPoints.Add(new DataPoint(convertToMilisecondsJS(item.Fecha), item.Volumen / max));
            }

            return dataPoints;
        }



        private List<DataPoint>   ObtenerRSIBag(List<Indices> listaIndices)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            foreach (Indices item in listaIndices)
            {
                dataPoints.Add(new DataPoint(convertToMilisecondsJS(item.Fecha), item.RSI));
            }

            return dataPoints;
        }

        private List<DataPoint> ObtenerStock6Bag(List<Indices> listaIndices)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            foreach (Indices item in listaIndices)
            {
                dataPoints.Add(new DataPoint(convertToMilisecondsJS(item.Fecha), item.Stock6));
            }
            return dataPoints;
        }

        private List<DataPoint> ObtenerStock3Bag(List<Indices> listaIndices)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            foreach (Indices item in listaIndices)
            {
                dataPoints.Add(new DataPoint(convertToMilisecondsJS(item.Fecha), item.Stock3));
            }
            return dataPoints;
        }

        private List<DataPoint> ObtenerStockBag(List<Indices> listaIndices)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            foreach (Indices item in listaIndices)
            {
                dataPoints.Add(new DataPoint(convertToMilisecondsJS(item.Fecha), item.Stock));
            }
            return dataPoints;
        }


        private List<DataPoint> ObtenerTendeciaBag(List<Indices> listaIndices)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            foreach (Indices item in listaIndices)
            {
                dataPoints.Add(new DataPoint(convertToMilisecondsJS( item.Fecha), item.Tendencia));
            }
            return dataPoints;
        }

        private List<DataPointOHLC> ObtenerCandleStrickBag(List<Indices> listaIndices)
        {
            List<DataPointOHLC> dataPoints = new List<DataPointOHLC>();
            int pos = 0;
            foreach (Indices item in listaIndices)
            {
                //Double dFecha = Convert.ToDouble(item.Fecha);
                long ms1 = convertToMilisecondsJS(item.Fecha);
                
                
                //ms1 = ms1 * 1000;
                dataPoints.Add(new DataPointOHLC(ms1, new double[] { item.Apertura, item.Máximo, item.Mínimo, item.Último }));
                //string[] mat = item.Fecha.ToShortDateString().Split('/');
                //string fechaing = mat[0] + "-" + mat[1] + "-" + mat[2];
                //dataPoints.Add(new DataPointOHLC(fechaing, new double[] { item.Apertura, item.Máximo, item.Mínimo, item.Último }));
                //pos++;
            }

            //List<DataPointOHLC> dataPoints = new List<DataPointOHLC>();

            //dataPoints.Add(new DataPointOHLC(DateTime.Today.AddDays(1), new double[] { 141.38, 141.7, 115.02, 120.13 }));
            //dataPoints.Add(new DataPointOHLC(DateTime.Today.AddDays(2), new double[] { 119.64, 124.91, 102.1, 118.18 }));
            //dataPoints.Add(new DataPointOHLC(DateTime.Today.AddDays(3), new double[] { 119.01, 136.78, 118.25, 126.94 }));
            //dataPoints.Add(new DataPointOHLC(DateTime.Today.AddDays(4), new double[] { 126.23, 137.89, 125.11, 134.8 }));
            //dataPoints.Add(new DataPointOHLC(DateTime.Today.AddDays(5), new double[] { 134.38, 135.24, 125.88, 126.15 }));
            //dataPoints.Add(new DataPointOHLC(DateTime.Today.AddDays(6), new double[] { 126, 134.55, 122.35, 129.87 }));
            //dataPoints.Add(new DataPointOHLC(DateTime.Today.AddDays(7), new double[] { 129.54, 139.45, 123.96, 133.66 }));
            //dataPoints.Add(new DataPointOHLC(DateTime.Today.AddDays(8), new double[] { 133.21, 136.37, 129.14, 129.45 }));
            //dataPoints.Add(new DataPointOHLC(DateTime.Today.AddDays(9), new double[] { 130.03, 133.08, 126.31, 131.74 }));
            //dataPoints.Add(new DataPointOHLC(DateTime.Today.AddDays(10), new double[] { 131.28, 146.23, 130.74, 142 }));
            //dataPoints.Add(new DataPointOHLC(DateTime.Today.AddDays(11), new double[] { 142.95, 153.08, 138.8, 150.56 }));
            //dataPoints.Add(new DataPointOHLC(DateTime.Today.AddDays(12), new double[] { 150.74, 160.07, 150.02, 155.68 }));

            //ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            //return View();

            //string csv = "";

            //foreach (DataPointOHLC DataPoint in dataPoints)
            //{
            //    csv += DataPoint.X.ToString("yyyy-MM-dd");
            //    foreach (double Y in DataPoint.Y)
            //        csv += "," + Y.ToString();
            //    csv += "\n";
            //}

            //return csv;
            //return Content(csv);


            return dataPoints;

        }

        private long convertToMilisecondsJS(DateTime item)
        {
            DateTime dateini = new DateTime(1970, 1, 1);// January 1, 1970
            long ms1 = 0;
            ms1 = item.Ticks - dateini.Ticks;

            ms1 = ms1 / TimeSpan.TicksPerMillisecond;

            return ms1;
        }

        public ContentResult CSV()
        {
            //List<DataPointOHLC> dataPoints = new List<DataPointOHLC>();

            ////dataPoints.Add(new DataPointOHLC(new DateTime(2016, 01, 01), new double[] { 656.289978, 657.719971, 547.179993, 587.000000 }));
            ////dataPoints.Add(new DataPointOHLC(new DateTime(2016, 02, 01), new double[] { 578.150024, 581.799988, 474.000000, 552.520020 }));
            ////dataPoints.Add(new DataPointOHLC(new DateTime(2016, 03, 01), new double[] { 556.289978, 603.239990, 538.580017, 593.640015 }));
            ////dataPoints.Add(new DataPointOHLC(new DateTime(2016, 04, 01), new double[] { 590.489990, 669.979980, 585.250000, 659.590027 }));
            ////dataPoints.Add(new DataPointOHLC(new DateTime(2016, 05, 01), new double[] { 663.919983, 724.229980, 656.000000, 722.789978 }));
            ////dataPoints.Add(new DataPointOHLC(new DateTime(2016, 06, 01), new double[] { 720.900024, 731.500000, 682.119995, 715.619995 }));
            ////dataPoints.Add(new DataPointOHLC(new DateTime(2016, 07, 01), new double[] { 717.320007, 766.000000, 716.539978, 758.809998 }));
            ////dataPoints.Add(new DataPointOHLC(new DateTime(2016, 08, 01), new double[] { 759.869995, 774.979980, 750.349976, 769.159973 }));
            ////dataPoints.Add(new DataPointOHLC(new DateTime(2016, 09, 01), new double[] { 770.900024, 839.950012, 756.000000, 837.309998 }));
            ////dataPoints.Add(new DataPointOHLC(new DateTime(2016, 10, 01), new double[] { 836.000000, 847.210022, 774.609985, 789.820007 }));
            ////dataPoints.Add(new DataPointOHLC(new DateTime(2016, 11, 01), new double[] { 799.000000, 800.840027, 710.099976, 750.570007 }));
            ////dataPoints.Add(new DataPointOHLC(new DateTime(2016, 12, 01), new double[] { 752.409973, 782.460022, 736.700012, 768.659973 }));
            //foreach (Indices item in listaIndices)
            //{
            //    dataPoints.Add(new DataPointOHLC(item.Fecha, new double[] { item.Apertura, item.Máximo, item.Mínimo, item.Último }));
            //}



            string csv = "";

            //foreach (DataPointOHLC DataPoint in dataPoints)
            //{
            //    csv += DataPoint.X.ToString("yyyy-MM-dd");
            //    foreach (double Y in DataPoint.Y)
            //        csv += "," + Y.ToString();
            //    csv += "\n";
            //}

            return Content(csv);
        }

        public ContentResult JSON()
        {
            List<DataPointOHLC> dataPoints = new List<DataPointOHLC>();


            //dataPoints.Add(new DataPointOHLC(1506882600000, new double[] { 85.990997, 86.374001, 85.945999, 86.374001 }));
            //dataPoints.Add(new DataPointOHLC(1506969000000, new double[] { 86.374001, 86.374001, 86.374001, 86.374001 }));
            //dataPoints.Add(new DataPointOHLC(1507055400000, new double[] { 86.5, 88.679001, 86.199997, 87.764999 }));
            //dataPoints.Add(new DataPointOHLC(1507141800000, new double[] { 88.098999, 89.172997, 88.098999, 88.800003 }));
            //dataPoints.Add(new DataPointOHLC(1507228200000, new double[] { 88.532997, 89.334999, 88.532997, 89.271004 }));
            //dataPoints.Add(new DataPointOHLC(1507487400000, new double[] { 88.801003, 89.126999, 88.622002, 88.978996 }));
            //dataPoints.Add(new DataPointOHLC(1507573800000, new double[] { 88.693001, 88.900002, 88.199997, 88.639999 }));
            //dataPoints.Add(new DataPointOHLC(1507660200000, new double[] { 88.419998, 88.605003, 87.958, 88.605003 }));
            //dataPoints.Add(new DataPointOHLC(1507746600000, new double[] { 88.213997, 88.566002, 87.484001, 87.938004 }));
            //dataPoints.Add(new DataPointOHLC(1507833000000, new double[] { 87.800003, 87.800003, 86.848, 87.487 }));
            //dataPoints.Add(new DataPointOHLC(1508092200000, new double[] { 87.100998, 87.649002, 86.975998, 87.295998 }));
            //dataPoints.Add(new DataPointOHLC(1508178600000, new double[] { 86.906998, 87.656998, 86.370003, 87.656998 }));
            //dataPoints.Add(new DataPointOHLC(1508265000000, new double[] { 88.214996, 88.545998, 87.992996, 88.418999 }));
            //dataPoints.Add(new DataPointOHLC(1508351400000, new double[] { 87.699997, 87.699997, 86.099998, 87.093002 }));
            //dataPoints.Add(new DataPointOHLC(1508437800000, new double[] { 86.800003, 87.533997, 86.385002, 86.385002 }));
            //dataPoints.Add(new DataPointOHLC(1508697000000, new double[] { 86.221001, 86.613998, 85.751999, 85.999001 }));
            //dataPoints.Add(new DataPointOHLC(1508783400000, new double[] { 85.801003, 86.605003, 85.242996, 86.549004 }));
            //dataPoints.Add(new DataPointOHLC(1508869800000, new double[] { 86.248001, 86.248001, 85, 85.248001 }));
            //dataPoints.Add(new DataPointOHLC(1508956200000, new double[] { 85.401001, 86.554001, 85.188004, 86.066002 }));
            //dataPoints.Add(new DataPointOHLC(1509042600000, new double[] { 86.500999, 87.747002, 86.500999, 87.002998 }));
            //dataPoints.Add(new DataPointOHLC(1509301800000, new double[] { 87.225998, 87.776001, 87.225998, 87.282997 }));
            //dataPoints.Add(new DataPointOHLC(1509388200000, new double[] { 87.282997, 87.282997, 87.282997, 87.282997 }));


            JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            return Content(JsonConvert.SerializeObject(dataPoints, _jsonSetting), "application/json");
        }
    }
}
