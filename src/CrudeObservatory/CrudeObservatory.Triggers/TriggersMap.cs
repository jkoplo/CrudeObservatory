using CrudeObservatory.Abstractions.Interfaces;
using CrudeObservatory.Triggers.Auto;
using CrudeObservatory.Triggers.Auto.Models;
using CrudeObservatory.Triggers.Delay;
using CrudeObservatory.Triggers.Delay.Models;
using CrudeObservatory.Triggers.Manual;
using CrudeObservatory.Triggers.Manual.Models;

namespace CrudeObservatory.Triggers
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
