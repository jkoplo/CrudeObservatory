using CrudeObservatory.DataSources.Abstractions.Interfaces;
using CrudeObservatory.DataTargets.Abstractions.Interfaces;
using CrudeObservatory.Intervals.Abstractions.Interfaces;
using CrudeObservatory.Triggers.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.Acquisition.Models
{
    internal class AcquisitionSet
    {
        public string Name { get; set; }
        public ITrigger StartTrigger { get; set; }
        public ITrigger EndTrigger { get; set; }


        public IInterval Interval { get; set; }
        public List<IDataSource> DataSources { get; set; }
        public IDataTarget DataTarget { get; set; }

    }
}
