using CrudeObservatory.Abstractions.Interfaces;
using CrudeObservatory.Abstractions.Models;
using CrudeObservatory.Triggers;
using CrudeObservatory.DataSources;
using CrudeObservatory.DataTargets;
using CrudeObservatory.Intervals;
using JsonSubTypes;
using Newtonsoft.Json;

namespace CrudeObservatory.Acquisition.Services
{
    public class ParseAcquisitionConfig
    {
        private readonly JsonSerializerSettings settings;

        public ParseAcquisitionConfig()
        {
            settings = ConfigFileSerializerSettings();
        }

        public string SerializeToJson(AcquisitionConfig config)
        {
            string serialized = JsonConvert.SerializeObject(config, Formatting.Indented, settings);

            return serialized;
        }

        public AcquisitionConfig DeserializeFromJson(string jsonConfigString)
        {
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
