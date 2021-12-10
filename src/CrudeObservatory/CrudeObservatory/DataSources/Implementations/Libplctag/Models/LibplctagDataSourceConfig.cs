using CrudeObservatory.DataSources.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.DataSources.Implementations.Libplctag.Models
{
    public class DataSourceConfig : DataSourceConfigBase
    {
        public string TagPath { get; set; }
    }
}
