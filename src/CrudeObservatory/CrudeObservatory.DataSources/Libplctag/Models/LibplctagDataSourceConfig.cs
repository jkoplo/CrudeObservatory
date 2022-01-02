using CrudeObservatory.DataSources.Abstractions.Interfaces;
using libplctag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.DataSources.Libplctag.Models
{
    public class LibplctagDataSourceConfig : IDataSourceConfig
    {
        public string Gateway { get; set; }
        public string Path { get; set; }
        public Protocol Protocol { get; set; }
        public PlcType PlcType { get; set; }
        public float TimeoutSeconds { get; set; }

        public List<TagConfig> Tags { get; set; }

    }
}
