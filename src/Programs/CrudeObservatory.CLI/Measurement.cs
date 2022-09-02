using CrudeObservatory.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.CLI
{
    public class Measurement
    {
        public IIntervalOutput IntervalOutput { get; set; }
        public IEnumerable<IDataValue> DataValues { get; set; }
    }
}
