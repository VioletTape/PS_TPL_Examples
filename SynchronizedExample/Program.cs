using System;
using System.ComponentModel;
using System.Threading;
using PostSharp.Patterns.Threading;

namespace SynchronizedExample {
    internal class Program {
        private static void Main(string[] args) {
            var orderService = new OrderService();

            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += (sender, arg) => orderService.Process(1);
            backgroundWorker.RunWorkerAsync();

            orderService.Process(2);
        }
    }

    [Synchronized]
    public class OrderService {
        public void Process(int sequence) {
            Console.WriteLine("sequence {0}", sequence);
            Console.WriteLine("sleeping for 10s");

            Thread.Sleep(new TimeSpan(0, 0, 10));
        }
    }
}