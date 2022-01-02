using CrudeObservatory.Acquisition.Models;
using CrudeObservatory.Abstractions.Models;
using CrudeObservatory.Triggers;
using CrudeObservatory.DataSources;
using CrudeObservatory.DataTargets;
using CrudeObservatory.Intervals;

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
            acq.DataTarget = DataTargetsMap.GetDataSource(config.DataTarget);

            //Interval
            acq.Interval = IntervalsMap.GetDataSource(config.Interval);

            return acq;

        }

    }
}
