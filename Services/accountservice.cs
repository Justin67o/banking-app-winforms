using BankingApp.WinForms.Models;

namespace BankingApp.WinForms.Services;

public class AccountService
{
    public static AccountService Instance { get; } = new AccountService();
    private readonly StorageService _storage = new StorageService();
    private List<Account> _accounts;

    private AccountService()
    {
        _accounts = _storage.LoadAccounts();
        if (_accounts.Count == 0)
        {
            _accounts = GetInitialAccounts();
            _storage.SaveAccounts(_accounts);
        }
    }

    private static List<Account> GetInitialAccounts() => new()
    {
        new Account { Id = "1", AccountNumber = "****1234", AccountType = "checking", Balance = 5420.50m, Currency = "USD", AccountName = "Primary Checking" },
        new Account { Id = "2", AccountNumber = "****5678", AccountType = "savings", Balance = 12500.00m, Currency = "USD", AccountName = "Savings Account" },
        new Account { Id = "3", AccountNumber = "****9012", AccountType = "credit", Balance = -1250.75m, Currency = "USD", AccountName = "Credit Card" }
    };

    public List<Account> GetAccounts() => _accounts.ToList();
    public Account? GetAccountById(string id) => _accounts.FirstOrDefault(a => a.Id == id);

    public void UpdateAccountBalance(string accountId, decimal newBalance)
    {
        var acc = _accounts.FirstOrDefault(a => a.Id == accountId);
        if (acc != null)
        {
            acc.Balance = newBalance;
            _storage.SaveAccounts(_accounts);
        }
    }

    public decimal GetTotalBalance() => _accounts.Sum(a => a.Balance);
}
