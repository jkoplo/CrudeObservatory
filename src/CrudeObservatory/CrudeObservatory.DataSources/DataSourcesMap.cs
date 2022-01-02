using CrudeObservatory.DataSources.Abstractions.Interfaces;
using CrudeObservatory.DataSources.Abstractions.Models;
using CrudeObservatory.DataSources.Implementations.Libplctag;
using CrudeObservatory.DataSources.Implementations.SineWave;
using CrudeObservatory.DataSources.Libplctag.Models;
using CrudeObservatory.DataSources.SineWave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.DataSources
{
    internal static class DataSourcesMap
    {
        internal static Dictionary<DataSourceType, Type> ConfigMap { get; }

        private static Dictionary<Type, TriggerConstructor> constructorMap;

        private delegate IDataSource TriggerConstructor(IDataSourceConfig config);


        static DataSourcesMap()
        {
            ConfigMap = new Dictionary<DataSourceType, Type>()
            {
                { DataSourceType.libplctag, typeof(LibplctagDataSourceConfig) },
                { DataSourceType.SineWave, typeof(SineWaveDataSourceConfig) },
            };

            constructorMap = new Dictionary<Type, TriggerConstructor>()
            {
                { typeof(LibplctagDataSourceConfig), (x) =>  new LibPlcTagDataSource((LibplctagDataSourceConfig)x)},
                { typeof(SineWaveDataSourceConfig), (x) =>  new SineWaveDataSource((SineWaveDataSourceConfig)x)},
            };
        }

        internal static IDataSource GetDataSource(IDataSourceConfig config) => constructorMap[config.GetType()].Invoke(config);
    }
}
