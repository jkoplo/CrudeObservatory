using CrudeObservatory.Triggers.Abstractions.Interfaces;
using CrudeObservatory.Triggers.Abstractions.Models;
using CrudeObservatory.Triggers.Implementations.Auto.Models;
using CrudeObservatory.Triggers.Implementations.Automatic;
using CrudeObservatory.Triggers.Implementations.Delay;
using CrudeObservatory.Triggers.Implementations.Delay.Models;
using CrudeObservatory.Triggers.Implementations.Manual;
using CrudeObservatory.Triggers.Implementations.Manual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.Triggers.Implementations
{
    internal static class TriggersMap
    {
        internal static Dictionary<TriggerType, Type> ConfigMap { get; }

        private static Dictionary<Type, TriggerConstructor> constructorMap;

        private delegate ITrigger TriggerConstructor(ITriggerConfig config);

        static TriggersMap()
        {
            ConfigMap = new Dictionary<TriggerType, Type>()
            {
                { TriggerType.Auto, typeof(AutoTriggerConfig) },
                { TriggerType.Manual, typeof(ManualTriggerConfig) },
                { TriggerType.Delay, typeof(DelayTriggerConfig) },
            };

            constructorMap = new Dictionary<Type, TriggerConstructor>()
            {
                { typeof(AutoTriggerConfig), (x) =>  new AutoTrigger()},
                { typeof(ManualTriggerConfig), (x) =>  new ManualTrigger()},
                { typeof(DelayTriggerConfig), (x) =>  new DelayTrigger((DelayTriggerConfig)x)},
            };
        }

        static internal ITrigger GetTrigger(ITriggerConfig config) => constructorMap[config.GetType()].Invoke(config);
    }
}
