using System;
using System.Collections.Generic;
using System.Text;

namespace BankModels
{
    public class BusinessAccount : Account
    {
        static readonly double InterestRate = .20;
        public double Overdraft { get; set; }
        public double OverdraftCost { get; set; }
        public double OverdraftDueDate { get; set; }
    }
}
