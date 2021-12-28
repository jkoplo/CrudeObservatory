using CrudeObservatory.DataTargets.Abstractions.Models;
using CrudeObservatory.DataTargets.Implementations.CSV;
using CrudeObservatory.DataTargets.Implementations.CSV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.DataSources.Implementations
{
    internal static class DataTargetsMap
    {
        internal static Dictionary<DataTargetType, Type> ConfigMap { get; }

        static DataTargetsMap()
        {
            ConfigMap = new Dictionary<DataTargetType, Type>()
            {
                { DataTargetType.CSV, typeof(CsvDataTargetConfig) },
            };
        }
    }
}
