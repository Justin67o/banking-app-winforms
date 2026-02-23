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
        Rectangle bounds = Screen.PrimaryScreen?.WorkingArea ?? new Rectangle(0, 0, 1024, 768);
        SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        WindowState = FormWindowState.Normal;
        BackColor = Theme.Background;
        MinimumSize = new Size(1024, 600);
        StartPosition = FormStartPosition.CenterScreen;

        var nav = new Panel
        {
            Height = 56,
            Dock = DockStyle.Top,
            BackColor = Theme.Surface,
            Padding = new Padding(Theme.PadXLarge, Theme.PadMedium, Theme.PadXLarge, Theme.PadMedium)
        };
        var lblBrand = new Label
        {
            Text = "🏦 Banking App",
            AutoSize = true,
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = Theme.Primary
        };
        var btnDash = new Button
        {
            Text = "Dashboard",
            Size = new Size(100, 32),
            FlatStyle = FlatStyle.Flat,
            BackColor = Theme.Surface,
            ForeColor = Theme.TextSecondary
        };
        var btnAccounts = new Button
        {
            Text = "Accounts",
            Size = new Size(100, 32),
            FlatStyle = FlatStyle.Flat,
            BackColor = Theme.Surface,
            ForeColor = Theme.TextSecondary
        };
        var btnTrans = new Button
        {
            Text = "Transactions",
            Size = new Size(100, 32),
            FlatStyle = FlatStyle.Flat,
            BackColor = Theme.Surface,
            ForeColor = Theme.TextSecondary
        };
        var btnTransfer = new Button
        {
            Text = "Transfer",
            Size = new Size(100, 32),
            FlatStyle = FlatStyle.Flat,
            BackColor = Theme.Surface,
            ForeColor = Theme.TextSecondary
        };
        var lblUser = new Label
        {
            Text = AuthService.Instance.GetUsername(),
            AutoSize = true,
            ForeColor = Theme.TextPrimary,
            Font = Theme.BodyFont
        };
        var btnLogout = new Button
        {
            Text = "Logout",
            Size = new Size(80, 32),
            FlatStyle = FlatStyle.Flat,
            BackColor = Theme.Danger,
            ForeColor = Color.White
        };

        nav.Layout += (_, _) =>
        {
            int w = nav.ClientSize.Width - nav.Padding.Horizontal;
            int left = nav.Padding.Left;
            int brandW = lblBrand.PreferredWidth;
            int groupW = brandW + 12 + 100 * 4 + 12 * 3;
            int startX = left + Math.Max(0, (w - groupW) / 2);
            lblBrand.Location = new Point(startX, 14);
            btnDash.Location = new Point(startX + brandW + 12, 12);
            btnAccounts.Location = new Point(startX + brandW + 12 + 112, 12);
            btnTrans.Location = new Point(startX + brandW + 12 + 224, 12);
            btnTransfer.Location = new Point(startX + brandW + 12 + 336, 12);
            int rightX = nav.ClientSize.Width - nav.Padding.Right - 80 - 12 - lblUser.PreferredWidth;
            lblUser.Location = new Point(rightX, 16);
            btnLogout.Location = new Point(nav.ClientSize.Width - nav.Padding.Right - 80, 12);
        };

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
                Show();
            else
                Close();
        };

        nav.Controls.AddRange(new Control[] { lblBrand, btnDash, btnAccounts, btnTrans, btnTransfer, lblUser, btnLogout });

        _contentPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Theme.Background,
            Padding = new Padding(Theme.PadXLarge),
            AutoScroll = true
        };

        _dashboardView = new DashboardView(Navigate);
        _accountsView = new AccountsView(Navigate);
        _accountDetailsView = new AccountDetailsView(Navigate);
        _transactionsView = new TransactionsView();
        _transferView = new TransferView(Navigate);

        _dashboardView.Dock = _accountsView.Dock = _accountDetailsView.Dock = _transactionsView.Dock = _transferView.Dock = DockStyle.Fill;

        Controls.Add(nav);
        Controls.Add(_contentPanel);

        _contentPanel.Controls.Add(_dashboardView);
        _contentPanel.Controls.Add(_accountsView);
        _contentPanel.Controls.Add(_accountDetailsView);
        _contentPanel.Controls.Add(_transactionsView);
        _contentPanel.Controls.Add(_transferView);

        ShowView("Dashboard");
        Load += (_, _) =>
        {
            WindowState = FormWindowState.Maximized;
            PerformLayout();
        };
        Shown += (_, _) =>
        {
            WindowState = FormWindowState.Maximized;
            PerformLayout();
        };

    
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
