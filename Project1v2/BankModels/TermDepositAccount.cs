using System;
using System.Collections.Generic;
using System.Text;

namespace BankModels
{
    public class TermDepositAccount : Account
    {
        public DateTime Maturity { get; set; }
    }
}
