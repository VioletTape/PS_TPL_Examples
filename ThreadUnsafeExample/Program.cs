using System.Threading.Tasks;
using PostSharp.Patterns.Threading;

namespace ThreadUnsafeExample {
    internal class Program {
        private static void Main(string[] args) {
            var averageCalculator = new AverageCalculator();

            Task.Factory.StartNew(() => Calc(averageCalculator));
            Task.Factory.StartNew(() => Calc(averageCalculator));
        }

        private static void Calc(AverageCalculator calculator) {
            for (var i = 0; i < 10; i++) {
                calculator.AddSample(i);
            }
        }
    }

    [ThreadUnsafe]
    internal class AverageCalculator {
        private float sum;
        private int count;

        public void AddSample(float n) {
            count++;
            sum += n;
        }

        public float GetAverage() {
            return sum / count;
        }
    }
}