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
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(jsonPath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, config);
            }

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