using CrudeObservatory.Abstractions.Interfaces;
using CrudeObservatory.Triggers.Auto;
using CrudeObservatory.Triggers.Delay;
using CrudeObservatory.Triggers.Delay.Models;
using CrudeObservatory.Triggers.Immediate.Models;
using CrudeObservatory.Triggers.Manual;
using CrudeObservatory.Triggers.Manual.Models;

namespace CrudeObservatory.Triggers
{
    public static class TriggersMap
    {
        public static Dictionary<TriggerType, Type> ConfigMap { get; }

        private static Dictionary<Type, TriggerConstructor> constructorMap;

        private delegate ITrigger TriggerConstructor(ITriggerConfig config);

        static TriggersMap()
        {
            ConfigMap = new Dictionary<TriggerType, Type>()
            {
                { TriggerType.Immediate, typeof(ImmediateTriggerConfig) },
                { TriggerType.Manual, typeof(ManualTriggerConfig) },
                { TriggerType.Delay, typeof(DelayTriggerConfig) },
            };

            constructorMap = new Dictionary<Type, TriggerConstructor>()
            {
                { typeof(ImmediateTriggerConfig), (x) =>  new ImmediateTrigger()},
                { typeof(ManualTriggerConfig), (x) =>  new ManualTrigger()},
                { typeof(DelayTriggerConfig), (x) =>  new DelayTrigger((DelayTriggerConfig)x)},
            };
        }

        static public ITrigger GetTrigger(ITriggerConfig config) => constructorMap[config.GetType()].Invoke(config);
    }
}
