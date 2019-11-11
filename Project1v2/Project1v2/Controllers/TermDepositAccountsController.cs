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
    public class TermDepositAccountsController : Controller
    {
        private readonly BankDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public TermDepositAccountsController(BankDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TermDepositAccounts
        public async Task<IActionResult> Index()
        {
            return View(await _context.TermDepositAccounts.ToListAsync());
        }

        // GET: TermDepositAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termDepositAccount = await _context.TermDepositAccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (termDepositAccount == null)
            {
                return NotFound();
            }

            return View(termDepositAccount);
        }

        // GET: TermDepositAccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TermDepositAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Maturity,Id,OwnerId,type,Amount")] TermDepositAccount termDepositAccount)
        {
            termDepositAccount.type = Account.Types.TermDeposit;
            termDepositAccount.OwnerId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                _context.Add(termDepositAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(termDepositAccount);
        }

        public async Task<IActionResult> Withdraw(int? id)
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
        public async Task<IActionResult> Withdraw(int? id, double withdraw)
        {

            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account.Amount - withdraw >= 0)
            {
                account.Amount = account.Amount - withdraw;
            }

            try
            {
                _context.Update(account);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TermDepositAccountExists(account.Id))
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

        // GET: TermDepositAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termDepositAccount = await _context.TermDepositAccounts.FindAsync(id);
            if (termDepositAccount == null)
            {
                return NotFound();
            }
            return View(termDepositAccount);
        }

        // POST: TermDepositAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Maturity,Id,OwnerId,type,Amount")] TermDepositAccount termDepositAccount)
        {
            if (id != termDepositAccount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(termDepositAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TermDepositAccountExists(termDepositAccount.Id))
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
            return View(termDepositAccount);
        }

        // GET: TermDepositAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termDepositAccount = await _context.TermDepositAccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (termDepositAccount == null)
            {
                return NotFound();
            }

            return View(termDepositAccount);
        }

        // POST: TermDepositAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var termDepositAccount = await _context.TermDepositAccounts.FindAsync(id);
            _context.TermDepositAccounts.Remove(termDepositAccount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TermDepositAccountExists(int id)
        {
            return _context.TermDepositAccounts.Any(e => e.Id == id);
        }
    }
}
