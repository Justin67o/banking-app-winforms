using BankingApp.WinForms.Services;

namespace BankingApp.WinForms.Views;

public class TransactionsView : UserControl
{
    private ComboBox? _combo;
    private FlowLayoutPanel? _panel;

    public TransactionsView()
    {
        BuildUi();
    }

    private void BuildUi()
    {
        var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, AutoScroll = true, Padding = new Padding(10) };
        flow.Controls.Add(new Label { Text = "Transactions", Font = new Font("Segoe UI", 16) });
        flow.Controls.Add(new Label { Text = "View your transaction history" });
        _combo = new ComboBox { Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
        _combo.SelectedIndexChanged += (_, _) => RefreshData();
        flow.Controls.Add(_combo);
        _panel = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true };
        flow.Controls.Add(_panel);
        Controls.Add(flow);
    }

    public void RefreshData()
    {
        var accountService = AccountService.Instance;
        var transactionService = TransactionService.Instance;
        var accounts = accountService.GetAccounts();

        if (_combo!.Items.Count == 0)
        {
            _combo.Items.Add("All Accounts");
            foreach (var a in accounts)
                _combo.Items.Add(new ComboItem { Id = a.Id, Text = $"{a.AccountName} ({a.AccountNumber})" });
            _combo.DisplayMember = "Text";
            _combo.ValueMember = "Id";
            _combo.SelectedIndex = 0;
        }

        var selected = _combo.SelectedItem is ComboItem ci ? ci.Id : null;
        var transactions = string.IsNullOrEmpty(selected)
            ? transactionService.GetTransactions()
            : transactionService.GetTransactionsByAccount(selected);

        _panel!.Controls.Clear();
        foreach (var t in transactions)
        {
            var icon = t.Type == "debit" ? "↓" : t.Type == "credit" ? "↑" : "⇄";
            var sign = t.Type == "debit" ? "-" : "+";
            var accName = accountService.GetAccountById(t.AccountId)?.AccountName ?? "Unknown";
            var to = string.IsNullOrEmpty(t.RecipientAccount) ? "" : " To: " + (accountService.GetAccountById(t.RecipientAccount)?.AccountName ?? "Unknown");
            _panel.Controls.Add(new Label { Text = $"{icon} {t.Description} | {accName} | {t.Date:MMM d, y h:mm tt}{to} | {sign}{t.Amount:C2} | Balance after: {t.Balance:C2}", AutoSize = true, MaximumSize = new Size(600, 0) });
        }
    }

    private class ComboItem
    {
        public string Id { get; set; } = "";
        public string Text { get; set; } = "";
    }
}
