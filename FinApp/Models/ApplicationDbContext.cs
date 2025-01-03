using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Models {
    public class ApplicationDbContext : IdentityDbContext<AppUser> {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
        }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<MonthlyExpense> MonthlyExpense { get; set; }
        public DbSet<CurrentMonth> CurrentMonths { get; set; }
    }
}
