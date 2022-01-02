using CrudeObservatory.Abstractions.Interfaces;

namespace CrudeObservatory.DataSources.SineWave.Models
{
    public class SineWaveDataSourceConfig : IDataSourceConfig
    {
        public int PeriodSec { get; set; }
        public string Alias { get; set; }
    }
}
