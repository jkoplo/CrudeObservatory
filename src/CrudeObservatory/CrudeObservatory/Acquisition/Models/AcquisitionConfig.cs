using CrudeObservatory.DataSources.Abstractions.Interfaces;
using CrudeObservatory.DataTargets.Abstractions.Interfaces;
using CrudeObservatory.Intervals.Abstractions.Interfaces;
using CrudeObservatory.Triggers.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.Acquisition.Models
{
    public class AcquisitionConfig
    {
        public string Name { get; set; }
        public ITriggerConfig StartTrigger { get; set; }
        public ITriggerConfig EndTrigger { get; set; }


        public IIntervalConfig Interval { get; set; }
        public List<IDataSourceConfig> DataSources { get; set; }
        public IDataTargetConfig DataTarget { get; set; }
    }



}
