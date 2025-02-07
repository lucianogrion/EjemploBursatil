using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace EjemploBursatil.Models
{
    [DataContract]
    public class DataPointOHLC
    {
        public DataPointOHLC(long x, double[] y)
        {
            this.X = x;
            this.Y = y;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "x")]
        public long X;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public double[] Y = null;
    }
}