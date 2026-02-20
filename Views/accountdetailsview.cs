using BankingApp.WinForms.Services;

namespace BankingApp.WinForms.Views;

public class AccountDetailsView : UserControl
{
    private string _accountId = "";
    private readonly Action<string, string?> _navigate;
    private Label? _lblName;
    private Label? _lblBalance;
    private FlowLayoutPanel? _transPanel;

    public AccountDetailsView(Action<string, string?> navigate)
    {
        _navigate = navigate;
        BuildUi();
    }

    public void SetAccountId(string id) => _accountId = id;

    private void BuildUi()
    {
        var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, AutoScroll = true, Padding = new Padding(10) };
        var back = new LinkLabel { Text = "← Back to Accounts", AutoSize = true };
        back.Click += (_, _) => _navigate("Accounts", null);
        flow.Controls.Add(back);
        _lblName = new Label { AutoSize = true, Font = new Font("Segoe UI", 14) };
        _lblBalance = new Label { AutoSize = true };
        flow.Controls.Add(_lblName!);
        flow.Controls.Add(_lblBalance!);

        var btnTransfer = new Button { Text = "💸 Transfer Money", Size = new Size(140, 30) };
        var btnTrans = new Button { Text = "📋 View All Transactions", Size = new Size(160, 30) };
        btnTransfer.Click += (_, _) => _navigate("Transfer", null);
        btnTrans.Click += (_, _) => _navigate("Transactions", null);
        flow.Controls.Add(btnTransfer);
        flow.Controls.Add(btnTrans);

        flow.Controls.Add(new Label { Text = "Recent Transactions", Font = new Font("Segoe UI", 12) });
        _transPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true };
        flow.Controls.Add(_transPanel);
        Controls.Add(flow);
    }

    public void RefreshData()
    {
        var acc = AccountService.Instance.GetAccountById(_accountId);
        if (acc == null) return;
        _lblName!.Text = $"{acc.AccountName} ({acc.AccountNumber}) - {acc.AccountType}";
        _lblBalance!.Text = $"Current Balance: {acc.Balance:C2}";
        _lblBalance.ForeColor = acc.Balance < 0 ? Color.Red : Color.Black;

        var transactions = TransactionService.Instance.GetTransactionsByAccount(_accountId).Take(10).ToList();
        _transPanel!.Controls.Clear();
        if (transactions.Count == 0)
            _transPanel.Controls.Add(new Label { Text = "No transactions found for this account." });
        else
            foreach (var t in transactions)
            {
                var icon = t.Type == "debit" ? "↓" : t.Type == "credit" ? "↑" : "⇄";
                var sign = t.Type == "debit" ? "-" : "+";
                var to = string.IsNullOrEmpty(t.RecipientAccount) ? "" : " To: " + (AccountService.Instance.GetAccountById(t.RecipientAccount)?.AccountName ?? "Unknown");
                _transPanel.Controls.Add(new Label { Text = $"{icon} {t.Description} | {t.Date:MMM d, y h:mm tt}{to} | {sign}{t.Amount:C2} | Balance after: {t.Balance:C2}", AutoSize = true, MaximumSize = new Size(500, 0) });
            }
    }
}
