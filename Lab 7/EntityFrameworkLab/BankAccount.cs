using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkLab
{
    public class BankAccount
    {
        [Key]
        public int AccountIdentifier { get; set; }
        public decimal Balance { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public BankAccount() { }
    }
}
