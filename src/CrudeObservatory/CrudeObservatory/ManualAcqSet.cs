﻿using CrudeObservatory.Acquisition.Models;
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
using CrudeObservatory.Triggers.Abstractions.Models;
using CrudeObservatory.DataTargets.Implementations.CSV;
using CrudeObservatory.DataTargets.Implementations.CSV.Models;
using CrudeObservatory.DataSources.Abstractions.Models;
using libplctag;

namespace CrudeObservatory
{
    internal static class ManualAcqSet
    {
        internal static AcquisitionConfig GetAcquisitionConfig()
        {
            AcquisitionConfig acq = new AcquisitionConfig();


            acq.Name = "Manual Prototype Set";
            acq.StartTrigger = new TriggerConfigBase() { Type = TriggerType.Auto };
            acq.DataSources = new List<DataSourceConfigBase>()
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
                    Tags = Enumerable.Range(0, 10).Select(x => $"TestDINT{x.ToString("0000")}").ToList(),
                },
            };
            acq.Interval = new FixedIntervalConfig() { PeriodSec = .5, Type = IntervalType.Fixed };
            acq.EndTrigger = new DelayTriggerConfig() { Type = TriggerType.Delay, DelaySeconds = 10, Enabled = true };
            acq.DataTarget = new CsvDataTargetConfig() { FilePath = Path.Combine(System.Environment.CurrentDirectory, "DataTarget.csv") };

            return acq;

        }

        internal static AcquisitionSet GetAcquisition()
        {
            var config = ManualAcqSet.GetAcquisitionConfig();

            AcquisitionSet acq = new AcquisitionSet();

            acq.Name = "Manual Prototype Set";
            acq.StartTrigger = new AutoTrigger();
            acq.DataSources = new List<IDataSource>();
            acq.DataSources.AddRange(config.DataSources.OfType<SineWaveDataSourceConfig>().Select(x => new SineWaveDataSource(x)).ToList<IDataSource>());
            acq.DataSources.AddRange(config.DataSources.OfType<LibplctagDataSourceConfig>().Select(x => new LibPlcTagDataSource(x)).ToList<IDataSource>());
            acq.Interval = new FixedInterval((FixedIntervalConfig)config.Interval);
            acq.EndTrigger = new DelayTrigger((DelayTriggerConfig)config.EndTrigger);
            acq.DataTarget = new CsvDataTarget((CsvDataTargetConfig)config.DataTarget);

            return acq;

        }
    }
}
