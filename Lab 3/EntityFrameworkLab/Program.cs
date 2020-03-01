using System;
using System.Collections.Generic;

namespace EntityFrameworkLab
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (var ctx = new MyContext())
            {
                Address addr = new Address() { House_Name_or_Number = "hnameornum", Street = "st", City = "donc", County = "south yorkshire", Postcode = "dn59qe", People = new List<Person>(), Country = "UK" };
                Person prsn = new Person() { First_Name = "Zak", Middle_Names = "Reece", Last_Name = "Zak", Date_of_Birth = new DateTime(1994, 11, 6), Address = addr };

                ctx.Addresses.Add(addr);
                ctx.People.Add(prsn);
                ctx.SaveChanges();
            }
        }
    }
}
