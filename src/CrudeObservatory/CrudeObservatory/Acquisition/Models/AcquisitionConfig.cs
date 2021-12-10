using CrudeObservatory.DataSources.Abstractions.Models;
using CrudeObservatory.DataTargets.Abstractions.Models;
using CrudeObservatory.Intervals.Abstractions.Models;
using CrudeObservatory.Triggers.Abstractions.Models;
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
        public TriggerConfigBase StartTrigger { get; set; }
        public TriggerConfigBase EndTrigger { get; set; }


        public IntervalConfigBase Interval { get; set; }
        public List<DataSourceConfigBase> DataSources { get; set; }
        public DataTargetConfigBase DataTarget { get; set; }
    }



}
