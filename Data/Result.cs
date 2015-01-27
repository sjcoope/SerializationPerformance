namespace SJCNet.Samples.Performance.Serialization.Data
{
    public class Result
    {
        public long SerializedObjectSize { get; set; }
        public long SerializationTime { get; set; }
        public long DeserializationTime { get; set; }
        public int SampleSize { get; set; }
    }
}