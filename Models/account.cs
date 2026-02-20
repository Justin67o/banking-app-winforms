namespace BankingApp.WinForms.Models;

public class Account
{
    public string Id { get; set; } = "";
    public string AccountNumber { get; set; } = "";
    public string AccountType { get; set; } = ""; // "checking" | "savings" | "credit"
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "USD";
    public string AccountName { get; set; } = "";
}
