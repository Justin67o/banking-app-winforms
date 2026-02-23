using BankingApp.WinForms.Services;

namespace BankingApp.WinForms.Views;

public class TransactionsView : UserControl
{
    private ComboBox? _combo;
    private FlowLayoutPanel? _panel;
    private Panel? _contentWrap;
    private Panel? _titleWrap;
    private int _contentWidth = 700;

    public TransactionsView()
    {
        BackColor = Theme.Background;
        BuildUi();
    }

    private void BuildUi()
    {
        var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Theme.Background };
        _contentWrap = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Background, Padding = new Padding(100, Theme.PadMedium, 100, Theme.PadMedium) };
        var flow = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true,
            BackColor = Theme.Background,
            Dock = DockStyle.Top,
            Padding = new Padding(0)
        };
        _titleWrap = new Panel { Height = 70, Margin = new Padding(0, 0, 0, Theme.PadMedium) };
        var titleLbl = new Label { Text = "Transactions", Font = Theme.TitleFont, ForeColor = Theme.TextPrimary, AutoSize = true };
        var subtitleLbl = new Label { Text = "View your transaction history", Font = Theme.BodyFont, ForeColor = Theme.TextSecondary, AutoSize = true };
        _titleWrap.Controls.Add(titleLbl);
        _titleWrap.Controls.Add(subtitleLbl);
        _titleWrap.Layout += (_, _) =>
        {
            if (_titleWrap == null) return;
            titleLbl.Location = new Point(Math.Max(0, (_titleWrap.ClientSize.Width - titleLbl.Width) / 2), 0);
            subtitleLbl.Location = new Point(Math.Max(0, (_titleWrap.ClientSize.Width - subtitleLbl.Width) / 2), titleLbl.Height + Theme.PadSmall);
        };
        flow.Controls.Add(_titleWrap);
        _combo = new ComboBox { Width = 280, DropDownStyle = ComboBoxStyle.DropDownList, Font = Theme.BodyFont };
        _combo.SelectedIndexChanged += (_, _) => RefreshData();
        flow.Controls.Add(_combo);
        _panel = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true };
        flow.Controls.Add(_panel);

        _contentWrap.Controls.Add(flow);
        flow.Location = new Point(0, 0);
        scroll.Controls.Add(_contentWrap);
        Controls.Add(scroll);

        void OnContentResize()
        {
            if (_contentWrap == null) return;
            int availableWidth = _contentWrap.ClientSize.Width - _contentWrap.Padding.Horizontal;
            if (availableWidth > 0)
            {
                _contentWidth = availableWidth;
                if (_panel != null) _panel.Width = availableWidth;
                if (_titleWrap != null) _titleWrap.Width = availableWidth;
            }
        }
        _contentWrap.Resize += (_, _) => OnContentResize();
        Load += (_, _) => OnContentResize();
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
                int rowWidth = _contentWidth;
                var icon = t.Type == "debit" ? "↓" : t.Type == "credit" ? "↑" : "⇄";
                var iconBg = t.Type == "debit" ? Theme.IconDebitBg : t.Type == "credit" ? Theme.IconCreditBg : Theme.IconTransferBg;
                var amountColor = t.Type == "debit" ? Theme.Danger : Theme.Secondary;
                var sign = t.Type == "debit" ? "-" : "+";
                var accName = accountService.GetAccountById(t.AccountId)?.AccountName ?? "Unknown";
                var to = string.IsNullOrEmpty(t.RecipientAccount) ? "" : " To: " + (accountService.GetAccountById(t.RecipientAccount)?.AccountName ?? "Unknown");
                var row = new Panel { Size = new Size(rowWidth, 80), BackColor = Theme.Surface, BorderStyle = BorderStyle.FixedSingle, Margin = new Padding(0, 0, 0, Theme.PadSmall) };
                var lblIcon = new Label { Text = icon, Size = new Size(50, 50), Location = new Point(Theme.PadSmall, 15), BackColor = iconBg, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 14), ForeColor = amountColor };
                var lblDesc = new Label { Text = $"{t.Description}\r\n{accName} | {t.Date:MMM d, y h:mm tt}{to}", Location = new Point(72, 10), MaximumSize = new Size(rowWidth - 220, 0), AutoSize = true, Font = Theme.BodyFont, ForeColor = Theme.TextPrimary };
                var lblAmt = new Label { Text = $"{sign}{t.Amount:C2}", Location = new Point(rowWidth - 120, 28), AutoSize = true, Font = Theme.SectionFont, ForeColor = amountColor };
                var lblBal = new Label { Text = $"Balance after: {t.Balance:C2}", Location = new Point(72, 54), AutoSize = true, Font = Theme.BodySmallFont, ForeColor = Theme.TextSecondary };
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

