using FinApp.DTO;
using FinApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Services {
    public class BankAccountService {
        private ApplicationDbContext dbContext;
        private int getBalance = 0;

        public int GetBalance { get => getBalance; set => getBalance = value; }
        public BankAccountService(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }

         
        public async Task<IEnumerable<BankAccountDTO>> GetAllBankAccountsAsync(string userId) {

            var allBankAccounts = await dbContext.BankAccounts.Where(x => x.UserId == userId).ToListAsync();
            var bankAccountsDto = new List<BankAccountDTO>();
            getBalance = 0;
            foreach (var account in allBankAccounts) {
                bankAccountsDto.Add(modelToDto(account));
                getBalance += (int)account.Amount;
            }
            return bankAccountsDto;

        }
        public async Task CreateAsync(BankAccountDTO newBankAccount) {
           
            await dbContext.BankAccounts.AddAsync(dtoToModel(newBankAccount));
            await dbContext.SaveChangesAsync();
        }
        public async Task<BankAccountDTO> getByIdAsync(int id) {
            var bankAccountToEdit = await dbContext.BankAccounts.FirstOrDefaultAsync(x => x.Id == id);// FirstOrDefaultAsync - vytahne jednoho studenta nebo vrati default
            if (bankAccountToEdit == null) { return null; }//nevyhodi chybu IndexOutOfRange
            return modelToDto(bankAccountToEdit);
        }
       

        internal async Task<BankAccountDTO> UpdateAsync(int id, BankAccountDTO updateBankAccount) {
            dbContext.Update(dtoToModel(updateBankAccount));
            await dbContext.SaveChangesAsync();
            return updateBankAccount; 
        }
        private BankAccountDTO modelToDto(BankAccount account) {
            return new BankAccountDTO() {
                Id = account.Id,
                Name = account.Name,
                Amount = account.Amount,
                Description = account.Description,
                UserId = account.UserId,
                Cash = account.Cash,
            };
        }
        private BankAccount dtoToModel(BankAccountDTO bankAccount) {
            return new BankAccount() {
                Id = bankAccount.Id,
                Name = bankAccount.Name,
                Amount = bankAccount.Amount,
                Description = bankAccount.Description,
                UserId = bankAccount.UserId,
                Cash=bankAccount.Cash,
            };
        }

        internal async Task DeleteAsync(int id) {
            var bankAccountToDelete = dbContext.BankAccounts.FirstOrDefault(student => student.Id == id);
            dbContext.BankAccounts.Remove(bankAccountToDelete);
            await dbContext.SaveChangesAsync();
        }
    }
}
