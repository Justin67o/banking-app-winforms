namespace BankingApp.WinForms.Models;

public class Transaction
{
    public string Id { get; set; } = "";
    public string AccountId { get; set; } = "";
    public string Type { get; set; } = ""; // "debit" | "credit" | "transfer"
    public decimal Amount { get; set; }
    public string Description { get; set; } = "";
    public DateTime Date { get; set; }
    public decimal Balance { get; set; }
    public string? RecipientAccount { get; set; }
}

