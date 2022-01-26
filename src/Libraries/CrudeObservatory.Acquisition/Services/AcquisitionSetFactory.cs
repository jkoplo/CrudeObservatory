using CrudeObservatory.Abstractions.Models;
using CrudeObservatory.Acquisition.Models;
using CrudeObservatory.DataSources;
using CrudeObservatory.DataTargets;
using CrudeObservatory.Intervals;
using CrudeObservatory.Triggers;

namespace CrudeObservatory.Acquisition.Services
{
    public class AcquisitionSetFactory
    {
        public AcquisitionSet GetAcquisitionSet(AcquisitionConfig config)
        {
            AcquisitionSet acq = new AcquisitionSet();

            acq.Name = config.Name;
            //Start Trigger
            acq.StartTrigger = TriggersMap.GetTrigger(config.StartTrigger);

            //End Trigger
            acq.EndTrigger = TriggersMap.GetTrigger(config.EndTrigger);

            //Data Source
            acq.DataSources = config.DataSources.Select(x => DataSourcesMap.GetDataSource(x)).ToList();

            //Data Target
            acq.DataTargets = config.DataTargets.Select(x => DataTargetsMap.GetDataTarget(x)).ToList();

            //Interval
            acq.Interval = IntervalsMap.GetDataSource(config.Interval);

            return acq;

        }

    }
}
