using CrudeObservatory.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.DataTargets.InfluxDB.Models
{
    public class InfluxDBDataTargetConfig : IDataTargetConfig
    {
        public string Measurement { get; set; }
        public string Bucket { get; set; }
        public string Organization { get; set; }
        public string Token { get; set; }
        public string Url { get; set; }


    }
}
