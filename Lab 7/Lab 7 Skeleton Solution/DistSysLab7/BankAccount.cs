using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DistSysLab7
{
    public class BankAccount
    {
        [Key]
        public int AccountIdentifier { get; set; }
        public decimal Balance { get; set; }
        public BankAccount() { }
    }
}
