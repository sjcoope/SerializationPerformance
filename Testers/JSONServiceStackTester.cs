using ServiceStack.Text;
using SJCNet.Samples.Performance.Serialization.Data;
using System.IO;

namespace SJCNet.Samples.Performance.Serialization.Testers
{
    public class JsonServiceStackTester<T> : Tester<T>
    {
        private readonly TypeSerializer<T> _serializer;
        private StreamReader _streamReader;
        private StreamWriter _streamWriter;

        public JsonServiceStackTester(SampleDataSet<T> sampleDataSet)
            : base(sampleDataSet)
        {
            _serializer = new TypeSerializer<T>();
        }

        public override string Name
        {
            get { return "JSON Service Stack"; }
        }

        protected override MemoryStream Serialize()
        {
            var stream = new MemoryStream();
            _streamWriter = new StreamWriter(stream);
            _serializer.SerializeToWriter(base.SampleDataSet.Data, _streamWriter);
            _streamWriter.Flush();

            return stream;
        }

        protected override T Deserialize()
        {
            base.SerializedResult.Position = 0;
            _streamReader = new StreamReader(base.SerializedResult);
            return _serializer.DeserializeFromReader(_streamReader);
        }

        public override void Dispose()
        {
            if (_streamWriter != null)
            {
                _streamWriter.Close();
                _streamWriter.Dispose();
            }

            if (_streamReader != null)
            {
                _streamReader.Close();
                _streamReader.Dispose();
            }

            base.Dispose();
        }
    }
}
