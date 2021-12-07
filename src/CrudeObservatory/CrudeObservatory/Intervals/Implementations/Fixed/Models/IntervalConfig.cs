using CrudeObservatory.Intervals.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.Intervals.Implementations.Fixed.Models
{
    public class IntervalConfig : IntervalConfigBase
    {
        public int Rate { get; set; }
    }
}
