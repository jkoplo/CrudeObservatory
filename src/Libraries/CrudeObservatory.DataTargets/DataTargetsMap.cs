using CrudeObservatory.Abstractions.Interfaces;
using CrudeObservatory.Abstractions.Models;
using CrudeObservatory.DataTargets.CSV;
using CrudeObservatory.DataTargets.CSV.Models;

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
            };
            constructorMap = new Dictionary<Type, TriggerConstructor>()
            {
                { typeof(CsvDataTargetConfig), (x) =>  new CsvDataTarget((CsvDataTargetConfig)x)},
            };

        }

        public static IDataTarget GetDataSource(IDataTargetConfig config) => constructorMap[config.GetType()].Invoke(config);
    }
}
