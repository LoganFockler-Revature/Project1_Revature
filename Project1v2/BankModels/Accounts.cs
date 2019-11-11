using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankModels
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public String OwnerId { get; set; }

        public enum Types
        {
            Checking = 1,
            Business = 2,
            Loan = 3,
            TermDeposit = 4
        }

        public Types type { get; set; }

        public double Amount { get; set; }
    }
}

