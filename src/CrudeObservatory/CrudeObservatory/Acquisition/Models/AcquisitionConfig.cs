using CrudeObservatory.Triggers.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.Config.Models
{
    public class AcquisitionConfig
    {
        public string Name { get; set; }
        public TriggerConfigBase StartTrigger { get; set; }
        public Interval Interval { get; set; }
        public List<string> DataSources { get; set; }
        public DataTarget DataTarget { get; set; }
    }

    public class Start
    {
        public string Type { get; set; }
    }

    public class End
    {
        public string Type { get; set; }
    }

    public class Trigger
    {
        public bool Enabled { get; set; }
        public Start Start { get; set; }
        public End End { get; set; }
    }

    public class Interval
    {
        public string Type { get; set; }
        public int Rate { get; set; }
    }

    public class DataTarget
    {
        public string Type { get; set; }
        public string FilePath { get; set; }
        public bool IncludeHeader { get; set; }
    }



    public class Root
    {
        public AcquisitionConfig Acquisition { get; set; }
    }

}
