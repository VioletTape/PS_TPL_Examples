using System.Collections.Generic;

namespace ImmutableExample {
    public class Invoice1 {
        public readonly long _id;

        public Invoice1(long id) {
            SetIdentifier(id);
        }

        public void SetIdentifier(long id) {
//            _id = id;
        }
    }


    public class Invoice2 {
        public readonly Customer _customer;

        public Invoice2() {
            _customer = new Customer();
        }

        public void Refresh() {
            //valid but not immutable
            _customer.Name = "Jim";
            _customer.Phone = "555-123-9876";
        }
    }








    public class Invoice3 {
        public readonly IList<ItemX> _items;

        public Invoice3() {
            _items = new List<ItemX>();
        }

        public void Refresh() {
            //will cause a compilation error
//            _items = new List<Item>();

            //valid but not immutable
            _items.Add(new ItemX());
            _items[0].Price = 3.50;
            _items.RemoveAt(0);
        }
    }

    













    public class Customer {
        public string Name { get; set; }
        public string Phone { get; set; }
    }

    public class ItemX {
        public double Price { get; set; }
    }
}