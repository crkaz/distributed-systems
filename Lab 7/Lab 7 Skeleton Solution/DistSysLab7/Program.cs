using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DistSysLab7
{
    class Program
    {
        static void Main()
        {
            using (var ctx = new Lab7Context())
            {
                Address addr = new Address() { House_Name_or_Number = "1076", Street = "Some Street", City = "Some City", County = "Some County", Country = "UK", Postcode = "Some Postcode", People = new List<Person>() };
                BankAccount accnt = new BankAccount() { Balance = 50.0m };
                Person prsn = new Person() { First_Name = "Jane", Last_Name = "Doe", Date_of_Birth = new DateTime(2010, 10, 1), Age = 40, Address = addr, BankAccount = accnt };

                ctx.Addresses.Add(addr);
                ctx.BankAccounts.Add(accnt);
                ctx.People.Add(prsn);

                ctx.SaveChanges();
            }
        }
    }
}
