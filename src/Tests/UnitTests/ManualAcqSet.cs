using CrudeObservatory.Abstractions.Interfaces;
using CrudeObservatory.Abstractions.Models;
using CrudeObservatory.DataSources.Libplctag.Models;
using CrudeObservatory.DataSources.SineWave.Models;
using CrudeObservatory.DataTargets.CSV.Models;
using CrudeObservatory.Intervals.Fixed.Models;
using CrudeObservatory.Triggers.Auto.Models;
using CrudeObservatory.Triggers.Delay.Models;
using libplctag;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    internal static class ManualAcqSet
    {
        internal static AcquisitionConfig GetAcquisitionConfig()
        {

            AcquisitionConfig acq = new AcquisitionConfig();


            acq.Name = "Manual Prototype Set";
            acq.StartTrigger = new ImmediateTriggerConfig();
            acq.DataSources = new List<IDataSourceConfig>()
            {
                new SineWaveDataSourceConfig(){PeriodSec=5, Alias="Sine1" },
                new SineWaveDataSourceConfig(){PeriodSec=1, Alias="Sine2" },
                new LibplctagDataSourceConfig()
                {
                    //Gateway is the IP Address of the PLC or communication module.
                    Gateway = "10.10.10.17",
                    //Path is the location in the control plane of the CPU. Almost always "1,0".
                    Path = "1,0",
                    PlcType = PlcType.ControlLogix,
                    Protocol = Protocol.ab_eip,
                    TimeoutSeconds = 5,
                    Tags = Enumerable.Range(0, 10).Select(x => new TagConfig(){Name=  $"TestDINT{x.ToString("0000")}", TagType= TagType.Dint }).ToList(),
                },
            };
            acq.Interval = new PeriodicIntervalConfig() { PeriodSec = .5 };
            acq.EndTrigger = new DelayTriggerConfig() { DelaySeconds = 10, };
            acq.DataTarget = new CsvDataTargetConfig() { FilePath = "DataTarget.csv" };

            return acq;

        }

    }
}
