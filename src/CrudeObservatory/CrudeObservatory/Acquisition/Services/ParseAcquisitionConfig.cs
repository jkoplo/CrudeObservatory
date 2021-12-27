using CrudeObservatory.Acquisition.Models;
using CrudeObservatory.DataSources.Abstractions.Models;
using CrudeObservatory.DataSources.Implementations.Libplctag.Models;
using CrudeObservatory.DataSources.Implementations.SineWave.Models;
using CrudeObservatory.DataTargets.Abstractions.Models;
using CrudeObservatory.DataTargets.Implementations.CSV.Models;
using CrudeObservatory.Intervals.Abstractions.Models;
using CrudeObservatory.Intervals.Implementations.Fixed.Models;
using CrudeObservatory.Triggers.Abstractions.Models;
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
            settings.Converters.Add(JsonSubtypesConverterBuilder
                .Of<TriggerConfigBase>("Type") // type property is only defined here
                .RegisterSubtype<TriggerConfigBase>(TriggerType.Auto)
                .RegisterSubtype<DelayTriggerConfig>(TriggerType.Delay)
                .RegisterSubtype<ManualTriggerConfig>(TriggerType.Manual)
                .SerializeDiscriminatorProperty() // ask to serialize the type property
                .Build());

            //Intervals
            settings.Converters.Add(JsonSubtypesConverterBuilder
                .Of<IntervalConfigBase>("Type") // type property is only defined here
                .RegisterSubtype<FixedIntervalConfig>(IntervalType.Fixed)
                .SerializeDiscriminatorProperty() // ask to serialize the type property
                .Build());

            //Data Sources
            settings.Converters.Add(JsonSubtypesConverterBuilder
                .Of<DataSourceConfigBase>("Type") // type property is only defined here
                .RegisterSubtype<SineWaveDataSourceConfig>(DataSourceType.SineWave)
                .RegisterSubtype<LibplctagDataSourceConfig>(DataSourceType.libplctag)
                .SerializeDiscriminatorProperty() // ask to serialize the type property
                .Build());

            //Data Targets
            settings.Converters.Add(JsonSubtypesConverterBuilder
                .Of<DataTargetConfigBase>("Type") // type property is only defined here
                .RegisterSubtype<CsvDataTargetConfig>(DataTargetType.CSV)
                .SerializeDiscriminatorProperty() // ask to serialize the type property
                .Build());

            settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            return settings;
        }
    }
}
