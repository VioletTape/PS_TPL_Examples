using System.Runtime.InteropServices;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;

namespace FreezableExample {
    internal class Program {
        private static void Main(string[] args) {
            var invoice = new Invoice();
            ((IFreezable) invoice).Freeze();

        }
    }

    [Freezable]
    public class Invoice {
        public readonly AdvisableCollection<InvoiceLine> Lines = new AdvisableCollection<InvoiceLine>();

        public Customer Customer;
    }

    [Freezable]
    public class InvoiceLine {
        public Product Product;

        public Invoice ParentInvoice { get; private set; }
    }

    public class Product {}

    public class Customer {}
}