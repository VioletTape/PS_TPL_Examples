using System;
using System.Threading.Tasks;
using PostSharp.Patterns.Threading;

namespace ActorExample {
    internal class Program {
        public Program() {}

        private static void Main(string[] args) {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync() {
            var averageCalculator = new AverageCalculator();

            var observer = new SampleObserver(averageCalculator);
            DataSources.Source1.Subscribe(observer);
            DataSources.Source2.Subscribe(observer);
            DataSources.Source3.Subscribe(observer);
            DataSources.Source4.Subscribe(observer);

            Console.WriteLine("started");

            Console.ReadKey();
            var average = await averageCalculator.GetAverage();

            Console.WriteLine("Average: {0}", average);
            Console.ReadLine();
        }
    }

    public static class DataSources {
        public static DataSource Source1 = new DataSource();
        public static DataSource Source2 = new DataSource();
        public static DataSource Source3 = new DataSource();
        public static DataSource Source4 = new DataSource();
    }

    public class DataSource {
        public void Subscribe(SampleObserver observer) {
            new Task(() => Foo(observer), TaskCreationOptions.AttachedToParent).Start();
        }

        private void Foo(SampleObserver observer) {
            for (int i = 1; i < 1000; i++) {
                observer.OnNext(i);
            }
        }
    }

    [Actor]
    public class AverageCalculator {
        private float sum;
        private int count;

        public void AddSample(float n) {
            count++;
            sum += n;
        }

        [Reentrant]
        public async Task<float> GetAverage() {
            return sum / count;
        }
    }

    public class SampleObserver : IObserver<float> {
        private AverageCalculator calculator;

        public SampleObserver(AverageCalculator averageCalculator) {
            calculator = averageCalculator;
        }


        public void OnNext(float value) {
            // Each of the data sources can call us from a different thread and concurrently.
            // But we don't have to care since our calculator will enqueue method calls.
            calculator.AddSample(value);
        }

        public void OnError(Exception error) {}

        public void OnCompleted() {}

    }
}