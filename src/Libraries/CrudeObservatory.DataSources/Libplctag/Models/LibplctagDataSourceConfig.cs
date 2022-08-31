using CrudeObservatory.Abstractions.Interfaces;
using libplctag;

namespace CrudeObservatory.DataSources.Libplctag.Models
{
    public class LibplctagDataSourceConfig : IDataSourceConfig
    {
        public string Gateway { get; set; }
        public string Path { get; set; }
        public Protocol Protocol { get; set; }
        public PlcType PlcType { get; set; }
        public float TimeoutSeconds { get; set; }

        public List<TagConfig> Tags { get; set; }
    }
}
