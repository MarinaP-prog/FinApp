using FinApp.Models;
using FinApp.Services;
using FinApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.Controllers {
    [Authorize]
    public class BudgetPlannerVMController : Controller {
        private BankAccountService bankAccountService;
        private MonthlyExpenseService monthlyExpenseService;
        private UserManager<AppUser> userManager;
        public BudgetPlannerVMController(BankAccountService bankAccountService, MonthlyExpenseService monthlyExpenseService, UserManager<AppUser> userManager) {
            this.bankAccountService = bankAccountService;
            this.monthlyExpenseService = monthlyExpenseService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync() {
            var user = await userManager.GetUserAsync(User);
            var bankAccounts = await bankAccountService.GetAllBankAccountsAsync(userId: user.Id);
            var monthlyExpenses = await monthlyExpenseService.GetAllMonthlyExpensesAsync(user.Id);
            if (user == null) {
                return RedirectToAction("Login", "Account");
            }
            var viewModel = new BudgetPlannerVM {
                BankAccounts = bankAccounts,
                MonthlyExpenses = monthlyExpenses,
                MonthlyExpensesBalance = monthlyExpenseService.GetBalance,
                BankAccountsBalance = bankAccountService.GetBalance,
            };

            return View(viewModel);
        }
    }
}
