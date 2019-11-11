using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankModels;
using Microsoft.AspNetCore.Identity;
using Proj1.Models;

namespace Project1v2.Controllers
{
    public class LoanAccountsController : Controller
    {
        private readonly BankDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public LoanAccountsController(BankDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: LoanAccounts
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoanAccounts.ToListAsync());
        }

        // GET: LoanAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanAccount = await _context.LoanAccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loanAccount == null)
            {
                return NotFound();
            }

            return View(loanAccount);
        }

        // GET: LoanAccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LoanAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MonthlyDue,Id,OwnerId,type,Amount")] LoanAccount loanAccount)
        {
            loanAccount.type = Account.Types.Loan;
            loanAccount.OwnerId = _userManager.GetUserId(User);
            loanAccount.MonthlyDue = loanAccount.Amount / 24;
            if (ModelState.IsValid)
            {
                _context.Add(loanAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loanAccount);
        }

        public async Task<IActionResult> Deposit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(int? id, double deposit)
        {

            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.LoanAccounts.FindAsync(id);

            if(deposit > account.MonthlyDue)
            {
                account.Amount = account.Amount - deposit;
            }

            

            try
            {
                _context.Update(account);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(account.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

        }

        private bool AccountExists(int id)
        {
            throw new NotImplementedException();
        }

        // GET: LoanAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanAccount = await _context.LoanAccounts.FindAsync(id);
            if (loanAccount == null)
            {
                return NotFound();
            }
            return View(loanAccount);
        }

        // POST: LoanAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MonthlyDue,Id,OwnerId,type,Amount")] LoanAccount loanAccount)
        {
            if (id != loanAccount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loanAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanAccountExists(loanAccount.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(loanAccount);
        }

        // GET: LoanAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanAccount = await _context.LoanAccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loanAccount == null)
            {
                return NotFound();
            }

            return View(loanAccount);
        }

        // POST: LoanAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loanAccount = await _context.LoanAccounts.FindAsync(id);
            _context.LoanAccounts.Remove(loanAccount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanAccountExists(int id)
        {
            return _context.LoanAccounts.Any(e => e.Id == id);
        }
    }
}
