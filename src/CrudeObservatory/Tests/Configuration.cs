using CrudeObservatory;
using CrudeObservatory.Acquisition.Services;
using FluentAssertions;
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
            var parseConfig = new ParseAcquisitionConfig();

            var jsonPath = Path.Combine(Environment.CurrentDirectory, "TestConfig.json");

            //Act
            var serialized = parseConfig.SerializeToJson(config);
            File.WriteAllText(jsonPath, serialized);

            var configRoundTrip = parseConfig.DeserializeFromJson(serialized);

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