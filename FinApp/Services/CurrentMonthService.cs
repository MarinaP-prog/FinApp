using FinApp.DTO;
using FinApp.Models;
using FinApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Services {
    public class CurrentMonthService {
        private ApplicationDbContext dbContext;
        private int getBalance = 0;
        private int getMonth;
        private string getMonthName;
        private int getYear;
        public int GetBalance { get => getBalance; set => getBalance = value; }
        public string GetMonthName { get => getMonthName; set => getMonthName = value; }
        public int GetYear { get => getYear; set => getYear = value; }

        public CurrentMonthService(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }
        internal async Task<IEnumerable<CurrentMonthDTO>> GetAllCurrentExpensesAsync(string userId) {

            var allCurrentExpenses = await dbContext.CurrentMonths.Where(x => x.UserId == userId).Include(x => x.BankAccount).ToListAsync();
            //jestli nejsou zadne = kopiruj
            //filter na zvoleny rok a mesic, 
            if (!allCurrentExpenses.Any()) {
                var monthly = await dbContext.MonthlyExpense.Where(x => x.UserId == userId).ToListAsync();
                var copiedCurrent = CopyMontlyExpensses(monthly);

                dbContext.CurrentMonths.AddRange(copiedCurrent);
                await dbContext.SaveChangesAsync();

                allCurrentExpenses = copiedCurrent;

            }
            var currentExpensesDto = new List<CurrentMonthDTO>();
            getBalance = 0;
            getMonth = 0;
            getYear = 0;
            foreach (var current in allCurrentExpenses) {
                currentExpensesDto.Add(ModelToDto(current));
                getBalance += current.RemainingAmount;
                getMonth = current.Month;
                getYear = current.Year;
            }
            getMonthName = MonthNameString(getMonth);
            return currentExpensesDto;
        }
        //for db

        public async Task CreateAsync(CurrentMonthDTO newCurrentMonth) {
            CurrentMonth expenseToInsert = await DtoToModel(newCurrentMonth);
            await dbContext.CurrentMonths.AddAsync(expenseToInsert);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAllAsync(string userId) {
            var currentExpenseToDelete = await dbContext.CurrentMonths.Where(x => x.UserId == userId).ToListAsync();

            dbContext.CurrentMonths.RemoveRange(currentExpenseToDelete);
            await dbContext.SaveChangesAsync();
        }

        internal async Task UpdateAsync(int id,CurrentMonthDTO updateCurrentMonthDTO) {
            CurrentMonth expenseToEdit = await DtoToModel(updateCurrentMonthDTO);
           dbContext.CurrentMonths.Update(expenseToEdit);
            await dbContext.SaveChangesAsync();
        }

        public static List<CurrentMonth> CopyMontlyExpensses(List<MonthlyExpense> montlyExpense) {
            
            List<CurrentMonth> copyCurrentMonths = new List<CurrentMonth>();
            DateTime currentDate = System.DateTime.Now ;
            foreach (var m in montlyExpense) {
                if (FilterCurrentMonth(currentDate, m)) {
                    copyCurrentMonths.Add(new CurrentMonth {
                        Month = currentDate.Month,
                        Year = currentDate.Year,
                        Name = m.Name,
                        Description = m.Description,
                        RemainingAmount = m.Amount,
                        UserId = m.UserId,
                        BankAccount = m.BankAccount,
                    });
                }
            }
            return copyCurrentMonths;
        }
        public static string MonthNameString(int getMonth) {
            string currentMonthName = "";
            switch (getMonth) {
                case  1: currentMonthName = "January";
                    break;
                    case 2: currentMonthName = "February";
                    break ;
                    case 3:
                    currentMonthName = "March";
                    break;
                    case 4:
                    currentMonthName = "April";
                    break;
                    case 5:
                    currentMonthName = "May";
                    break;
                    case 6:
                    currentMonthName = "June";
                    break;
                    case 7:
                    currentMonthName = "July";
                    break;
                    case 8:
                    currentMonthName = "August";
                    break;
                    case 9:
                    currentMonthName = "September";
                    break;
                    case 10:
                    currentMonthName = "October";
                    break;
                    case 11:
                    currentMonthName = "November";
                    break;
                    case 12:
                    currentMonthName = "December";
                    break;
            }
            return currentMonthName;
        }
        private static bool FilterCurrentMonth(DateTime currentDate, MonthlyExpense m) {
            return (m.IsDecember && currentDate.Month == 12) ||
                   (m.IsJanuary && currentDate.Month == 1) ||
                   (m.IsFebruary && currentDate.Month == 2) ||
                   (m.IsMarch && currentDate.Month == 3) ||
                   (m.IsApril && currentDate.Month == 4) ||
                   (m.IsMay && currentDate.Month == 5) ||
                   (m.IsJune && currentDate.Month == 6) ||
                   (m.IsJuly && currentDate.Month == 7) ||
                   (m.IsAugust && currentDate.Month == 8) ||
                   (m.IsSeptember && currentDate.Month == 9) ||
                   (m.IsOctober && currentDate.Month == 10) ||
                   (m.IsNovember && currentDate.Month == 11);
        }
        private  CurrentMonthDTO ModelToDto(CurrentMonth current) {
            return new CurrentMonthDTO {
                Id = current.Id,
                Month = current?.Month ?? 0,
                Year = current?.Year ?? 0, 
                Name = current?.Name ?? "N/A", 
                Description = current?.Description ?? "No description",
                RemainingAmount = current?.RemainingAmount ?? 0,
                UserId = current.UserId,
                IsPaidExpense = current.IsPaidExpense,
                BankAccountId = current?.BankAccount?.Id ?? 0,
                BankAccountName = current?.BankAccount?.Name ?? "Cash"
            };
        }
        private  async Task<CurrentMonth> DtoToModel(CurrentMonthDTO newCurrentMonth) {
            DateTime currentDate = System.DateTime.Now;
            return new CurrentMonth() {
                Id = newCurrentMonth.Id,
                Month = currentDate.Month,
                Year = currentDate.Year,
                Description = newCurrentMonth.Description,
                Name = newCurrentMonth.Name,
                RemainingAmount = newCurrentMonth.RemainingAmount,
                UserId=newCurrentMonth.UserId,
                IsPaidExpense = newCurrentMonth.IsPaidExpense,
                BankAccount = await dbContext.BankAccounts.FirstOrDefaultAsync(x =>  x.Id == newCurrentMonth.BankAccountId),
            };
        }

        internal async Task<CurrentMonthDTO> GetByIdAsync(int id) {
           var currentExpenseAction = await dbContext.CurrentMonths.FirstOrDefaultAsync(x => x.Id == id);
            if (currentExpenseAction == null) { return null; }
            return ModelToDto(currentExpenseAction);
        }

        internal async Task<BudgetPlannerDropdownVM> GetBankDropdownValue(string userId) {
            var bankDropdownVM = new BudgetPlannerDropdownVM() {
                bankAccounts = await dbContext.BankAccounts.Where(x => x.UserId == userId).OrderBy(x => x.Name).ToListAsync(),
            };
            return bankDropdownVM;
        }

        internal async Task DeleteAsync(int id) {
            var currentExpenseToDelete = await dbContext.CurrentMonths.FirstOrDefaultAsync(expense => expense.Id == id);
            dbContext.CurrentMonths.Remove(currentExpenseToDelete);
            await dbContext.SaveChangesAsync();
        }
    }
}
