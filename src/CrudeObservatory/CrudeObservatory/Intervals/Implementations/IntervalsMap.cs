using CrudeObservatory.Intervals.Abstractions.Models;
using CrudeObservatory.Intervals.Implementations.Fixed;
using CrudeObservatory.Intervals.Implementations.Fixed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.Intervals.Implementations
{
    internal static class IntervalsMap
    {
        internal static Dictionary<IntervalType, Type> ConfigMap { get; }

        static IntervalsMap()
        {
            ConfigMap = new Dictionary<IntervalType, Type>()
            {
                { IntervalType.Fixed, typeof(FixedIntervalConfig) },
            };
        }
    }
}
