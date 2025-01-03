using FinApp.Models;
using FinApp.Services;
using FinApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.Controllers {
    [Authorize]
    public class MontlyBudgetPlannerVMController : Controller {

        private BankAccountService bankAccountService;
        private CurrentMonthService currentMonthService;
        private UserManager<AppUser> userManager;

        public MontlyBudgetPlannerVMController(BankAccountService bankAccountService, CurrentMonthService currentMonthService, UserManager<AppUser> userManager) {
            this.bankAccountService = bankAccountService;
            this.currentMonthService = currentMonthService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            var user = await userManager.GetUserAsync(User);
            var bankAccounts = await bankAccountService.GetAllBankAccountsAsync(userId: user.Id);
            var currentExpenses = await currentMonthService.GetAllCurrentExpensesAsync(user.Id);
            if (user == null) {
                return RedirectToAction("Login", "Account");
            }
            var viewModel = new MontlyBudgetPlannerVM {
                BankAccounts = bankAccounts,
                CurrentMonths = currentExpenses,
                CurrentMonthBalance = currentMonthService.GetBalance,
                BankAccountsBalance = bankAccountService.GetBalance,
                MonthName = currentMonthService.GetMonthName,
                YearName = currentMonthService.GetYear
            };
            return View(viewModel);
        }
    }
}
