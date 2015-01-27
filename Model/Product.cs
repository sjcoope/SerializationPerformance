using ProtoBuf;

namespace SJCNet.Samples.Performance.Serialization.Model
{
    [ProtoContract]
    public class Product
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public string Description { get; set; }

        [ProtoMember(4)]
        public int Price { get; set; }
    }
}
