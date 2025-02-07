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
    public class DatosCrudosController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        public IEnumerable<Array> Get()
        {
            List<Array> datos = new List<Array>();

            int[] matriz = new int[13];
            int[] matriz2 = new int[13];
            for (var i = 0; i < 13; i++)
            {
                matriz[i] = i;
                matriz2[i] = 13 - i;
            }
            datos.Add(matriz);
            datos.Add(matriz2);
            return datos;
        }

        // GET api/<controller>/5
        public IEnumerable<Array> Get(int id)
        {
             List<Array> datos = new List<Array>();
             
            switch (id)
            {
                case  1:
                    int[] matriz = new int[13];
                    int[] matriz2 = new int[13];
                    for (var i=0; i<13; i++) {
                        matriz[i] = i;
                        matriz2[i] = 13 - i;
                    }
                    datos.Add(matriz);
                    datos.Add(matriz2);
                    break;
                case  2:
                    
                    break;
                default:
                    break;
            }
            return datos;

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