using CrudeObservatory.DataSources.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.DataSources.Implementations.SineWave.Models
{
    internal class SineWaveDataSourceConfig : DataSourceConfigBase
    {
        public int PeriodSec { get; set; }
    }
}
