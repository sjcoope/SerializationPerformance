using SJCNet.Samples.Performance.Serialization.Data;
using System.IO;
using System.Runtime.Serialization.Json;

namespace SJCNet.Samples.Performance.Serialization.Testers
{
    public class DataContractJsonTester<T> : Tester<T>
    {
        private readonly DataContractJsonSerializer _serializer;

        public DataContractJsonTester(SampleDataSet<T> sampleDataSet)
           : base(sampleDataSet)
        {
            _serializer = new DataContractJsonSerializer(typeof(T));
        }


        public override string Name
        {
            get { return "Data Contract JSON"; }
        }

        protected override MemoryStream Serialize()
        {
            var stream = new MemoryStream();
            _serializer.WriteObject(stream, base.SampleDataSet.Data);
            return stream;
        }

        protected override T Deserialize()
        {
            base.SerializedResult.Position = 0;
            return (T)_serializer.ReadObject(base.SerializedResult);
        }
    }
}
