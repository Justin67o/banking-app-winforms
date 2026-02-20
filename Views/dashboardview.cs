using System.Windows.Forms;
using BankingApp.WinForms.Models;
using BankingApp.WinForms.Services;

namespace BankingApp.WinForms.Views;

public class DashboardView : UserControl
{
    private readonly Action<string, string?> _navigate;
    private FlowLayoutPanel? _accountsPanel;
    private FlowLayoutPanel? _transactionsPanel;
    private Label? _lblTotalBalance;
    private Label? _lblAccountCount;
    private Label? _lblRecentCount;

    public DashboardView(Action<string, string?> navigate)
    {
        _navigate = navigate;
        BuildUi();
    }

    private void BuildUi()
    {
        var accountService = AccountService.Instance;
        var transactionService = TransactionService.Instance;

        var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
        var flow = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };

        flow.Controls.Add(new Label { Text = "Dashboard", Font = new Font("Segoe UI", 16), AutoSize = true });
        flow.Controls.Add(new Label { Text = "Welcome back! Here's your financial overview.", AutoSize = true });

        var cards = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
        _lblTotalBalance = new Label { Text = $"💰 Total Balance: {accountService.GetTotalBalance():C2}", Size = new Size(180, 50), BorderStyle = BorderStyle.FixedSingle, TextAlign = ContentAlignment.MiddleCenter };
        _lblAccountCount = new Label { Text = $"🏦 Accounts: {accountService.GetAccounts().Count}", Size = new Size(120, 50), BorderStyle = BorderStyle.FixedSingle, TextAlign = ContentAlignment.MiddleCenter };
        _lblRecentCount = new Label { Text = "📊 Recent Transactions: 5", Size = new Size(180, 50), BorderStyle = BorderStyle.FixedSingle, TextAlign = ContentAlignment.MiddleCenter };
        cards.Controls.AddRange(new Control[] { _lblTotalBalance, _lblAccountCount, _lblRecentCount });
        flow.Controls.Add(cards);

        flow.Controls.Add(new Label { Text = "Your Accounts", Font = new Font("Segoe UI", 12), AutoSize = true });
        var viewAllAccounts = new LinkLabel { Text = "View All →", AutoSize = true };
        viewAllAccounts.Click += (_, _) => _navigate("Accounts", null);
        flow.Controls.Add(viewAllAccounts);

        _accountsPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true };
        flow.Controls.Add(_accountsPanel);

        flow.Controls.Add(new Label { Text = "Recent Transactions", Font = new Font("Segoe UI", 12), AutoSize = true });
        var viewAllTrans = new LinkLabel { Text = "View All →", AutoSize = true };
        viewAllTrans.Click += (_, _) => _navigate("Transactions", null);
        flow.Controls.Add(viewAllTrans);

        _transactionsPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true };
        flow.Controls.Add(_transactionsPanel);

        var quickFlow = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
        var btnTransfer = new Button { Text = "💸 Transfer Money", Size = new Size(140, 32) };
        var btnViewTrans = new Button { Text = "📋 View Transactions", Size = new Size(140, 32) };
        btnTransfer.Click += (_, _) => _navigate("Transfer", null);
        btnViewTrans.Click += (_, _) => _navigate("Transactions", null);
        quickFlow.Controls.Add(btnTransfer);
        quickFlow.Controls.Add(btnViewTrans);
        flow.Controls.Add(quickFlow);

        scroll.Controls.Add(flow);
        flow.Location = new Point(10, 10);
        Controls.Add(scroll);
    }

    public void RefreshData()
    {
        var accountService = AccountService.Instance;
        var transactionService = TransactionService.Instance;
        var accounts = accountService.GetAccounts();
        var transactions = transactionService.GetTransactions().Take(5).ToList();

        if (_lblTotalBalance != null) _lblTotalBalance.Text = $"💰 Total Balance: {accountService.GetTotalBalance():C2}";
        if (_lblAccountCount != null) _lblAccountCount.Text = $"🏦 Accounts: {accounts.Count}";
        if (_lblRecentCount != null) _lblRecentCount.Text = $"📊 Recent Transactions: {transactions.Count}";

        if (_accountsPanel != null)
        {
            _accountsPanel.Controls.Clear();
            foreach (var acc in accounts)
            {
                var card = new Button
                {
                    Text = $"{acc.AccountName} ({acc.AccountNumber}) {acc.Balance:C2}",
                    Size = new Size(300, 50),
                    Tag = acc.Id,
                    FlatStyle = FlatStyle.Flat
                };
                card.Click += (s, e) => _navigate("AccountDetails", (string)((Button)s!).Tag!);
                _accountsPanel.Controls.Add(card);
            }
        }

        if (_transactionsPanel != null)
        {
            _transactionsPanel.Controls.Clear();
            foreach (var t in transactions)
            {
                var icon = t.Type == "debit" ? "↓" : t.Type == "credit" ? "↑" : "⇄";
                var sign = t.Type == "debit" ? "-" : "+";
                var lbl = new Label
                {
                    Text = $"{icon} {t.Description} | {t.Date:MMM d, y} | {sign}{t.Amount:C2}",
                    AutoSize = true,
                    MaximumSize = new Size(400, 0)
                };
                _transactionsPanel.Controls.Add(lbl);
            }
        }
    }
}
