using EjemploBursatil.DAL;
using EjemploBursatil.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace EjemploBursatil.Controllers
{
    public class DatosPromedioController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        public IEnumerable<Array> Get()
        {
            int acum = 0; 

            List<Array> datos = new List<Array>();
            DataIndices data = new DataIndices();
            List<Indices> indices=  data.TraerTodos();

            double[] series1 = new double[indices.Count()];
            double[] series2 = new double[indices.Count()];

            foreach (var item in indices)
            {
                series1[acum] = 1; //item.SerieProm1.Value;
                series2[acum] = 2;// item.SerieProm2.Value;
                acum++;
            }

            datos.Add(series1);
            datos.Add(series2);
           
            return datos;
        }

        // GET api/<controller>/5
        public IEnumerable<Array> Get(int id)
        {
            return null;
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