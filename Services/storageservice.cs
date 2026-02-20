using System.Text.Json;
using BankingApp.WinForms.Models;

namespace BankingApp.WinForms.Services;

public class StorageService
{
    private static readonly string Folder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "BankingApp");
    private static readonly string AccountsPath = Path.Combine(Folder, "accounts.json");
    private static readonly string TransactionsPath = Path.Combine(Folder, "transactions.json");
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    public List<Account> LoadAccounts()
    {
        if (!File.Exists(AccountsPath)) return new List<Account>();
        var json = File.ReadAllText(AccountsPath);
        return JsonSerializer.Deserialize<List<Account>>(json) ?? new List<Account>();
    }

    public void SaveAccounts(List<Account> accounts)
    {
        Directory.CreateDirectory(Folder);
        File.WriteAllText(AccountsPath, JsonSerializer.Serialize(accounts, Options));
    }

    public List<Transaction> LoadTransactions()
    {
        if (!File.Exists(TransactionsPath)) return new List<Transaction>();
        var json = File.ReadAllText(TransactionsPath);
        var list = JsonSerializer.Deserialize<List<TransactionJson>>(json);
        if (list == null) return new List<Transaction>();
        return list.Select(t => new Transaction
        {
            Id = t.Id,
            AccountId = t.AccountId,
            Type = t.Type,
            Amount = t.Amount,
            Description = t.Description,
            Date = DateTime.Parse(t.Date),
            Balance = t.Balance,
            RecipientAccount = t.RecipientAccount
        }).ToList();
    }

    public void SaveTransactions(List<Transaction> transactions)
    {
        Directory.CreateDirectory(Folder);
        var list = transactions.Select(t => new TransactionJson
        {
            Id = t.Id,
            AccountId = t.AccountId,
            Type = t.Type,
            Amount = t.Amount,
            Description = t.Description,
            Date = t.Date.ToString("O"),
            Balance = t.Balance,
            RecipientAccount = t.RecipientAccount
        }).ToList();
        File.WriteAllText(TransactionsPath, JsonSerializer.Serialize(list, Options));
    }

    private class TransactionJson
    {
        public string Id { get; set; } = "";
        public string AccountId { get; set; } = "";
        public string Type { get; set; } = "";
        public decimal Amount { get; set; }
        public string Description { get; set; } = "";
        public string Date { get; set; } = "";
        public decimal Balance { get; set; }
        public string? RecipientAccount { get; set; }
    }
}
