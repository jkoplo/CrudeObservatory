using CrudeObservatory.Acquisition.Models;
using CrudeObservatory.DataSources.Abstractions.Interfaces;
using CrudeObservatory.DataSources.Implementations.Libplctag.Models;
using libplctag;
using libplctag.DataTypes.Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.DataSources.Implementations.Libplctag
{
    internal class LibPlcTagDataSource : IDataSource
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
                var tag = new TagDint()
                {
                    //Name is the full path to tag. 
                    Name = item,
                    //Gateway is the IP Address of the PLC or communication module.
                    Gateway = DataSourceConfig.Gateway,
                    //Path is the location in the control plane of the CPU. Almost always "1,0".
                    Path = DataSourceConfig.Path,
                    PlcType = DataSourceConfig.PlcType,
                    Protocol = DataSourceConfig.Protocol,
                    Timeout = TimeSpan.FromSeconds(DataSourceConfig.TimeoutSeconds),
                };
                tagList.Add(tag);

            }

            await Task.WhenAll(tagList.Select(x => x.InitializeAsync()));
        }

        public async Task<IEnumerable<DataValue>> ReadDataAsync(CancellationToken stoppingToken)
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
    }
}
