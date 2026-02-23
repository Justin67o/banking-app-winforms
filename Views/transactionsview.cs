using BankingApp.WinForms.Services;

namespace BankingApp.WinForms.Views;

public class TransactionsView : UserControl
{
    private ComboBox? _combo;
    private FlowLayoutPanel? _panel;

    public TransactionsView()
    {
        BackColor = Theme.Background;
        BuildUi();
    }

    private void BuildUi()
    {
        var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, AutoScroll = true, BackColor = Theme.Background, Padding = new Padding(Theme.PadMedium) };
                flow.Controls.Add(new Label { Text = "Transactions", Font = Theme.TitleFont, ForeColor = Theme.TextPrimary, AutoSize = true, Margin = new Padding(0, Theme.PadMedium, 0, 0) });
        flow.Controls.Add(new Label { Text = "View your transaction history", Font = Theme.BodyFont, ForeColor = Theme.TextSecondary, AutoSize = true, Margin = new Padding(0, Theme.PadSmall, 0, Theme.PadLarge) });
        _combo = new ComboBox { Width = 280, DropDownStyle = ComboBoxStyle.DropDownList, Font = Theme.BodyFont };
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
            _combo.SelectedIndex = 0;
        }

        var selected = _combo.SelectedItem is ComboItem ci ? ci.Id : null;
        var transactions = string.IsNullOrEmpty(selected)
            ? transactionService.GetTransactions()
            : transactionService.GetTransactionsByAccount(selected);

        _panel!.Controls.Clear();
        if (transactions.Count == 0)
            _panel.Controls.Add(new Label { Text = "No transactions found.", ForeColor = Theme.TextSecondary, Font = Theme.BodyFont });
        else
            foreach (var t in transactions)
            {
                var icon = t.Type == "debit" ? "↓" : t.Type == "credit" ? "↑" : "⇄";
                var iconBg = t.Type == "debit" ? Theme.IconDebitBg : t.Type == "credit" ? Theme.IconCreditBg : Theme.IconTransferBg;
                var amountColor = t.Type == "debit" ? Theme.Danger : Theme.Secondary;
                var sign = t.Type == "debit" ? "-" : "+";
                var accName = accountService.GetAccountById(t.AccountId)?.AccountName ?? "Unknown";
                var to = string.IsNullOrEmpty(t.RecipientAccount) ? "" : " To: " + (accountService.GetAccountById(t.RecipientAccount)?.AccountName ?? "Unknown");
                var row = new Panel { Size = new Size(700, 72), BackColor = Theme.Surface, BorderStyle = BorderStyle.FixedSingle, Margin = new Padding(0, 0, 0, Theme.PadSmall) };
                var lblIcon = new Label { Text = icon, Size = new Size(50, 50), Location = new Point(Theme.PadSmall, 10), BackColor = iconBg, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 14), ForeColor = amountColor };
                var lblDesc = new Label { Text = $"{t.Description}\r\n{accName} | {t.Date:MMM d, y h:mm tt}{to}", Location = new Point(72, 8), MaximumSize = new Size(400, 0), AutoSize = true, Font = Theme.BodyFont, ForeColor = Theme.TextPrimary };
                var lblAmt = new Label { Text = $"{sign}{t.Amount:C2}", Location = new Point(520, 20), AutoSize = true, Font = Theme.SectionFont, ForeColor = amountColor };
                var lblBal = new Label { Text = $"Balance after: {t.Balance:C2}", Location = new Point(72, 50), AutoSize = true, Font = Theme.BodySmallFont, ForeColor = Theme.TextSecondary };
                row.Controls.AddRange(new Control[] { lblIcon, lblDesc, lblAmt, lblBal });
                _panel.Controls.Add(row);
            }
    }

    private class ComboItem
    {
        public string Id { get; set; } = "";
        public string Text { get; set; } = "";
    }
}

