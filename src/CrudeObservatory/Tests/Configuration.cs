using CrudeObservatory.DataSources.Abstractions.Models;
using CrudeObservatory.DataSources.Implementations.Libplctag.Models;
using CrudeObservatory.DataSources.Implementations.SineWave.Models;
using JsonSubTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using Xunit;

namespace Tests
{
    public class Configuration
    {

        [Fact]
        public void SerializeConfigToJson_TestData_ValidFile()
        {
            //Arrange
            var config = ManualAcqSet.GetAcquisitionConfig();
            var nameGuid = Guid.NewGuid().ToString();
            config.Name = nameGuid;
            var jsonPath = Path.Combine(Environment.CurrentDirectory, "TestConfig.json");


            //Act
            //JsonSerializer serializer = new JsonSerializer();
            //serializer.Converters.Add(new JavaScriptDateTimeConverter());
            //serializer.NullValueHandling = NullValueHandling.Ignore;

            var settings = new JsonSerializerSettings( );
            settings.Converters.Add(JsonSubtypesConverterBuilder
                .Of(typeof(DataSourceConfigBase), "ObjectType") // type property is only defined here
                .RegisterSubtype(typeof(SineWaveDataSourceConfig), DataSourceType.SineWave)
                .RegisterSubtype(typeof(LibplctagDataSourceConfig), DataSourceType.libplctag)
                .SerializeDiscriminatorProperty() // ask to serialize the type property
                .Build());
            settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            //JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

            string serialized = JsonConvert.SerializeObject(config, settings);
            
            //List<Base> deserializedList = JsonConvert.DeserializeObject<List<Base>>(Serialized, settings);

            File.WriteAllText(jsonPath, serialized);

            //Assert
            //Just check that the serialized file actually contains the guid (a serialization happened)
            var jsonOutput = File.ReadAllText(jsonPath);
            Assert.Contains(nameGuid, jsonOutput);
        }

        [Fact]
        public void ParseConfig_DynamicTypes_SpecificClassLoaded()
        {
            //Arrange


            //Act
            //Assert
            Assert.True(true);
        }
    }
}