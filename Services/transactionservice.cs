using BankingApp.WinForms.Models;

namespace BankingApp.WinForms.Services;

public class TransactionService
{
    public static TransactionService Instance { get; } = new TransactionService();
    private readonly StorageService _storage = new StorageService();
    private List<Transaction> _transactions;
    private readonly AccountService _accountService = AccountService.Instance;

    private TransactionService()
    {
        _transactions = _storage.LoadTransactions();
        if (_transactions.Count == 0)
        {
            _transactions = GetInitialTransactions();
            _storage.SaveTransactions(_transactions);
        }
    }

    private static List<Transaction> GetInitialTransactions()
    {
        var now = DateTime.Now;
        return new List<Transaction>
        {
            new() { Id = "1", AccountId = "1", Type = "debit", Amount = 45.50m, Description = "Grocery Store Purchase", Date = now.AddDays(-2), Balance = 5420.50m },
            new() { Id = "2", AccountId = "2", Type = "credit", Amount = 500.00m, Description = "Salary Deposit", Date = now.AddDays(-5), Balance = 12500.00m },
            new() { Id = "3", AccountId = "1", Type = "transfer", Amount = 200.00m, Description = "Transfer to Savings", Date = now.AddDays(-7), Balance = 5466.00m, RecipientAccount = "2" },
            new() { Id = "4", AccountId = "3", Type = "debit", Amount = 89.25m, Description = "Online Purchase", Date = now.AddDays(-10), Balance = -1250.75m }
        };
    }

    public List<Transaction> GetTransactions() => _transactions.OrderByDescending(t => t.Date).ToList();

    public List<Transaction> GetTransactionsByAccount(string accountId) =>
        _transactions.Where(t => t.AccountId == accountId || t.RecipientAccount == accountId)
            .OrderByDescending(t => t.Date).ToList();

    public void AddTransaction(string accountId, string type, decimal amount, string description, string? recipientAccount = null)
    {
        var account = _accountService.GetAccountById(accountId);
        if (account == null) return;

        decimal newBalance = account.Balance;
        if (type == "credit") newBalance += amount;
        else if (type == "debit") newBalance -= amount;
        else if (type == "transfer" && !string.IsNullOrEmpty(recipientAccount))
        {
            newBalance -= amount;
            var recipient = _accountService.GetAccountById(recipientAccount);
            if (recipient != null)
                _accountService.UpdateAccountBalance(recipient.Id, recipient.Balance + amount);
        }

        _accountService.UpdateAccountBalance(account.Id, newBalance);

        var tx = new Transaction
        {
            Id = DateTime.UtcNow.Ticks.ToString(),
            AccountId = accountId,
            Type = type,
            Amount = amount,
            Description = description,
            Date = DateTime.Now,
            Balance = newBalance,
            RecipientAccount = recipientAccount
        };
        _transactions.Insert(0, tx);
        _storage.SaveTransactions(_transactions);
    }
}
