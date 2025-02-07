using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EjemploBursatil.Controllers
{
    public class csvController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { System.IO.File.ReadAllText(@"C:\tmp\ArchivoCSV.csv")};
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return System.IO.File.ReadAllText(@"C:\tmp\ArchivoCSV.csv");
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