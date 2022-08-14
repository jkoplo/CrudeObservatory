using CrudeObservatory.Abstractions.Interfaces;
using CrudeObservatory.DataSources.Libplctag.Models;
using libplctag;

namespace CrudeObservatory.DataSources.Libplctag
{
    public class LibPlcTagDataSource : IDataSource
    {
        private List<ITag> tagList;

        public LibPlcTagDataSource(LibplctagDataSourceConfig dataSourceConfig)
        {
            DataSourceConfig = dataSourceConfig ?? throw new ArgumentNullException(nameof(dataSourceConfig));
        }

        public LibplctagDataSourceConfig DataSourceConfig { get; }

        public async Task InitializeAsync(CancellationToken stoppingToken)
        {
            tagList = new List<ITag>();

            foreach (var item in DataSourceConfig.Tags)
            {
                tagList.Add(GetTag(DataSourceConfig, item));
            }

            await Task.WhenAll(tagList.Select(x => x.InitializeAsync()));
        }

        public async Task<IEnumerable<IDataValue>> ReadDataAsync(CancellationToken stoppingToken)
        {
            await Task.WhenAll(tagList.Select(x => x.ReadAsync()));

            var results = tagList.Select(x => new DataValue() { Name = x.Name, Value = x.Value });

            return results;
        }

        public Task ShutdownAsync(CancellationToken stoppingToken)
        {
            foreach (var item in tagList)
            {
                item.Dispose();
            }
            return Task.CompletedTask;
        }

        private ITag GetTag(LibplctagDataSourceConfig controllerConfig, TagConfig tagConfig)
        {
            var tag = TagTypeMap.GetDataSource(tagConfig.TagType);

            //Name is the full path to tag.
            tag.Name = tagConfig.Name;
            //Gateway is the IP Address of the PLC or communication module.
            tag.Gateway = DataSourceConfig.Gateway;
            //Path is the location in the control plane of the CPU. Almost always "1,0".
            tag.Path = DataSourceConfig.Path;
            tag.PlcType = DataSourceConfig.PlcType;
            tag.Protocol = DataSourceConfig.Protocol;
            tag.Timeout = TimeSpan.FromSeconds(DataSourceConfig.TimeoutSeconds);

            return tag;
        }
    }
}
