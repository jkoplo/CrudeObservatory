using CrudeObservatory.Abstractions.Interfaces;

namespace CrudeObservatory.Abstractions.Models
{
    public class AcquisitionConfig
    {
        public string Name { get; set; }
        public ITriggerConfig StartTrigger { get; set; }
        public ITriggerConfig EndTrigger { get; set; }


        public IIntervalConfig Interval { get; set; }
        public List<IDataSourceConfig> DataSources { get; set; }
        public List<IDataTargetConfig> DataTargets { get; set; }
    }



}
