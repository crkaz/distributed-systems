using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkLab
{
    public class Person
    {
        public int PersonID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public DateTime Date_of_Birth { get; set; }
        public virtual Address Address { get; set; } // virtual keyword for lazy loading
        public string Middle_Names { get; set; }
        public virtual BankAccount BankAccount { get; set; } // virtual keyword for lazy loading
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Person() { }
    }
}
