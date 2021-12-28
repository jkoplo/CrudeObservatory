using CrudeObservatory.DataTargets.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.DataTargets.Implementations.CSV.Models
{
    public class CsvDataTargetConfig : IDataTargetConfig
    {
        public string FilePath { get; set; }
    }
}
