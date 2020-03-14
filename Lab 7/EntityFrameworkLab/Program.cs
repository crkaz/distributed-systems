using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityFrameworkLab
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // MANAGE OPTIMISTIC CONCURRENCY
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    using (var ctx = new MyContext())
                    {
                        Person prsn = ctx.People.First();
                        decimal balance = prsn.BankAccount.Balance;
                        decimal balancechange;
                        do
                        {
                            Console.WriteLine("Enter a balance modifier");
                        }
                        while (!decimal.TryParse(Console.ReadLine(), out balancechange));
                        balance += balancechange;
                        prsn.BankAccount.Balance = balance;
                        ctx.SaveChanges();
                        return;
                    }
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
                {
                    Console.WriteLine("Oh no! Looks like the database was modified whilst you were making your change.Try again.");
                }
            }
            Console.WriteLine("Data access failed three times - perhaps try again later.");

            //using (var ctx = new MyContext())
            //{
            //    Person prsn = ctx.People.First();
            //    /// NO longer needed because downloaded lazyloading proxies from nuget and enabled in context.
            //    //ctx.Entry(prsn).Reference("BankAccount").Load(); // Explicitly load else null (because lazy loading is off by default).
            //    /// In this case, loading prsn also loads an address which is uncessessary for the operation, so lazyLoad not always best option.
            //    decimal balance = prsn.BankAccount.Balance;

            //    decimal balancechange = 0;
            //    do
            //    {
            //        Console.WriteLine("Enter a balance modifier");
            //    }
            //    while (!decimal.TryParse(Console.ReadLine(), out balancechange));
            //    prsn.BankAccount.Balance += balancechange;
            //    ctx.SaveChanges();
            //}
        }
    }
}
