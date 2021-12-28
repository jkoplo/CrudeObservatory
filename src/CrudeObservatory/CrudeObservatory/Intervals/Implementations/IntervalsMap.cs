using CrudeObservatory.Intervals.Abstractions.Interfaces;
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
        private static Dictionary<Type, TriggerConstructor> constructorMap;
        private delegate IInterval TriggerConstructor(IIntervalConfig config);

        static IntervalsMap()
        {
            ConfigMap = new Dictionary<IntervalType, Type>()
            {
                { IntervalType.Fixed, typeof(FixedIntervalConfig) },
            }; 
            constructorMap = new Dictionary<Type, TriggerConstructor>()
            {
                { typeof(FixedIntervalConfig), (x) =>  new FixedInterval((FixedIntervalConfig)x)},
            };

        }

        internal static IInterval GetDataSource(IIntervalConfig config)=> constructorMap[config.GetType()].Invoke(config);
    }
}
