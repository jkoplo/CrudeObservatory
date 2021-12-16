using CrudeObservatory.Acquisition.Models;
using CrudeObservatory.DataSources.Implementations.SineWave;
using CrudeObservatory.DataSources.Implementations.SineWave.Models;
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
using CrudeObservatory.Triggers.Abstractions.Models;
using CrudeObservatory.DataTargets.Implementations.CSV;
using CrudeObservatory.DataTargets.Implementations.CSV.Models;

namespace CrudeObservatory
{
    internal static class ManualAcqSet
    {
        internal static AcquisitionSet GetAcquisition()
        {
            AcquisitionSet acq = new AcquisitionSet();

            acq.Name = "Manual Prototype Set";
            acq.StartTrigger = new AutoTrigger();
            acq.DataSources = new List<IDataSource>()
            {
                new SineWaveDataSource(new SineWaveDataSourceConfig(){PeriodSec=5, Alias="SineWave" })
            };
            acq.Interval = new FixedInterval(new FixedIntervalConfig() { PeriodSec = .1, Type = IntervalType.Fixed });
            acq.EndTrigger = new DelayTrigger(new DelayTriggerConfig() { Type = TriggerType.Delay, DelaySeconds = 10, Enabled = true });
            acq.DataTarget = new CsvDataTarget(new CsvDataTargetConfig() { FilePath = Path.Combine(System.Environment.CurrentDirectory, "DataTarget.csv") });

            return acq;

        }
    }
}
