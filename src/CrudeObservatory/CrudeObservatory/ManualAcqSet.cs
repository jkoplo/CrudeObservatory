using CrudeObservatory.Abstractions.Models;
using CrudeObservatory.Acquisition.Models;
using CrudeObservatory.Acquisition.Services;

namespace CrudeObservatory
{
    internal static class ManualAcqSet
    {
        internal static AcquisitionConfig GetAcquisitionConfig()
        {
            var jsonConfigString = File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, "AcqConfig.json"));

            //HACK: This ManualAcqSet class will go away once all the infrastructure is in place
            AcquisitionConfig acq = new ParseAcquisitionConfig().DeserializeFromJson(jsonConfigString);

            return acq;

        }

        internal static AcquisitionSet GetAcquisition(AcquisitionConfig acquisitionConfig)
        {
            var factory = new AcquisitionSetFactory();

            var acq = factory.GetAcquisitionSet(acquisitionConfig);

            return acq;

        }
    }
}
