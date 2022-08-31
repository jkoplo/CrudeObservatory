using CrudeObservatory.Abstractions.Interfaces;

namespace CrudeObservatory.DataSources.Libplctag
{
    internal class DataValue : IDataValue
    {
        public DataValue() { }

        public string Name { get; set; }
        public object Value { get; set; }
    }
}
