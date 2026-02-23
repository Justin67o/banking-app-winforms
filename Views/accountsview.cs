using BankingApp.WinForms.Services;

namespace BankingApp.WinForms.Views;

public class AccountsView : UserControl
{
    private readonly Action<string, string?> _navigate;
    private FlowLayoutPanel? _panel;
    private Label? _lblTotal;
    private Panel? _scrollPanel;
    private Panel? _contentWrap;

    public AccountsView(Action<string, string?> navigate)
    {
        _navigate = navigate;
        BuildUi();
    }

    private void BuildUi()
    {
        _scrollPanel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Theme.Background };
        _contentWrap = new Panel { BackColor = Theme.Background, Padding = new Padding(0, Theme.PadMedium, Theme.PadMedium, Theme.PadMedium) };
        var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, AutoScroll = true, Padding = new Padding(Theme.PadMedium) };
        flow.Controls.Add(new Label { Text = "My Accounts", Font = Theme.TitleFont, ForeColor = Theme.TextPrimary, AutoSize = true });
        flow.Controls.Add(new Label { Text = "Manage and view all your accounts", Font = Theme.BodyFont, ForeColor = Theme.TextSecondary, AutoSize = true, Margin = new Padding(0, Theme.PadSmall, 0, Theme.PadLarge) });
        _panel = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true };
        flow.Controls.Add(_panel);
        _lblTotal = new Label { Text = "Total Balance: ", Font = new Font("Segoe UI", 12), AutoSize = true };
        flow.Controls.Add(_lblTotal);

        _contentWrap.Controls.Add(flow);
        _scrollPanel.Controls.Add(_contentWrap);
        _contentWrap.Location = new Point(0, 0);
        Controls.Add(_scrollPanel);
        _scrollPanel.Resize += (_, _) =>
        {
            if (_contentWrap != null && _scrollPanel != null && _scrollPanel.ClientSize.Width > 0)
                _contentWrap.Size = _scrollPanel.ClientSize;
        };
        if (_scrollPanel.ClientSize.Width > 0)
            _contentWrap.Size = _scrollPanel.ClientSize;
    
    }

    public void RefreshData()
    {
        var accountService = AccountService.Instance;
        var accounts = accountService.GetAccounts();
        _lblTotal!.Text = $"Total Balance: {accountService.GetTotalBalance():C2}";
        _panel!.Controls.Clear();
        foreach (var acc in accounts)
        {
            var icon = acc.AccountType == "savings" ? "💰" : "💳";
            var card = new Button
            {
                Text = $"{icon} {acc.AccountName} | {acc.AccountNumber} | {acc.AccountType} | {acc.Balance:C2}",
                Size = new Size(400, 60),
                Tag = acc.Id,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft
            };
            card.Click += (s, e) => _navigate("AccountDetails", (string)((Button)s!).Tag!);
            _panel.Controls.Add(card);
        }
    }
}
