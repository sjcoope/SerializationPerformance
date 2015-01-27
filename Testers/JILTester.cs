using Jil;
using System.IO;

namespace SJCNet.Samples.Performance.Serialization.Testers
{
    public class JILTester<T> : Tester<T>
    {
        private StreamWriter _streamWriter;
        private StreamReader _streamReader;

        public JILTester(Data.SampleDataSet<T> sampleDataSet) : base(sampleDataSet)
        { }

        public override string Name
        {
            get { return "JIL tester"; }
        }

        protected override MemoryStream Serialize()
        {
            // Read the data
            var stream = new MemoryStream();
            _streamWriter = new StreamWriter(stream);

            JSON.Serialize<T>(base.SampleDataSet.Data, _streamWriter);

            _streamWriter.Flush();

            return stream;
        }

        protected override T Deserialize()
        {
            // Read the serialized result
            base.SerializedResult.Position = 0;
            _streamReader = new StreamReader(base.SerializedResult);

            var result = JSON.Deserialize<T>(_streamReader);

            return result;
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
