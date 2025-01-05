using FinApp.DTO;
using FinApp.Models;
using FinApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinApp.Controllers {
    [Authorize]
    public class CurrentMonthController : Controller {
        private CurrentMonthService currentMonthService;
        private UserManager<AppUser> userManager;

        public CurrentMonthController(CurrentMonthService currentMonthService, UserManager<AppUser> userManager) {
            this.currentMonthService = currentMonthService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync() {
            await GetDropdownValue();
            return View(new CurrentMonthDTO());
        }

        private async Task GetDropdownValue() {
            var user = await userManager.GetUserAsync(User);
            var bankDropdownValues = await currentMonthService.GetBankDropdownValue(user.Id);

            ViewBag.bankAccount = new SelectList(bankDropdownValues.bankAccounts, "Id", "Name");
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CurrentMonthDTO newCurrentMonthDTO) {
            var user = await userManager.GetUserAsync(User);
            if (user == null) {
                 return RedirectToAction("Login", "Account");
            }
            newCurrentMonthDTO.UserId = user.Id;
            await currentMonthService.CreateAsync(newCurrentMonthDTO);
            return RedirectToAction("Index", "MontlyBudgetPlannerVM");
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id) {
            await GetDropdownValue();
            var currentExpense = await currentMonthService.GetByIdAsync(id);
            if (currentExpense == null) {
                return   NotFound();
            }
            return View(currentExpense);
        }
        [HttpPost]
        public async Task<IActionResult> EditAsync(int id, CurrentMonthDTO currentMonthDTO) {
            var user = await userManager.GetUserAsync(User);
            if (user == null) {
                return RedirectToAction("Login", "Account");
            }
            currentMonthDTO.UserId = user.Id;
            await currentMonthService.UpdateAsync(id, currentMonthDTO);
            return RedirectToAction("Index", "MontlyBudgetPlannerVM");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAllAsync() {
            var user = await userManager.GetUserAsync(User);
            if (user == null) {
                return RedirectToAction("Login", "Account");
            }

            await currentMonthService.DeleteAllAsync(user.Id);
            return RedirectToAction("Index", "MontlyBudgetPlannerVM");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id) {

            var currentExpensesToDelete = await currentMonthService.GetByIdAsync(id);
            if (currentExpensesToDelete == null) {
                return View("NotFound");
            }
            await currentMonthService.DeleteAsync(id);
            return RedirectToAction("Index", "MontlyBudgetPlannerVM");
        }
    }
}
