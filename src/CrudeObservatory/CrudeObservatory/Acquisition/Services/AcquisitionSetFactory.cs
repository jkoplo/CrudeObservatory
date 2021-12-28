using CrudeObservatory.Acquisition.Models;
using CrudeObservatory.DataSources.Implementations.SineWave;
using CrudeObservatory.DataSources.Implementations.SineWave.Models;
using CrudeObservatory.DataSources.Implementations.Libplctag;
using CrudeObservatory.DataSources.Implementations.Libplctag.Models;
using CrudeObservatory.Triggers.Implementations.Automatic;
using CrudeObservatory.DataSources.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrudeObservatory.Intervals.Implementations.Fixed;
using CrudeObservatory.Intervals.Implementations.Fixed.Models;
using CrudeObservatory.Intervals.Abstractions.Models;
using CrudeObservatory.Triggers.Implementations.Delay;
using CrudeObservatory.Triggers.Implementations.Delay.Models;
using CrudeObservatory.DataTargets.Implementations.CSV;
using CrudeObservatory.DataTargets.Implementations.CSV.Models;
using CrudeObservatory.DataSources.Abstractions.Models;
using libplctag;
using CrudeObservatory.Acquisition.Services;
using CrudeObservatory.Triggers.Abstractions.Interfaces;
using CrudeObservatory.Triggers.Implementations;
using CrudeObservatory.DataSources.Implementations;
using CrudeObservatory.Intervals.Implementations;

namespace CrudeObservatory.Acquisition.Services
{
    internal class AcquisitionSetFactory
    {
        internal AcquisitionSet GetAcquisitionSet(AcquisitionConfig config)
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
