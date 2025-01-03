using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinApp.Migrations
{
    /// <inheritdoc />
    public partial class nonnullableAddedCurrent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentMonths_BankAccounts_bankAccountId",
                table: "CurrentMonths");

            migrationBuilder.RenameColumn(
                name: "bankAccountId",
                table: "CurrentMonths",
                newName: "BankAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentMonths_bankAccountId",
                table: "CurrentMonths",
                newName: "IX_CurrentMonths_BankAccountId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CurrentMonths",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentMonths_BankAccounts_BankAccountId",
                table: "CurrentMonths",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentMonths_BankAccounts_BankAccountId",
                table: "CurrentMonths");

            migrationBuilder.RenameColumn(
                name: "BankAccountId",
                table: "CurrentMonths",
                newName: "bankAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentMonths_BankAccountId",
                table: "CurrentMonths",
                newName: "IX_CurrentMonths_bankAccountId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CurrentMonths",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentMonths_BankAccounts_bankAccountId",
                table: "CurrentMonths",
                column: "bankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id");
        }
    }
}
