using CrudeObservatory.Acquisition.Models;
using CrudeObservatory.DataSources.Abstractions.Models;
using CrudeObservatory.DataSources.Implementations;
using CrudeObservatory.DataSources.Implementations.Libplctag.Models;
using CrudeObservatory.DataSources.Implementations.SineWave.Models;
using CrudeObservatory.DataTargets.Abstractions.Models;
using CrudeObservatory.DataTargets.Implementations.CSV.Models;
using CrudeObservatory.Intervals.Abstractions.Models;
using CrudeObservatory.Intervals.Implementations;
using CrudeObservatory.Intervals.Implementations.Fixed.Models;
using CrudeObservatory.Triggers.Abstractions.Models;
using CrudeObservatory.Triggers.Implementations;
using CrudeObservatory.Triggers.Implementations.Delay.Models;
using CrudeObservatory.Triggers.Implementations.Manual.Models;
using JsonSubTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.Acquisition.Services
{
    internal static class ParseAcquisitionConfig
    {
        internal static string SerializeToJson(AcquisitionConfig config)
        {
            JsonSerializerSettings settings = ConfigFileSerializerSettings();

            string serialized = JsonConvert.SerializeObject(config, Formatting.Indented, settings);

            return serialized;
        }

        internal static AcquisitionConfig DeserializeFromJson(string jsonConfigString)
        {
            JsonSerializerSettings settings = ConfigFileSerializerSettings();

            var config = JsonConvert.DeserializeObject<AcquisitionConfig>(jsonConfigString, settings);

            return config;
        }



        private static JsonSerializerSettings ConfigFileSerializerSettings()
        {
            var settings = new JsonSerializerSettings();

            //Triggers
            settings.Converters.Add(
                MapTypes(settings, typeof(TriggerConfigBase), TriggerMap.ConfigMap.ToDictionary(x => x.Key as object, x => x.Value))
            );

            //Intervals
            settings.Converters.Add(
                MapTypes(settings, typeof(IntervalConfigBase), IntervalsMap.ConfigMap.ToDictionary(x => x.Key as object, x => x.Value))
            );

            //Data Sources
            settings.Converters.Add(
                MapTypes(settings, typeof(DataSourceConfigBase), DataSourcesMap.ConfigMap.ToDictionary(x => x.Key as object, x => x.Value))
            );

            //Data Targets
            settings.Converters.Add(
                MapTypes(settings, typeof(DataTargetConfigBase), DataTargetsMap.ConfigMap.ToDictionary(x => x.Key as object, x => x.Value))
            );

            settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            return settings;
        }

        private static JsonConverter MapTypes(JsonSerializerSettings settings, Type baseType, Dictionary<object, Type> map)
        {
            var triggerConverterBuilder = JsonSubtypesConverterBuilder
                .Of(baseType, "Type"); // type property is only defined here

            foreach (var item in map)
            {
                triggerConverterBuilder.RegisterSubtype(item.Value, item.Key);
            }


            return triggerConverterBuilder
                .SerializeDiscriminatorProperty() // ask to serialize the type property
                .Build();
        }
    }
}
