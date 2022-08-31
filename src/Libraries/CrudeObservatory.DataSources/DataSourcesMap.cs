using CrudeObservatory.Abstractions.Interfaces;
using CrudeObservatory.Abstractions.Models;
using CrudeObservatory.DataSources.Libplctag;
using CrudeObservatory.DataSources.Libplctag.Models;
using CrudeObservatory.DataSources.SineWave;
using CrudeObservatory.DataSources.SineWave.Models;

namespace CrudeObservatory.DataSources
{
    public static class DataSourcesMap
    {
        public static Dictionary<DataSourceType, Type> ConfigMap { get; }

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
                { typeof(LibplctagDataSourceConfig), (x) => new LibPlcTagDataSource((LibplctagDataSourceConfig)x) },
                { typeof(SineWaveDataSourceConfig), (x) => new SineWaveDataSource((SineWaveDataSourceConfig)x) },
            };
        }

        public static IDataSource GetDataSource(IDataSourceConfig config) =>
            constructorMap[config.GetType()].Invoke(config);
    }
}
