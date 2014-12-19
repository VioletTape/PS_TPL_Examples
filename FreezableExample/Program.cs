using System;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;

namespace FreezableExample {
    internal class Program {
        private static void Main(string[] args) {
            var invoice = new Invoice();
            invoice.Lines.Add(new InvoiceLine());
            Console.WriteLine("New invoice line added");
            invoice.Customer = new Customer {Name = "Smith"};
            Console.WriteLine("New customer added");

            ((IFreezable) invoice).Freeze();
            Console.WriteLine(">> Invoice freezed");

            invoice.Customer.Name = "Johnes";
            Console.WriteLine("Customer name changed");

            Console.ReadLine();
        }
    }

    [Freezable]
    public class Invoice {
        [Child]
        public readonly AdvisableCollection<InvoiceLine> Lines = new AdvisableCollection<InvoiceLine>();

        [Reference]
        public Customer Customer;
    }

    [Freezable]
    public class InvoiceLine {
        [Reference]
        public Product Product;

        [Parent]
        public Invoice ParentInvoice { get; private set; }
    }

    public class Product {}

    public class Customer {
        public string Name { get; set; }
    }
}