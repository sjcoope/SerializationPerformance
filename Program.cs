using SJCNet.Samples.Performance.Serialization.Data;
using SJCNet.Samples.Performance.Serialization.Model;
using SJCNet.Samples.Performance.Serialization.Testers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SJCNet.Samples.Performance.Serialization
{
    class Program
    {
        private const string _dateTimeFormat = "dd/mm/yyyy H:mm:ss";
        private const int _testsToPerformPerInstance = 10;

        private static readonly int[] _sampleSizes = { 100, 1000, 10000 };
        private static readonly Type[] _testerTypes = {
            typeof(ProtobufTester<Orders>),
            typeof(DataContractJsonTester<Orders>),
            typeof(JSONNetTester<Orders>),
            typeof(JsonServiceStackTester<Orders>),
            typeof(JILTester<Orders>),
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Started: " + DateTime.Now.ToString(_dateTimeFormat));

            var sampleDatasets = CreateGroupSampleData();

            var resultGroups = CreateResultGroups();

            PerformTests(sampleDatasets, resultGroups);

            CalculateResults(resultGroups);

            OutputResults(resultGroups);

            Console.WriteLine("");
            Console.WriteLine("Ended: " + DateTime.Now.ToString(_dateTimeFormat));
            Console.Read();
            Console.WriteLine("Press any key to close the window...");
        }

        private static SampleDataSets<Orders> CreateGroupSampleData()
        {
            var sampleDatasets = new SampleDataSets<Orders>();
            var dataGenerator = new DataGenerator();

            foreach (var sampleSize in _sampleSizes)
            {
                Console.WriteLine("Generating Sample of {0} Records...", sampleSize);
                sampleDatasets.Add(new SampleDataSet<Orders> { Size = sampleSize, Data = dataGenerator.GenerateTestOrders(sampleSize, 20) });
            }
            Console.WriteLine("");

            return sampleDatasets;
        }

        private static ResultGroups CreateResultGroups()
        {
            var results = new ResultGroups();
            foreach (var testerType in _testerTypes)
            {
                results.Add(new ResultGroup { SerializerType = testerType });
            }
            return results;
        }

        private static void PerformTests(SampleDataSets<Orders> sampleDatasets, ResultGroups resultGroups)
        {
            foreach (var testerType in _testerTypes)
            {
                foreach (var sampleSize in _sampleSizes)
                {
                    // Get the sample data
                    var sampleDataset = sampleDatasets.FirstOrDefault(x => x.Size == sampleSize);
                    if (sampleDataset != null)
                    {
                        // Get the result group to add the test results to
                        var resultGroup = resultGroups.GetByTesterType(testerType);

                        // Perform number of tests required and get the results
                        PerformTestByTesterAndSample(testerType, sampleDataset, resultGroup, _testsToPerformPerInstance);
                    }
                }
            }
        }

        private static void PerformTestByTesterAndSample(Type testerType, SampleDataSet<Orders> sampleDataset, ResultGroup resultGroup, int testsToPerform)
        {
            for (var i = 1; i <= testsToPerform; i++)
            {
                // Create the new tester
                var ctor = testerType.GetConstructor(new[] { typeof(SampleDataSet<Orders>) });
                var tester = (Tester<Orders>)ctor.Invoke(new object[] { sampleDataset });

                // Run test
                Console.WriteLine(String.Format("Testing {0} [Sample Size: {1}]...", tester.Name, tester.SampleDataSet.Size));
                tester.Test();

                // Collect results
                resultGroup.Results.Add(tester.GetResult());

                // Clean up any unused/unreferenced resources.
                tester.Dispose();
                GC.Collect();
            }
        }

        private static void CalculateResults(ResultGroups resultGroups)
        {
            // Calculate the results for each tester and sample size
            foreach(var testerType in _testerTypes)
            {
                // Get the tester results
                var testerResults = resultGroups.SingleOrDefault(i => i.SerializerType == testerType);
                if(testerResults == null)
                {
                    throw new NullReferenceException(String.Format("Cannot find tester results for type {0}", testerType.ToString()));
                }

                foreach (var sampleSize in _sampleSizes)
                {
                    // Get the sample results
                    var sampleResults = testerResults.Results.Where(i => i.SampleSize == sampleSize);
                    if(sampleResults.Count() == 0)
                    {
                        throw new KeyNotFoundException(String.Format("Cannot find tester results for sample size of {0}", sampleSize));
                    }

                    var calculatedResult = CalculateAverageOfResults(sampleResults);
                    calculatedResult.SampleSize = sampleSize;
                    testerResults.CalculatedResults.Add(calculatedResult);
                }
            }
        }

        private static Result CalculateAverageOfResults(IEnumerable<Result> results)
        {
            long totalSerializationTime = 0, totalDeserializationTime = 0, totalSerializedObjectSize = 0;
            foreach(var result in results)
            {
                totalSerializationTime += result.SerializationTime;
                totalDeserializationTime += result.DeserializationTime;
                totalSerializedObjectSize += result.SerializedObjectSize;
            }

            var resultCount = Convert.ToInt64(results.Count());
            return new Result
            {
                SerializationTime = (totalSerializationTime / resultCount),
                DeserializationTime = (totalDeserializationTime / resultCount),
                SerializedObjectSize = (totalSerializedObjectSize / resultCount)
            };
        }

        private static void OutputResults(ResultGroups resultGroups)
        {
            var consoleFormatString = "|{0,-30}|{1,10}|{2,10}|{3,10}|{4,10}|";
            Console.WriteLine("");
            Console.WriteLine(String.Format(consoleFormatString, "Serializer", "Sample", "Size", "Ser.", "Deser."));
            foreach (var resultGroup in resultGroups)
            {
                foreach (var calculatedResult in resultGroup.CalculatedResults.OrderBy(i => i.SampleSize))
                {
                    Console.WriteLine(String.Format(consoleFormatString, 
                        resultGroup.SerializerType.Name,
                        calculatedResult.SampleSize,
                        calculatedResult.SerializedObjectSize,
                        calculatedResult.SerializationTime, 
                        calculatedResult.DeserializationTime));
                }
            }
        }
    }
}