using System;
using System.Threading.Tasks;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;

namespace ReaderWriterSynchronizedExample {
    internal class Program {
        private static void Main(string[] args) {
            var order = new Order();

            var t1 = new Task(() => {
                                  var random = new Random();
                                  for (var i = 0; i < 1000; i++) {
                                      order.Set(random.Next(100), random.Next(100));
                                  }
                              });
            var t2 = new Task(() => {
                                  var random = new Random();

                                  for (var i = 0; i < 1000; i++) {
                                      order.Set(random.Next(100), random.Next(100));
                                  }
                              });
            var task = new Task(() => {
                                    while (true) {
                                        var amountAfterDiscount = order.AmountAfterDiscount;
                                        if (amountAfterDiscount < 0) {
                                            Console.WriteLine(amountAfterDiscount);
                                        }
                                    }
                                });
            t1.Start();
            t2.Start();
            task.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine("End");
            Console.ReadLine();
        }
    }

    [ReaderWriterSynchronized]
    public class Order {
        private decimal Amount { get; set; }
        private decimal Discount { get; set; }

        [Child]
        private readonly AdvisableCollection<Item> lines = new AdvisableCollection<Item>();

        public decimal AmountAfterDiscount {
            get { return Amount - Discount; }
        }

        [Writer]
        public void Set(decimal amount, decimal discount) {
            if (amount < discount) {
                return;
            }

            Amount = amount;
            Discount = discount;
            lines.Add(new Item {Amount = amount});
        }

        [UpgradeableReader]
        public void Recalculate() {
            decimal total = 0;

            for (var i = 0; i < lines.Count; ++i) {
                total += lines[i].Amount;
            }

            Amount = total;
        }
    }

    [ReaderWriterSynchronized]
    public class Item {
        public decimal Amount { get; set; }
    }
}