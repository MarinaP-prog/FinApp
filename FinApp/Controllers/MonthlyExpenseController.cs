using FinApp.DTO;
using FinApp.Models;
using FinApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinApp.Controllers {
    [Authorize]
    public class MonthlyExpenseController : Controller {
        private MonthlyExpenseService monthlyExpenseService;
        public UserManager<AppUser> userManager;

        public MonthlyExpenseController(MonthlyExpenseService monthlyExpenseService, UserManager<AppUser> userManager) {
            this.monthlyExpenseService = monthlyExpenseService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync() {
            await GetDropdownValue();
            return View(new MonthlyExpenseDTO());
        }

        private async Task GetDropdownValue() {
            var user = await userManager.GetUserAsync(User);
            var bankDropdownValues = await monthlyExpenseService.GetBankDropdownValue(user.Id);

            ViewBag.bankAccount = new SelectList(bankDropdownValues.bankAccounts, "Id", "Name");
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(MonthlyExpenseDTO newMonthlyExpense) {

            var user = await userManager.GetUserAsync(User);
            if (user == null) {
                return RedirectToAction("Login", "Account");
            }
            newMonthlyExpense.UserId = user.Id;


            await monthlyExpenseService.CreateAsync(newMonthlyExpense);
            return RedirectToAction("Index", "BudgetPlannerVM");
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id) {
            await GetDropdownValue();
            var montlyExpense = await monthlyExpenseService.GetByIdAsync(id);
            if (montlyExpense == null) {
                return NotFound();
            }
            return View(montlyExpense);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(int id, MonthlyExpenseDTO monthlyExpenseDTO) {
            var user = await userManager.GetUserAsync(User);
            if (user == null) {
                return RedirectToAction("Login", "Account");
            }
            monthlyExpenseDTO.UserId = user.Id;
            await monthlyExpenseService.UpdateAsync(monthlyExpenseDTO);
            return RedirectToAction("Index", "BudgetPlannerVM");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id) {

            var monthlyExpensesToDelete = await monthlyExpenseService.GetByIdAsync(id);
            if (monthlyExpensesToDelete == null) {
                return View("NotFound");
            }
            await monthlyExpenseService.DeleteAsync(id);
            return RedirectToAction("Index", "BudgetPlannerVM");
        }
    }
}
