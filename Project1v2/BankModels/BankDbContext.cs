using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankModels
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> context)
            : base(context)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<BusinessAccount> BusinessAccounts { get; set; }
        public DbSet<LoanAccount> LoanAccounts { get; set; }

        public DbSet<TermDepositAccount> TermDepositAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }


}
