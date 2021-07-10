using System;
using System.Collections.Generic;
using System.Text;

namespace RestCustomerConsumer
{
    class Customer
    {
        private static int nextId;

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int YearOfReg { get; set; }

        public Customer(string first, string last, int year)
        {
            ID = nextId++;
            FirstName = first;
            LastName = last;
            YearOfReg = year;
        }
        public override string ToString()
        {
            return $"Name: {FirstName} {LastName}, Id: {ID}, Year of registration: {YearOfReg}";
        }

        public Customer() { ID = nextId++; }
    }
}
