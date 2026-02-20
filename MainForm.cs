using BankingApp.WinForms.Services;
using BankingApp.WinForms.Views;

namespace BankingApp.WinForms;

public partial class MainForm : Form
{
    private readonly Panel _contentPanel;
    private readonly DashboardView _dashboardView;
    private readonly AccountsView _accountsView;
    private readonly AccountDetailsView _accountDetailsView;
    private readonly TransactionsView _transactionsView;
    private readonly TransferView _transferView;

    public MainForm()
    {
        Text = "Banking App";

        var nav = new Panel { Height = 50, Dock = DockStyle.Top, BackColor = Color.LightGray };
        var lblBrand = new Label { Text = "🏦 Banking App", Location = new Point(10, 12), AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
        var btnDash = new Button { Text = "Dashboard", Location = new Point(180, 10), Size = new Size(90, 28) };
        var btnAccounts = new Button { Text = "Accounts", Location = new Point(275, 10), Size = new Size(90, 28) };
        var btnTrans = new Button { Text = "Transactions", Location = new Point(370, 10), Size = new Size(90, 28) };
        var btnTransfer = new Button { Text = "Transfer", Location = new Point(465, 10), Size = new Size(90, 28) };
        var lblUser = new Label { Text = AuthService.Instance.GetUsername(), Location = new Point(600, 14), AutoSize = true };
        var btnLogout = new Button { Text = "Logout", Location = new Point(680, 10), Size = new Size(70, 28) };

        btnDash.Click += (_, _) => ShowView("Dashboard");
        btnAccounts.Click += (_, _) => ShowView("Accounts");
        btnTrans.Click += (_, _) => ShowView("Transactions");
        btnTransfer.Click += (_, _) => ShowView("Transfer");
        btnLogout.Click += (_, _) =>
        {
            AuthService.Instance.Logout();
            var login = new LoginForm();
            Hide();
            if (login.ShowDialog() == DialogResult.OK)
            {
                Show();
            }
            else
                Close();
        };

        nav.Controls.AddRange(new Control[] { lblBrand, btnDash, btnAccounts, btnTrans, btnTransfer, lblUser, btnLogout });
        _contentPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };

        _dashboardView = new DashboardView(Navigate);
        _accountsView = new AccountsView(Navigate);
        _accountDetailsView = new AccountDetailsView(Navigate);
        _transactionsView = new TransactionsView();
        _transferView = new TransferView(Navigate);

        _dashboardView.Dock = _accountsView.Dock = _accountDetailsView.Dock = _transactionsView.Dock = _transferView.Dock = DockStyle.Fill;

        Controls.Add(_contentPanel);
        Controls.Add(nav);
        _contentPanel.Controls.Add(_dashboardView);
        _contentPanel.Controls.Add(_accountsView);
        _contentPanel.Controls.Add(_accountDetailsView);
        _contentPanel.Controls.Add(_transactionsView);
        _contentPanel.Controls.Add(_transferView);

        ShowView("Dashboard");
    }

    public void Navigate(string view, string? param = null)
    {
        if (view == "AccountDetails" && !string.IsNullOrEmpty(param))
            _accountDetailsView.SetAccountId(param);
        ShowView(view);
    }

    private void ShowView(string view)
    {
        _dashboardView.Visible = view == "Dashboard";
        _accountsView.Visible = view == "Accounts";
        _accountDetailsView.Visible = view == "AccountDetails";
        _transactionsView.Visible = view == "Transactions";
        _transferView.Visible = view == "Transfer";

        if (view == "Dashboard") _dashboardView.RefreshData();
        if (view == "Accounts") _accountsView.RefreshData();
        if (view == "AccountDetails") _accountDetailsView.RefreshData();
        if (view == "Transactions") _transactionsView.RefreshData();
        if (view == "Transfer") _transferView.RefreshData();
    }
}
