using Newtonsoft.Json;
using SJCNet.Samples.Performance.Serialization.Data;
using System.IO;

namespace SJCNet.Samples.Performance.Serialization.Testers
{
    public class JSONNetTester<T> : Tester<T>
    {
        private JsonSerializer _serializer;
        private StreamWriter _streamWriter;
        private StreamReader _streamReader;
        private JsonReader _jsonReader;

        public JSONNetTester(SampleDataSet<T> sampleDataSet)
            : base(sampleDataSet)
        {
            _serializer = new JsonSerializer();
        }

        public override string Name
        {
            get { return "JSON.NET"; }
        }

        protected override MemoryStream Serialize()
        {
            // Read the data
            var stream = new MemoryStream();
            _streamWriter = new StreamWriter(stream);

            _serializer.Serialize(_streamWriter, base.SampleDataSet.Data);

            _streamWriter.Flush();

            return stream;
        }

        protected override T Deserialize()
        {
            // Read the serialized result
            base.SerializedResult.Position = 0;
            _streamReader = new StreamReader(base.SerializedResult);
            _jsonReader = new JsonTextReader(_streamReader);

            var result = _serializer.Deserialize<T>(_jsonReader);

            return result;
        }

        public override void Dispose()
        {
            if (_streamWriter != null)
            {
                _streamWriter.Close();
                _streamWriter.Dispose();
            }

            if (_jsonReader != null)
            {
                _jsonReader.Close();
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
