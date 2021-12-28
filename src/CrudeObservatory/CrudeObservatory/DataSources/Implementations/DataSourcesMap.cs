using CrudeObservatory.DataSources.Abstractions.Models;
using CrudeObservatory.DataSources.Implementations.Libplctag;
using CrudeObservatory.DataSources.Implementations.Libplctag.Models;
using CrudeObservatory.DataSources.Implementations.SineWave;
using CrudeObservatory.DataSources.Implementations.SineWave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.DataSources.Implementations
{
    internal static class DataSourcesMap
    {
        internal static Dictionary<DataSourceType, Type> ConfigMap { get; }

        static DataSourcesMap()
        {
            ConfigMap = new Dictionary<DataSourceType, Type>()
            {
                { DataSourceType.libplctag, typeof(LibplctagDataSourceConfig) },
                { DataSourceType.SineWave, typeof(SineWaveDataSourceConfig) },
            };
        }
    }
}
