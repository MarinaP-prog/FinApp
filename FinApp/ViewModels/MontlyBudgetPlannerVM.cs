using FinApp.DTO;

namespace FinApp.ViewModels {
    public class MontlyBudgetPlannerVM {
        public IEnumerable<BankAccountDTO> BankAccounts { get; set; }
        public IEnumerable<CurrentMonthDTO> CurrentMonths { get; set; }
        public int CurrentMonthBalance { get; set; }
        public int BankAccountsBalance { get; set; }
        public bool IsPaidExpense { get; set; }
        public string MonthName { get; set; }
        public int YearName {  get; set; }
    }
}
