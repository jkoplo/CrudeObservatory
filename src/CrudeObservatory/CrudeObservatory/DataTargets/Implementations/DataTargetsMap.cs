using CrudeObservatory.DataTargets.Abstractions.Interfaces;
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

        internal static IDataTarget GetDataSource(IDataTargetConfig config) => constructorMap[config.GetType()].Invoke(config);
    }
}
