using BankingApp.WinForms.Services;

namespace BankingApp.WinForms.Views;

public class AccountsView : UserControl
{
    private readonly Action<string, string?> _navigate;
    private FlowLayoutPanel? _panel;
    private Label? _lblTotal;

    public AccountsView(Action<string, string?> navigate)
    {
        _navigate = navigate;
        BuildUi();
    }

    private void BuildUi()
    {
        var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, AutoScroll = true, Padding = new Padding(10) };
        flow.Controls.Add(new Label { Text = "My Accounts", Font = new Font("Segoe UI", 16), AutoSize = true });
        flow.Controls.Add(new Label { Text = "Manage and view all your accounts", AutoSize = true });
        _panel = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true };
        flow.Controls.Add(_panel);
        _lblTotal = new Label { Text = "Total Balance: ", Font = new Font("Segoe UI", 12), AutoSize = true };
        flow.Controls.Add(_lblTotal);
        Controls.Add(flow);
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
