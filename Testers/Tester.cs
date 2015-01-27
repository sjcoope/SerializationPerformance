using SJCNet.Samples.Performance.Serialization.Data;
using System;
using System.Diagnostics;
using System.IO;

namespace SJCNet.Samples.Performance.Serialization.Testers
{
    public abstract class Tester<T> : IDisposable
    {
        public Tester(SampleDataSet<T> sampleDataSet)
        {
            this.SampleDataSet = sampleDataSet;
        }

        #region Properties

        public abstract string Name { get; }

        public SampleDataSet<T> SampleDataSet { get; private set; }

        protected MemoryStream SerializedResult { get; set; }

        protected long SerializationTime { get; set; }

        public long SerializedObjectSize { get; private set; }

        protected T DeserializedResult { get; set; }

        protected long DeserializationTime { get; set; }

        #endregion

        #region Methods

        protected abstract MemoryStream Serialize();

        protected abstract T Deserialize();

        public void Test()
        {
            this.SerializedResult = new MemoryStream();

            // Test Serialization
            var serializationWatch = Stopwatch.StartNew();
            this.SerializedResult = this.Serialize();
            serializationWatch.Stop();
            this.SerializationTime = serializationWatch.ElapsedMilliseconds;

            // Test Deserialization
            var deserializationWatch = Stopwatch.StartNew();
            this.DeserializedResult = this.Deserialize();
            deserializationWatch.Stop();
            this.DeserializationTime = deserializationWatch.ElapsedMilliseconds;
        }

        public Result GetResult()
        {
            return new Result
            {
                SampleSize = this.SampleDataSet.Size,
                DeserializationTime = this.DeserializationTime,
                SerializationTime = this.SerializationTime,
                SerializedObjectSize = this.SerializedResult.Length
            };
        }

        public virtual void Dispose()
        {
            this.SampleDataSet = null;
            if (this.SerializedResult != null)
            {
                this.SerializedResult.Dispose();
            }
        }

        #endregion
    }
}