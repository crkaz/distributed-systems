using System;
using System.Collections.Generic;

namespace EntityFrameworkLab
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new MyContext())
            {
                Address address = new Address()
                {
                    House_Name_or_Number = "1076",
                    Street = "Some Street",
                    City = "Some City",
                    Postcode = "Some Postcode",
                    County = "some County",
                    People = new List<Person>()
                };
                Person person = new Person()
                {
                    First_Name = "Jane",
                    Middle_Name = "Janet",
                    Last_Name = "Doe",
                    Date_of_Birth = new DateTime(2010, 10, 1),
                    Address = address
                };
                ctx.Addresses.Add(address);
                ctx.People.Add(person);
                ctx.SaveChanges();
            }
        }
    }
}
