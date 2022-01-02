using CrudeObservatory.Abstractions.Interfaces;

namespace CrudeObservatory.Intervals.Fixed
{
    internal class DataValue : IDataValue
    {
        public string Name { get; set; }
        public long? Value { get; set; }
        object IDataValue.Value { get => Value; set => Value = Convert.ToInt64(value); }
    }
}