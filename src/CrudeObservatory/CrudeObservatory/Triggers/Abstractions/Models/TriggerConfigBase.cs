using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.Triggers.Abstractions.Models
{
    public abstract class TriggerConfigBase
    {
        public bool Enabled { get; set; }
        public TriggerType Type { get; set; }

    }
}
