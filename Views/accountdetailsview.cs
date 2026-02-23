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
        BackColor = Theme.Background;
        BuildUi();
    }

    public void SetAccountId(string id) => _accountId = id;

    private void BuildUi()
    {
        var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, AutoScroll = true, BackColor = Theme.Background, Padding = new Padding(Theme.PadMedium) };
                var back = new Button { Text = "← Back to Accounts", FlatStyle = FlatStyle.Flat, BackColor = Theme.Background, ForeColor = Theme.TextPrimary, Size = new Size(160, 32), Font = Theme.BodyFont, Margin = new Padding(0, Theme.PadMedium, 0, 0) };
        back.Click += (_, _) => _navigate("Accounts", null);
        flow.Controls.Add(back);
        _lblName = new Label { Font = Theme.TitleFont, ForeColor = Theme.TextPrimary, AutoSize = true, Margin = new Padding(0, Theme.PadLarge, 0, Theme.PadSmall) };
        _lblBalance = new Label { Font = Theme.SectionFont, AutoSize = true };
        flow.Controls.Add(_lblName!);
        flow.Controls.Add(_lblBalance!);
        var btnTransfer = new Button { Text = "💸 Transfer Money", Size = new Size(160, 36), BackColor = Theme.Primary, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = Theme.BodyFont };
        var btnTrans = new Button { Text = "📋 View All Transactions", Size = new Size(180, 36), BackColor = Theme.Primary, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = Theme.BodyFont, Margin = new Padding(Theme.PadMedium, 0, 0, 0) };
        btnTransfer.Click += (_, _) => _navigate("Transfer", null);
        btnTrans.Click += (_, _) => _navigate("Transactions", null);
        flow.Controls.Add(btnTransfer);
        flow.Controls.Add(btnTrans);
        flow.Controls.Add(new Label { Text = "Recent Transactions", Font = Theme.SectionFont, ForeColor = Theme.TextPrimary, AutoSize = true, Margin = new Padding(0, Theme.PadLarge, 0, Theme.PadSmall) });
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
        _lblBalance.ForeColor = acc.Balance < 0 ? Theme.Danger : Theme.TextPrimary;

        var transactions = TransactionService.Instance.GetTransactionsByAccount(_accountId).Take(10).ToList();
        _transPanel!.Controls.Clear();
        if (transactions.Count == 0)
            _transPanel.Controls.Add(new Label { Text = "No transactions found for this account.", ForeColor = Theme.TextSecondary, Font = Theme.BodyFont });
        else
            foreach (var t in transactions)
            {
                var icon = t.Type == "debit" ? "↓" : t.Type == "credit" ? "↑" : "⇄";
                var iconBg = t.Type == "debit" ? Theme.IconDebitBg : t.Type == "credit" ? Theme.IconCreditBg : Theme.IconTransferBg;
                var amountColor = t.Type == "debit" ? Theme.Danger : Theme.Secondary;
                var sign = t.Type == "debit" ? "-" : "+";
                var to = string.IsNullOrEmpty(t.RecipientAccount) ? "" : " To: " + (AccountService.Instance.GetAccountById(t.RecipientAccount)?.AccountName ?? "Unknown");
                var row = new Panel { Size = new Size(600, 70), BackColor = Theme.Surface, BorderStyle = BorderStyle.FixedSingle, Margin = new Padding(0, 0, 0, Theme.PadSmall) };
                var lblIcon = new Label { Text = icon, Size = new Size(48, 48), Location = new Point(Theme.PadSmall, 10), BackColor = iconBg, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 14), ForeColor = amountColor };
                var lblDesc = new Label { Text = $"{t.Description}\r\n{t.Date:MMM d, y h:mm tt}{to}", Location = new Point(70, 8), AutoSize = true, Font = Theme.BodyFont, ForeColor = Theme.TextPrimary };
                var lblAmt = new Label { Text = $"{sign}{t.Amount:C2}", Location = new Point(450, 18), AutoSize = true, Font = Theme.SectionFont, ForeColor = amountColor };
                var lblBal = new Label { Text = $"Balance after: {t.Balance:C2}", Location = new Point(70, 48), AutoSize = true, Font = Theme.BodySmallFont, ForeColor = Theme.TextSecondary };
                row.Controls.AddRange(new Control[] { lblIcon, lblDesc, lblAmt, lblBal });
                _transPanel.Controls.Add(row);
            }
    }
}
