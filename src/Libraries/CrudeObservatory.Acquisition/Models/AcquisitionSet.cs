using CrudeObservatory.Abstractions.Interfaces;

namespace CrudeObservatory.Acquisition.Models
{
    public class AcquisitionSet
    {
        public string Name { get; set; }
        public ITrigger StartTrigger { get; set; }
        public ITrigger EndTrigger { get; set; }

        public IInterval Interval { get; set; }
        public List<IDataSource> DataSources { get; set; }
        public List<IDataTarget> DataTargets { get; set; }
    }
}
