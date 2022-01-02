﻿using CrudeObservatory.Abstractions.Interfaces;
using CrudeObservatory.Abstractions.Models;
using CrudeObservatory.Intervals.Fixed;
using CrudeObservatory.Intervals.Fixed.Models;

namespace CrudeObservatory.Intervals
{
    public static class IntervalsMap
    {
        public static Dictionary<IntervalType, Type> ConfigMap { get; }
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

        public static IInterval GetDataSource(IIntervalConfig config) => constructorMap[config.GetType()].Invoke(config);
    }
}