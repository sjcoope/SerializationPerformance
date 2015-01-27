using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJCNet.Samples.Performance.Serialization.Data
{
    public class ResultGroups : List<ResultGroup>
    {
        public ResultGroup GetByTesterType(Type testerType)
        {
            var match = this.SingleOrDefault(i => i.SerializerType == testerType);
            if (match == null)
            {
                throw new KeyNotFoundException(String.Format("Cannot find result group for tester with type of {0}", testerType.ToString()));
            }

            return match;
        }
    }
}
