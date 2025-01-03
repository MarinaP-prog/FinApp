using FinApp.DTO;
//using FinApp.ViewModels;

namespace FinApp.ViewModels {
    public class BudgetPlannerVM {
        public IEnumerable<BankAccountDTO> BankAccounts { get; set; }
        public IEnumerable<MonthlyExpenseDTO> MonthlyExpenses { get; set; }
        public IEnumerable<CurrentMonthDTO> CurrentMonth { get; set; }
        public int BankAccountsBalance { get; set; }
        public int MonthlyExpensesBalance { get;set; }

    }
}
