using FinApp.DTO;
using FinApp.Models;
using FinApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.Controllers {
    [Authorize]
    public class BankAccountController : Controller {
        private BankAccountService bankAccountService;
        private UserManager<AppUser> userManager;
        public BankAccountController(BankAccountService bankAccountService, UserManager<AppUser> userManager) {
            this.bankAccountService = bankAccountService;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Create() {
         
            return View(new BankAccountDTO());
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(BankAccountDTO newBankAccount) {
            var user = await userManager.GetUserAsync(User);
            if (user == null) {
                return RedirectToAction("Login", "Account");
            }

            newBankAccount.UserId = user.Id;
            
            await bankAccountService.CreateAsync(newBankAccount);
            return RedirectToAction("Index", "BudgetPlannerVM");
        }
        [HttpGet]
        public async Task<IActionResult> EditAsync(int id) {
            var bankAccount = await bankAccountService.getByIdAsync(id);
            if (bankAccount == null) {
                return NotFound();
            }
            return View(bankAccount);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(int id, BankAccountDTO bankAccount) {
            var user = await userManager.GetUserAsync(User);
            if (user == null) {
                return RedirectToAction("Login", "Account");
            }
            bankAccount.UserId = user.Id;
            await bankAccountService.UpdateAsync(bankAccount);
            return RedirectToAction("Index", "BudgetPlannerVM");
        }
        [HttpPost] // delete action
        public async Task<IActionResult> DeleteAsync(int id) {
            var bankAccountToDelete = await bankAccountService.getByIdAsync(id);
            if (bankAccountToDelete == null) {
                return View("NotFound");
            }
            await bankAccountService.DeleteAsync(id);
            return RedirectToAction("Index", "BudgetPlannerVM");
        }
    }
}
