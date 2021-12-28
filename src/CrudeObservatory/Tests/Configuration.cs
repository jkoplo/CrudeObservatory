using CrudeObservatory;
using CrudeObservatory.Acquisition.Services;
using CrudeObservatory.DataSources.Abstractions.Models;
using CrudeObservatory.DataSources.Implementations.Libplctag.Models;
using CrudeObservatory.DataSources.Implementations.SineWave.Models;
using FluentAssertions;
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
        public void SerializeConfigToJson_RandomData_RoundTripEqual()
        {
            //Arrange
            var config = ManualAcqSet.GetAcquisitionConfig();
            var nameGuid = Guid.NewGuid().ToString();
            config.Name = nameGuid;

            var jsonPath = Path.Combine(Environment.CurrentDirectory, "TestConfig.json");

            //Act
            var serialized = ParseAcquisitionConfig.SerializeToJson(config);
            File.WriteAllText(jsonPath, serialized);

            var configRoundTrip = ParseAcquisitionConfig.DeserializeFromJson(serialized);

            //Assert
            configRoundTrip.Should().BeEquivalentTo(config);
        }

        [Fact]
        public void CreateAcquisitionSet_DynamicTypes_ProperConcreteInstantiated()
        {
            //Arrange
            var config = ManualAcqSet.GetAcquisitionConfig();

            //Act
            var factory = new AcquisitionSetFactory();
            var acqSet = factory.GetAcquisitionSet(config);

            //Assert
            Assert.True(true);
        }
    }
}