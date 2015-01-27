using SJCNet.Samples.Performance.Serialization.Data;
using System.IO;

namespace SJCNet.Samples.Performance.Serialization.Testers
{
    public class ProtobufTester<T> : Tester<T>
    {
        public ProtobufTester(SampleDataSet<T> sampleDataSet)
            : base(sampleDataSet)
        { }

        public override string Name
        {
            get { return "ProtoBuf"; }
        }

        protected override MemoryStream Serialize()
        {
            var stream = new MemoryStream();
            ProtoBuf.Serializer.Serialize(stream, base.SampleDataSet.Data);
            return stream;
        }

        protected override T Deserialize()
        {
            base.SerializedResult.Position = 0;
            return ProtoBuf.Serializer.Deserialize<T>(base.SerializedResult);
        }
    }
}
