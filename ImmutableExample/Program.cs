using System;
using System.Linq;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;

namespace ImmutableExample {
    internal class Program {
        private static void Main(string[] args) {
            var invoice = new Invoice(123);
    
            Console.WriteLine(invoice.Items.First().Name);

            Console.ReadLine();
        }
    }

    [Immutable]
    public class Invoice  {

        public long Id { get; set; }
        public Invoice(long id)  {
            Id = id;
            Items = new AdvisableCollection<Item>();
            Items.Add(new Item("widget"));
        }

        [Child]
        public AdvisableCollection<Item> Items { get; set; }
    }

    [Immutable]
    public class Document {
        private long _id;

        public Document(long id) {
            _id = id;
        }
    }

    [Immutable]
    public class Item {
        public Item(string name) {
            Name = name;
        }

        public string Name { get; set; }
    }
}