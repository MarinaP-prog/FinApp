using FinApp.DTO;
using FinApp.Models;
using FinApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Services {
    public class MonthlyExpenseService {
        private ApplicationDbContext dbContext;

        private int getBalance = 0;
        public int GetBalance { get => getBalance; set => getBalance = value; }
        public MonthlyExpenseService(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }
        public async Task<BudgetPlannerDropdownVM> GetBankDropdownValue(string userId) {
            var bankDropdownVM = new BudgetPlannerDropdownVM() {
                bankAccounts = await dbContext.BankAccounts.Where(x => x.UserId == userId).OrderBy(x => x.Name).ToListAsync(),
            };
            return bankDropdownVM;
        }
        internal async Task<IEnumerable<MonthlyExpenseDTO>> GetAllMonthlyExpensesAsync(string userId) {
            var allExpenses = await dbContext.MonthlyExpense.Where(x => x.UserId == userId).Include(x => x.BankAccount).ToListAsync();
            var expensesDto = new List<MonthlyExpenseDTO>();
            var bankVM = new List<BudgetPlannerVM>();
            getBalance = 0;
            foreach (var expense in allExpenses) {
                expensesDto.Add(ModelToDto(expense));
                getBalance += expense.Amount;
            }
           
            return expensesDto;
        }
        

        public async Task CreateAsync(MonthlyExpenseDTO newMontlyExpense) {
            MonthlyExpense expenseToInsert = await DtoToModel(newMontlyExpense);
            await dbContext.MonthlyExpense.AddAsync(expenseToInsert);
            await dbContext.SaveChangesAsync();
        }
        public async Task<MonthlyExpenseDTO> GetByIdAsync(int id) {
            var montlyExpenseToEdit = await dbContext.MonthlyExpense.FirstOrDefaultAsync(x => x.Id == id);
            if (montlyExpenseToEdit == null) { return null; }
            return ModelToDto(montlyExpenseToEdit);
        }
        internal async Task UpdateAsync(MonthlyExpenseDTO updateMonthlyExpenseDTO) {
            MonthlyExpense expenseToEdit = await DtoToModel(updateMonthlyExpenseDTO);
            dbContext.MonthlyExpense.Update(expenseToEdit);
            await dbContext.SaveChangesAsync();
        }
        private  MonthlyExpenseDTO ModelToDto(MonthlyExpense expense) {
            return new MonthlyExpenseDTO {
                Amount = expense.Amount,
                Id = expense.Id,
                Name = expense.Name,
                Description = expense.Description,
                UserId = expense.UserId,
                IsJanuary = expense.IsJanuary,
                IsFebruary = expense.IsFebruary,
                IsMarch = expense.IsMarch,
                IsApril = expense.IsApril,
                IsMay = expense.IsMay,
                IsJune = expense.IsJune,
                IsJuly = expense.IsJuly,
                IsAugust = expense.IsAugust,
                IsSeptember = expense.IsSeptember,
                IsOctober = expense.IsOctober,
                IsNovember = expense.IsNovember,
                IsDecember = expense.IsDecember,
                BankAccountId = expense?.BankAccount?.Id,
                BankAccountName = expense?.BankAccount?.Name ?? "Cash"
            };
        }
        private async Task<MonthlyExpense> DtoToModel(MonthlyExpenseDTO newMontlyExpense) {
            return new MonthlyExpense() {
                Amount = newMontlyExpense.Amount,
                Id = newMontlyExpense.Id,
                Name = newMontlyExpense.Name,
                Description = newMontlyExpense.Description,
                UserId = newMontlyExpense.UserId,
                IsJanuary = newMontlyExpense.IsJanuary,
                IsFebruary = newMontlyExpense.IsFebruary,
                IsMarch = newMontlyExpense.IsMarch,
                IsApril = newMontlyExpense.IsApril,
                IsMay = newMontlyExpense.IsMay,
                IsJune = newMontlyExpense.IsJune,
                IsJuly = newMontlyExpense.IsJuly,
                IsAugust = newMontlyExpense.IsAugust,
                IsSeptember = newMontlyExpense.IsSeptember,
                IsOctober = newMontlyExpense.IsOctober,
                IsNovember = newMontlyExpense.IsNovember,
                IsDecember = newMontlyExpense.IsDecember,
                BankAccount = await dbContext.BankAccounts.FirstOrDefaultAsync(x => x.Id == newMontlyExpense.BankAccountId),
                
            };
        }

        internal async Task DeleteAsync(int id) {
           var monthlyExpenseToDelete = await dbContext.MonthlyExpense.FirstOrDefaultAsync(expense => expense.Id == id);
            dbContext.MonthlyExpense.Remove(monthlyExpenseToDelete);
            await dbContext.SaveChangesAsync();
        }
    }
}
