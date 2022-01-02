using CrudeObservatory.Abstractions.Interfaces;

namespace CrudeObservatory.Intervals.Fixed.Models
{
    public class FixedIntervalConfig : IIntervalConfig
    {
        public double PeriodSec { get; set; }
    }
}
