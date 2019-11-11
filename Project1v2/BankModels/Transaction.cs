using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankModels
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        public enum Types
        {
            Create = 1,
            Delete = 2,
            Deposit = 3,
            Withdraw = 4,
            Transfer = 5,
            PayLoan = 6
        }
        public Types Type { get; set; }

        public double Amount { get; set; }

        public virtual ICollection<Account> AcountIds { get; set; }
    }
}
