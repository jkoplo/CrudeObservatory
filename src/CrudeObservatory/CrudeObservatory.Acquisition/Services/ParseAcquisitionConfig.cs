using CrudeObservatory.Acquisition.Models;
using CrudeObservatory.DataSources.Abstractions.Interfaces;
using CrudeObservatory.DataSources.Implementations;
using CrudeObservatory.DataTargets.Abstractions.Interfaces;
using CrudeObservatory.Intervals.Abstractions.Interfaces;
using CrudeObservatory.Intervals.Implementations;
using CrudeObservatory.Triggers.Abstractions.Interfaces;
using CrudeObservatory.Triggers.Implementations;
using JsonSubTypes;
using Newtonsoft.Json;

namespace CrudeObservatory.Acquisition.Services
{
    internal class ParseAcquisitionConfig
    {
        internal string SerializeToJson(AcquisitionConfig config)
        {
            JsonSerializerSettings settings = ConfigFileSerializerSettings();

            string serialized = JsonConvert.SerializeObject(config, Formatting.Indented, settings);

            return serialized;
        }

        internal AcquisitionConfig DeserializeFromJson(string jsonConfigString)
        {
            JsonSerializerSettings settings = ConfigFileSerializerSettings();

            var config = JsonConvert.DeserializeObject<AcquisitionConfig>(jsonConfigString, settings);

            return config;
        }



        private JsonSerializerSettings ConfigFileSerializerSettings()
        {
            var settings = new JsonSerializerSettings();

            //Triggers
            settings.Converters.Add(
                MapTypes(typeof(ITriggerConfig), TriggersMap.ConfigMap.ToDictionary(x => x.Key as object, x => x.Value))
            );

            //Intervals
            settings.Converters.Add(
                MapTypes(typeof(IIntervalConfig), IntervalsMap.ConfigMap.ToDictionary(x => x.Key as object, x => x.Value))
            );

            //Data Sources
            settings.Converters.Add(
                MapTypes(typeof(IDataSourceConfig), DataSourcesMap.ConfigMap.ToDictionary(x => x.Key as object, x => x.Value))
            );

            //Data Targets
            settings.Converters.Add(
                MapTypes(typeof(IDataTargetConfig), DataTargetsMap.ConfigMap.ToDictionary(x => x.Key as object, x => x.Value))
            );

            settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            return settings;
        }

        private JsonConverter MapTypes(Type baseType, Dictionary<object, Type> map)
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
