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
    public class BusinessAccountsController : Controller
    {
        private readonly BankDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public BusinessAccountsController(BankDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: BusinessAccounts
        public async Task<IActionResult> Index()
        {
            return View(await _context.BusinessAccounts.ToListAsync());
        }

        // GET: BusinessAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessAccount = await _context.BusinessAccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (businessAccount == null)
            {
                return NotFound();
            }

            return View(businessAccount);
        }

        // GET: BusinessAccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BusinessAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Overdraft,OverdraftCost,OverdraftDueDate,Id,OwnerId,type,Amount")] BusinessAccount businessAccount)
        {
            businessAccount.type = Account.Types.Business;
            businessAccount.OwnerId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                _context.Add(businessAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(businessAccount);
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

            var account = await _context.BusinessAccounts.FindAsync(id);
            if(account.Overdraft == 0)
            {
                account.Amount = account.Amount - withdraw;
            }
            if (account.Amount < 0)
            {
                DateTime today = DateTime.Today;
                account.Overdraft = Math.Abs(account.Amount);
                account.Amount = 0;
                account.OverdraftCost = account.Overdraft * .08 + account.Overdraft;
                account.OverdraftDueDate = 30;
            }

            try
            {
                _context.Update(account);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessAccountExists(account.Id))
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

            var account = await _context.BusinessAccounts.FindAsync(id);

            if (account.OverdraftCost == 0)
            {
                account.Amount = account.Amount + deposit;
            }
            if (account.OverdraftCost > 0)
            {
                account.OverdraftCost = account.OverdraftCost - deposit;
            }
            if(account.OverdraftCost < 0)
            {
                account.Amount = Math.Abs(account.OverdraftCost);
                account.OverdraftCost = 0;
                account.Overdraft = 0;
                account.OverdraftDueDate = 0;

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

        // GET: BusinessAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessAccount = await _context.BusinessAccounts.FindAsync(id);
            if (businessAccount == null)
            {
                return NotFound();
            }
            return View(businessAccount);
        }

        // POST: BusinessAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Overdraft,OverdraftCost,OverdraftDueDate,Id,OwnerId,type,Amount")] BusinessAccount businessAccount)
        {
            if (id != businessAccount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(businessAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusinessAccountExists(businessAccount.Id))
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
            return View(businessAccount);
        }

        // GET: BusinessAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessAccount = await _context.BusinessAccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (businessAccount == null)
            {
                return NotFound();
            }

            return View(businessAccount);
        }

        // POST: BusinessAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var businessAccount = await _context.BusinessAccounts.FindAsync(id);
            _context.BusinessAccounts.Remove(businessAccount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BusinessAccountExists(int id)
        {
            return _context.BusinessAccounts.Any(e => e.Id == id);
        }
    }
}
