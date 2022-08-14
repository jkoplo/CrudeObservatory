using CrudeObservatory.Abstractions.Interfaces;
using CrudeObservatory.Abstractions.Models;
using CrudeObservatory.DataTargets.CSV;
using CrudeObservatory.DataTargets.CSV.Models;
using CrudeObservatory.DataTargets.InfluxDB;
using CrudeObservatory.DataTargets.InfluxDB.Models;

namespace CrudeObservatory.DataTargets
{
    public static class DataTargetsMap
    {
        public static Dictionary<DataTargetType, Type> ConfigMap { get; }
        private static Dictionary<Type, TriggerConstructor> constructorMap;
        private delegate IDataTarget TriggerConstructor(IDataTargetConfig config);

        static DataTargetsMap()
        {
            ConfigMap = new Dictionary<DataTargetType, Type>()
            {
                { DataTargetType.CSV, typeof(CsvDataTargetConfig) },
                { DataTargetType.InfluxDB, typeof(InfluxDBDataTargetConfig) },
            };
            constructorMap = new Dictionary<Type, TriggerConstructor>()
            {
                { typeof(CsvDataTargetConfig), (x) => new CsvDataTarget((CsvDataTargetConfig)x) },
                { typeof(InfluxDBDataTargetConfig), (x) => new InfluxDBDataTarget((InfluxDBDataTargetConfig)x) },
            };
        }

        public static IDataTarget GetDataTarget(IDataTargetConfig config) =>
            constructorMap[config.GetType()].Invoke(config);
    }
}
