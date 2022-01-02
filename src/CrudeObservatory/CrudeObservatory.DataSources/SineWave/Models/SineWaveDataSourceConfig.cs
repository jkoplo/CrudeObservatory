using CrudeObservatory.DataSources.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.DataSources.SineWave.Models
{
    internal class SineWaveDataSourceConfig : IDataSourceConfig
    {
        public int PeriodSec { get; set; }
        public string Alias { get; set; }
    }
}
