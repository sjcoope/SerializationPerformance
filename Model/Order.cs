using ProtoBuf;
using System.Collections.Generic;

namespace SJCNet.Samples.Performance.Serialization.Model
{
    [ProtoContract]
    public class Order
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public List<Product> Products { get; set; }
    }
}
