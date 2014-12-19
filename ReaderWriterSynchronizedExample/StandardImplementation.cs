using System;
using System.Threading;

namespace ReaderWriterSynchronizedExample1 {
    internal class Order {
        private readonly ReaderWriterLockSlim orderLock = new ReaderWriterLockSlim();

        public decimal Amount { get; private set; }
        public decimal Discount { get; private set; }

        public decimal AmountAfterDiscount {
            get {
                orderLock.EnterReadLock();
                var result = Amount - Discount;
                orderLock.ExitReadLock();
                return result;
            }
        }

        public void Set(decimal amount, decimal discount) {
            if (amount < discount) {
                throw new InvalidOperationException();
            }

            orderLock.EnterWriteLock();
            Amount = amount;
            Discount = discount;
            orderLock.ExitWriteLock();
        }
    }
}