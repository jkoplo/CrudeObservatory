using CrudeObservatory.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.Triggers.Delay.Models
{
    public class DelayTriggerConfig : ITriggerConfig
    {
        public double DelaySeconds { get; set; }
    }
}
