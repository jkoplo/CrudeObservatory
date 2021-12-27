using CrudeObservatory;
using CrudeObservatory.Acquisition.Services;
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
            var serialized = ParseAcquisitionConfig.SerializeToJson(config);
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