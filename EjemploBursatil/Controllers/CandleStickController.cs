using EjemploBursatil.DAL;
using EjemploBursatil.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;

namespace EjemploBursatil.Controllers
{
    public class CandleStickController : ApiController
    {
        // GET api/<controller>
        public List<RowCandleStick> Get()
        {
            List<RowCandleStick> auxTipado = new List<RowCandleStick>();
            DataIndices mngr = new DataIndices();
            foreach (var item in mngr.TraerTodos())
	        {
                auxTipado.Add(ConvertToCandleStick(item));
	        }
           
            
            return auxTipado;

        }

        private RowCandleStick ConvertToCandleStick(Indices item)
        {
           
            RowCandleStick row = new RowCandleStick();
            row.Open =(float) item.Apertura;
            row.Close = (float)item.Último;

            row.Fecha = item.Fecha.ToString();
            row.Min = (float)item.Mínimo;
            row.Max = (float)item.Máximo;
            return row;

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