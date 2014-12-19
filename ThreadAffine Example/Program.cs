using System;
using System.ComponentModel;
using System.Threading;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;

namespace ThreadAffine_Example {
    internal class Program {
        private static void Main(string[] args) {
            var orderService = new OrderService();

            orderService.Process(1);

            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += (sender, arg) => {
                                           try {
                                               orderService.Process(2);
                                           }
                                           catch (Exception ex) {
                                               Console.WriteLine(ex.ToString());
                                           }
                                       };
            backgroundWorker.RunWorkerAsync();

            Console.ReadLine();
        }
    }

    [ThreadAffine]
    public class OrderService {
        public void Process(int sequence) {
            Console.WriteLine("sequence {0}", sequence);
            Console.WriteLine("sleeping for 10s");

            Thread.Sleep(new TimeSpan(0, 0, 5));
        }
    }
}