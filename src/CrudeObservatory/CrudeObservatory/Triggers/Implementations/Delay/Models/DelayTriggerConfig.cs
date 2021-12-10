using CrudeObservatory.Triggers.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.Triggers.Implementations.Delay.Models
{
    internal class DelayTriggerConfig : TriggerConfigBase
    {
        public double DelaySeconds { get; set; }
    }
}
