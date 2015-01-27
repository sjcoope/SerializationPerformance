using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJCNet.Samples.Performance.Serialization.Data
{
    public class SampleDataSet<T>
    {
        public int Size { get; set; }
        public T Data { get; set; }
    }
}
