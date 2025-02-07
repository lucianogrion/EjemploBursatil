using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EjemploBursatil.DAL
{
    [JsonObject]
    public class RowCandleStick
    {
        private String fecha;
        
        [JsonProperty(PropertyName="Fecha")]
        public String Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        private float max;
        
        [JsonProperty(PropertyName = "Max")]
        public float Max
        {
            get { return max; }
            set { max = value; }
        }
        private float min;
        [JsonProperty(PropertyName = "Min")]
        public float Min
        {
            get { return min; }
            set { min = value; }
        }
        private float open;
        [JsonProperty(PropertyName = "Open")]
        public float Open
        {
            get { return open; }
            set { open = value; }
        }
        private float close;
        [JsonProperty(PropertyName = "Close")]
        public float Close
        {
            get { return close; }
            set { close = value; }
        }
    }
}