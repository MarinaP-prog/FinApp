using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinApp.Models {
    public class CurrentMonth {
        public int Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; } 
        public string Name { get; set; }
        public string? Description { get; set; }
        public int RemainingAmount { get; set; }
        public bool IsPaidExpense { get; set; }
        public string UserId { get; set; }
        public BankAccount? BankAccount { get; set; }
        [NotMapped]
        public IdentityApplicationUser applicationUser { get; set; }
    }
}
