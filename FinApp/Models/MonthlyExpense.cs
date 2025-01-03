using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinApp.Models {
    public class MonthlyExpense {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string UserId { get; set; }
        public string? Description { get; set; } 
        public bool IsJanuary { get; set; } = false;
        public bool IsFebruary { get; set; } = false;
        public bool IsMarch { get; set; } = false;
        public bool IsApril { get; set; } = false;
        public bool IsMay { get; set; } = false;
        public bool IsJune { get; set; } = false;
        public bool IsJuly { get; set; } = false;
        public bool IsAugust { get; set; } = false;
        public bool IsSeptember { get; set; } = false;
        public bool IsOctober { get; set; } = false;
        public bool IsNovember { get; set; } = false;
        public bool IsDecember { get; set; } = false;
        public BankAccount? BankAccount { get; set; }

    }
}
