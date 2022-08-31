using CrudeObservatory.Abstractions.Interfaces;

namespace CrudeObservatory.DataSources.SineWave.Models
{
    public class SineWaveDataSourceConfig : IDataSourceConfig
    {
        public int PeriodSec { get; set; }
        public double Frequency { get; set; }
        public double Phase { get; set; }
        public double Amplitude { get; set; }
        public double Offset { get; set; }
        public bool Invert { get; set; }
        public string Alias { get; set; }
    }
}
