using CrudeObservatory.Abstractions.Interfaces;

namespace CrudeObservatory.DataTargets.CSV.Models
{
    public class CsvDataTargetConfig : IDataTargetConfig
    {
        public string FilePath { get; set; }
    }
}
