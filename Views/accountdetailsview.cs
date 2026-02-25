using BankingApp.WinForms.Models;
using BankingApp.WinForms.Services;

namespace BankingApp.WinForms.Views;

public class AccountDetailsView : UserControl
{
    private string _accountId = "";
    private readonly Action<string, string?> _navigate;
    private Label? _lblOverviewName;
    private Label? _lblOverviewNumber;
    private Label? _lblTypePill;
    private Label? _lblBalanceAmount;
    private Label? _lblInfoNumber;
    private Label? _lblInfoType;
    private Label? _lblInfoCurrency;
    private Label? _lblInfoName;
    private FlowLayoutPanel? _transPanel;
    private Panel? _contentWrap;
    private Panel? _overviewCard;
    private Label? _overviewIcon;
    private Panel? _transCard;
    private Panel? _infoCard;
    private Panel? _quickCard;
    private TableLayoutPanel? _transHeader;
    private int _contentWidth = 800;

    public AccountDetailsView(Action<string, string?> navigate)
    {
        _navigate = navigate;
        BackColor = Theme.Background;
        BuildUi();
    }

    public void SetAccountId(string id) => _accountId = id;

    private void BuildUi()
    {
        var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Theme.Background };
        _contentWrap = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Background, Padding = new Padding(100, Theme.PadXLarge + Theme.PadMedium, 100, Theme.PadMedium) };
        var flow = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true,
            BackColor = Theme.Background,
            Dock = DockStyle.Top
        };

        var back = new Button
        {
            Text = "← Back to Accounts",
            FlatStyle = FlatStyle.Flat,
            BackColor = Theme.Background,
            ForeColor = Theme.TextPrimary,
            Size = new Size(160, 32),
            Font = Theme.BodyFont,
            Margin = new Padding(0, Theme.PadMedium, 0, Theme.PadMedium)
        };
        back.Click += (_, _) => _navigate("Accounts", null);
        flow.Controls.Add(back);

        // Account Overview Card
        _overviewCard = new Panel
        {
            BackColor = Theme.Surface,
            BorderStyle = BorderStyle.FixedSingle,
            Size = new Size(800, 192),
            Margin = new Padding(0, 0, 0, Theme.PadMedium),
            Padding = new Padding(Theme.PadXLarge)
        };
        var overviewCard = _overviewCard;
        _overviewIcon = new Label
        {
            Text = "💳",
            Font = new Font("Segoe UI", 24),
            Size = new Size(56, 56),
            Location = new Point(Theme.PadMedium, Theme.PadMedium),
            BackColor = Theme.IconTransferBg,
            TextAlign = ContentAlignment.MiddleCenter
        };
        var iconBox = _overviewIcon;
        _lblOverviewName = new Label
        {
            Text = "",
            Font = Theme.TitleFont,
            ForeColor = Theme.TextPrimary,
            Location = new Point(88, Theme.PadMedium),
            AutoSize = true
        };
        _lblOverviewNumber = new Label
        {
            Text = "",
            Font = Theme.BodyFont,
            ForeColor = Theme.TextSecondary,
            Location = new Point(88, 58),
            AutoSize = true
        };
        _lblTypePill = new Label
        {
            Text = "",
            Font = Theme.BodyFont,
            ForeColor = Color.White,
            BackColor = Theme.Primary,
            Location = new Point(88, 82),
            AutoSize = true,
            Padding = new Padding(Theme.PadSmall, 4, Theme.PadSmall, 4)
        };
        var currentBalanceLbl = new Label
        {
            Text = "Current Balance",
            Font = Theme.BodyFont,
            ForeColor = Theme.TextSecondary,
            Location = new Point(overviewCard.Width - 180, Theme.PadMedium),
            AutoSize = true
        };
        _lblBalanceAmount = new Label
        {
            Text = "$0.00",
            Font = new Font("Segoe UI", 24, FontStyle.Bold),
            ForeColor = Theme.TextPrimary,
            Location = new Point(overviewCard.Width - 180, 44),
            AutoSize = true
        };
        overviewCard.Controls.AddRange(new Control[] { iconBox, _lblOverviewName, _lblOverviewNumber, _lblTypePill, currentBalanceLbl, _lblBalanceAmount });
        overviewCard.Layout += (_, _) =>
        {
            if (overviewCard == null || _lblBalanceAmount == null) return;
            currentBalanceLbl.Left = overviewCard.ClientSize.Width - 24 - currentBalanceLbl.Width;
            _lblBalanceAmount.Left = overviewCard.ClientSize.Width - 24 - _lblBalanceAmount.Width;
        };
        flow.Controls.Add(overviewCard);

        // Two columns: Account Information + Quick Actions
        var twoCols = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, WrapContents = false, AutoSize = true, Margin = new Padding(0, 0, 0, Theme.PadMedium) };
        int halfWidth = (800 - Theme.PadMedium) / 2;

        _infoCard = new Panel
        {
            BackColor = Theme.Surface,
            BorderStyle = BorderStyle.FixedSingle,
            Size = new Size(halfWidth, 200),
            Margin = new Padding(0, 0, Theme.PadMedium, 0),
            Padding = new Padding(Theme.PadXLarge)
        };
        var infoCard = _infoCard;
        var infoTitle = new Label { Text = "Account Information", Font = Theme.SectionFont, ForeColor = Theme.TextPrimary, Location = new Point(Theme.PadXLarge, Theme.PadMedium), AutoSize = true };
        int ry = 52;
        infoCard.Controls.Add(infoTitle);
        infoCard.Controls.Add(new Label { Text = "Account Number", Font = Theme.BodyFont, ForeColor = Theme.TextSecondary, Location = new Point(Theme.PadXLarge, ry), AutoSize = true });
        _lblInfoNumber = new Label { Text = "", Font = Theme.BodyFont, ForeColor = Theme.TextPrimary, Location = new Point(halfWidth - 24 - 120, ry), AutoSize = true };
        infoCard.Controls.Add(_lblInfoNumber);
        ry += 28;
        infoCard.Controls.Add(new Label { Text = "Account Type", Font = Theme.BodyFont, ForeColor = Theme.TextSecondary, Location = new Point(Theme.PadXLarge, ry), AutoSize = true });
        _lblInfoType = new Label { Text = "", Font = Theme.BodyFont, ForeColor = Theme.TextPrimary, Location = new Point(halfWidth - 24 - 120, ry), AutoSize = true };
        infoCard.Controls.Add(_lblInfoType);
        ry += 28;
        infoCard.Controls.Add(new Label { Text = "Currency", Font = Theme.BodyFont, ForeColor = Theme.TextSecondary, Location = new Point(Theme.PadXLarge, ry), AutoSize = true });
        _lblInfoCurrency = new Label { Text = "", Font = Theme.BodyFont, ForeColor = Theme.TextPrimary, Location = new Point(halfWidth - 24 - 120, ry), AutoSize = true };
        infoCard.Controls.Add(_lblInfoCurrency);
        ry += 28;
        infoCard.Controls.Add(new Label { Text = "Account Name", Font = Theme.BodyFont, ForeColor = Theme.TextSecondary, Location = new Point(Theme.PadXLarge, ry), AutoSize = true });
        _lblInfoName = new Label { Text = "", Font = Theme.BodyFont, ForeColor = Theme.TextPrimary, Location = new Point(halfWidth - 24 - 120, ry), AutoSize = true };
        infoCard.Controls.Add(_lblInfoName);
        twoCols.Controls.Add(infoCard);
        _infoCard.Layout += (_, _) =>
        {
            if (_infoCard == null) return;
            int rightX = _infoCard.ClientSize.Width - Theme.PadXLarge - 8;
            if (_lblInfoNumber != null) _lblInfoNumber.Left = rightX - _lblInfoNumber.PreferredWidth;
            if (_lblInfoType != null) _lblInfoType.Left = rightX - _lblInfoType.PreferredWidth;
            if (_lblInfoCurrency != null) _lblInfoCurrency.Left = rightX - _lblInfoCurrency.PreferredWidth;
            if (_lblInfoName != null) _lblInfoName.Left = rightX - _lblInfoName.PreferredWidth;
        };

        _quickCard = new Panel
        {
            BackColor = Theme.Surface,
            BorderStyle = BorderStyle.FixedSingle,
            Size = new Size(halfWidth, 200),
            Padding = new Padding(Theme.PadXLarge)
        };
        var quickCard = _quickCard;
        var quickTitle = new Label { Text = "Quick Actions", Font = Theme.SectionFont, ForeColor = Theme.TextPrimary, Location = new Point(Theme.PadXLarge, Theme.PadMedium), AutoSize = true };
        var btnTransfer = new Button
        {
            Text = "💸\r\nTransfer Money",
            Size = new Size(160, 100),
            BackColor = Theme.Surface,
            FlatStyle = FlatStyle.Flat,
            Font = Theme.BodyFont,
            ForeColor = Theme.TextPrimary,
            Location = new Point(Theme.PadXLarge, 52),
            TextAlign = ContentAlignment.MiddleCenter
        };
        var btnTrans = new Button
        {
            Text = "📋\r\nView All Transactions",
            Size = new Size(160, 100),
            BackColor = Theme.Surface,
            FlatStyle = FlatStyle.Flat,
            Font = Theme.BodyFont,
            ForeColor = Theme.TextPrimary,
            Location = new Point(Theme.PadXLarge + 170, 52),
            TextAlign = ContentAlignment.MiddleCenter
        };
        btnTransfer.Click += (_, _) => _navigate("Transfer", null);
        btnTrans.Click += (_, _) => _navigate("Transactions", null);
        quickCard.Controls.AddRange(new Control[] { quickTitle, btnTransfer, btnTrans });
        twoCols.Controls.Add(quickCard);

        flow.Controls.Add(twoCols);

        // Recent Transactions card
        _transHeader = new TableLayoutPanel { RowCount = 1, ColumnCount = 2, Size = new Size(800, 36), Margin = new Padding(0, 0, 0, 0) };
        _transHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _transHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        _transHeader.Controls.Add(new Label { Text = "Recent Transactions", Font = Theme.SectionFont, ForeColor = Theme.TextPrimary, AutoSize = true }, 0, 0);
        var viewAllLink = new LinkLabel { Text = "View All →", AutoSize = true, ForeColor = Theme.Primary, Font = Theme.BodyFont };
        viewAllLink.Click += (_, _) => _navigate("Transactions", null);
        _transHeader.Controls.Add(viewAllLink, 1, 0);
        _transPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true, Margin = new Padding(0) };
        _transCard = new Panel
        {
            BackColor = Theme.Surface,
            BorderStyle = BorderStyle.FixedSingle,
            AutoSize = true,
            Padding = new Padding(Theme.PadXLarge),
            Margin = new Padding(0)
        };
        _transCard.Controls.Add(_transHeader);
        _transCard.Controls.Add(_transPanel);
        _transCard.Layout += (_, _) =>
        {
            if (_transCard == null || _transHeader == null || _transPanel == null) return;
            _transHeader.Location = new Point(0, 0);
            _transHeader.Width = _transCard.ClientSize.Width;
            int gap = Theme.PadMedium;
            _transPanel.Location = new Point(0, _transHeader.Height + gap);
            _transPanel.Width = _transCard.ClientSize.Width;
        };
        flow.Controls.Add(_transCard);

        _contentWrap.Controls.Add(flow);
        flow.Location = new Point(0, 0);
        scroll.Controls.Add(_contentWrap);
        Controls.Add(scroll);

        void OnContentResize()
        {
            if (_contentWrap == null) return;
            int clientWidth = _contentWrap.ClientSize.Width;
            int horizontalPad = clientWidth < 900 ? Theme.PadLarge : 100;
            if (_contentWrap.Padding.Left != horizontalPad || _contentWrap.Padding.Right != horizontalPad)
            {
                _contentWrap.Padding = new Padding(horizontalPad, _contentWrap.Padding.Top, horizontalPad, _contentWrap.Padding.Bottom);
            }
            int availableWidth = clientWidth - _contentWrap.Padding.Horizontal;
            if (availableWidth <= 0) return;
            _contentWidth = availableWidth;
            int halfW = (availableWidth - Theme.PadMedium) / 2;
            if (_overviewCard != null) _overviewCard.Width = availableWidth;
            if (_infoCard != null) _infoCard.Width = halfW;
            if (_quickCard != null) _quickCard.Width = halfW;
            if (_transCard != null) _transCard.Width = availableWidth;
            if (_transHeader != null) _transHeader.Width = Math.Max(200, availableWidth - Theme.PadXLarge * 2);
            _contentWrap.PerformLayout();
        }
        _contentWrap.Resize += (_, _) => OnContentResize();
        Load += (_, _) => OnContentResize();
    }

    public void RefreshData()
    {
        var acc = AccountService.Instance.GetAccountById(_accountId);
        if (acc == null) return;

        var typeDisplay = acc.AccountType.Length > 0
            ? char.ToUpperInvariant(acc.AccountType[0]) + acc.AccountType[1..].ToLowerInvariant()
            : acc.AccountType;
        var typePillText = typeDisplay + " Account";

        if (_lblOverviewName != null) _lblOverviewName.Text = acc.AccountName;
        if (_lblOverviewNumber != null) _lblOverviewNumber.Text = acc.AccountNumber;
        if (_lblTypePill != null) _lblTypePill.Text = typePillText;
        if (_lblBalanceAmount != null)
        {
            _lblBalanceAmount.Text = acc.Balance.ToString("C2");
            _lblBalanceAmount.ForeColor = acc.Balance < 0 ? Theme.Danger : Theme.TextPrimary;
        }
        if (_lblInfoNumber != null) _lblInfoNumber.Text = acc.AccountNumber;
        if (_lblInfoType != null) _lblInfoType.Text = typeDisplay;
        if (_lblInfoCurrency != null) _lblInfoCurrency.Text = acc.Currency;
        if (_lblInfoName != null) _lblInfoName.Text = acc.AccountName;
        if (_overviewIcon != null) _overviewIcon.Text = acc.AccountType.ToLowerInvariant() == "savings" ? "💰" : "💳";

        var transactions = TransactionService.Instance.GetTransactionsByAccount(_accountId).Take(10).ToList();
        _transPanel!.Controls.Clear();
        if (transactions.Count == 0)
            _transPanel.Controls.Add(new Label { Text = "No transactions found for this account.", ForeColor = Theme.TextSecondary, Font = Theme.BodyFont });
        else
        {
            int rowWidth = Math.Max(600, _contentWidth - Theme.PadXLarge * 2);
            foreach (var t in transactions)
            {
                var icon = t.Type == "debit" ? "↓" : t.Type == "credit" ? "↑" : "⇄";
                var iconBg = t.Type == "debit" ? Theme.IconDebitBg : t.Type == "credit" ? Theme.IconCreditBg : Theme.IconTransferBg;
                var amountColor = t.Type == "debit" ? Theme.Danger : Theme.Secondary;
                var sign = t.Type == "debit" ? "-" : "+";
                var to = string.IsNullOrEmpty(t.RecipientAccount) ? "" : " · To: " + (AccountService.Instance.GetAccountById(t.RecipientAccount)?.AccountName ?? "Unknown");
                var row = new Panel { Size = new Size(rowWidth, 80), BackColor = Theme.Surface, BorderStyle = BorderStyle.FixedSingle, Margin = new Padding(0, 0, 0, Theme.PadSmall) };
                var lblIcon = new Label { Text = icon, Size = new Size(48, 48), Location = new Point(Theme.PadSmall, 16), BackColor = iconBg, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 14), ForeColor = amountColor };
                var lblDesc = new Label { Text = $"{t.Description}\r\n{t.Date:MMM d, y h:mm tt}{to}", Location = new Point(68, 14), MaximumSize = new Size(rowWidth - 200, 0), AutoSize = true, Font = Theme.BodyFont, ForeColor = Theme.TextPrimary };
                var lblAmt = new Label { Text = $"{sign}{t.Amount:C2}", Location = new Point(rowWidth - 100, 28), AutoSize = true, Font = Theme.SectionFont, ForeColor = amountColor };
                row.Controls.AddRange(new Control[] { lblIcon, lblDesc, lblAmt });
                _transPanel.Controls.Add(row);
            }
        }
    }
}
