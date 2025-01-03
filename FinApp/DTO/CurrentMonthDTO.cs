using FinApp.Models;

namespace FinApp.DTO {
    public class CurrentMonthDTO {
        public int Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RemainingAmount { get; set; }
        public bool IsPaidExpense { get; set; }
        public string UserId { get; set; }
        public int BankAccountId { get; set; }
        public string BankAccountName { get; set; }
    }
}
