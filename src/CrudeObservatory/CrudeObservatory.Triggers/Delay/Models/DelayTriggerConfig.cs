using CrudeObservatory.Abstractions.Interfaces;

namespace CrudeObservatory.Triggers.Delay.Models
{
    public class DelayTriggerConfig : ITriggerConfig
    {
        public double DelaySeconds { get; set; }
    }
}
