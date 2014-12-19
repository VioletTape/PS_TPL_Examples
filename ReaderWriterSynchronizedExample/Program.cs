using System;
using System.Collections.Generic;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;

namespace ReaderWriterSynchronizedExample {
    internal class Program {
        private static void Main(string[] args) {}
    }

    [ReaderWriterSynchronized]
    internal class Order {
        private decimal Amount { get; set; }
        private decimal Discount { get; set; }
        [Child]
        private AdvisableCollection<Item> lines = new AdvisableCollection<Item>(); 

        public decimal AmountAfterDiscount {
            get { return Amount - Discount; }
        }

        [Writer]
        public void Set(decimal amount, decimal discount) {
            if (amount < discount) {
                throw new InvalidOperationException();
            }

            Amount = amount;
            Discount = discount;
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

    internal class Item {
        public decimal Amount  { get; set; }
    }
}