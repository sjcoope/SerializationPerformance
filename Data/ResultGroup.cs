using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJCNet.Samples.Performance.Serialization.Data
{
    public class ResultGroup
    {
        public ResultGroup()
        {
            this.Results = new List<Result>();
            this.CalculatedResults = new List<Result>();
        }

        public Type SerializerType { get; internal set; }
        public List<Result> Results { get; private set; }
        public List<Result> CalculatedResults { get; set; }
    }
}
