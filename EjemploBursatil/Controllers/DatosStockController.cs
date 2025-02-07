using EjemploBursatil.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EjemploBursatil.Controllers
{
    public class DatosStockController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Array> Get()
        {
            int acum = 0;
            List<Array> datos = new List<Array>();
            DataIndices data = new DataIndices();
            List<Indices> indices = data.TraerTodos();

            double[] series1 = new double[indices.Count()];
            double[] series2 = new double[indices.Count()];

            foreach (var item in indices)
            {
                series1[acum] = item.Stock3;
                series2[acum] = item.Stock6;
                acum++;
            }

            datos.Add(series1);
            datos.Add(series2);

            return datos;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}