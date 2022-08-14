using CrudeObservatory.Abstractions.Interfaces;

namespace CrudeObservatory.DataSources.SineWave.Models
{
    public class DataValue : IDataValue
    {
        public string Name { get; set; }
        public double Value { get; set; }
        object IDataValue.Value
        {
            get => Value;
            set => Value = Convert.ToDouble(value);
        }
    }
}
