namespace FinApp.DTO {
    public class BankAccountDTO {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Amount { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public int Cash { get; set; }
    }
}
