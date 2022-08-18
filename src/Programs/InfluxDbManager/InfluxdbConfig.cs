using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfluxDbManager
{
    public class InfluxdbConfig
    {
        public string Bucket { get; set; }
        public string Organization { get; set; }
        public string Token { get; set; }
        public string Url { get; set; }
    }
}
