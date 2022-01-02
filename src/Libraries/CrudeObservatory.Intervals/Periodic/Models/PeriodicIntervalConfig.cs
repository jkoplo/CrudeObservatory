using CrudeObservatory.Abstractions.Interfaces;

namespace CrudeObservatory.Intervals.Fixed.Models
{
    public class PeriodicIntervalConfig : IIntervalConfig
    {
        public double PeriodSec { get; set; }
    }
}
