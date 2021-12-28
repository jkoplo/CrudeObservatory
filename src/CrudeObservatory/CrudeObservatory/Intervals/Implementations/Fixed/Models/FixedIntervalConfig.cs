using CrudeObservatory.Intervals.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.Intervals.Implementations.Fixed.Models
{
    public class FixedIntervalConfig : IIntervalConfig
    {
        public double PeriodSec { get; set; }
    }
}
