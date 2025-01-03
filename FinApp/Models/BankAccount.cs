using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinApp.Models {
    public class BankAccount {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Amount { get; set; }
        public string Description { get; set; }
        //uzivatel
        public string UserId { get; set; }
        public int Cash {  get; set; }
        [NotMapped]
        public IdentityApplicationUser applicationUser { get; set; }
    }
}
