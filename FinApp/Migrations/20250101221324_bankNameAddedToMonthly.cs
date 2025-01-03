using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinApp.Migrations
{
    /// <inheritdoc />
    public partial class bankNameAddedToMonthly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyExpense_BankAccounts_bankAccountId",
                table: "MonthlyExpense");

            migrationBuilder.RenameColumn(
                name: "bankAccountId",
                table: "MonthlyExpense",
                newName: "BankAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_MonthlyExpense_bankAccountId",
                table: "MonthlyExpense",
                newName: "IX_MonthlyExpense_BankAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyExpense_BankAccounts_BankAccountId",
                table: "MonthlyExpense",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyExpense_BankAccounts_BankAccountId",
                table: "MonthlyExpense");

            migrationBuilder.RenameColumn(
                name: "BankAccountId",
                table: "MonthlyExpense",
                newName: "bankAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_MonthlyExpense_BankAccountId",
                table: "MonthlyExpense",
                newName: "IX_MonthlyExpense_bankAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyExpense_BankAccounts_bankAccountId",
                table: "MonthlyExpense",
                column: "bankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id");
        }
    }
}
