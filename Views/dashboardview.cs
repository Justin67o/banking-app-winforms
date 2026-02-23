using BankingApp.WinForms.Models;
using BankingApp.WinForms.Services;

namespace BankingApp.WinForms.Views;

public class DashboardView : UserControl
{
    private readonly Action<string, string?> _navigate;
    private Panel? _contentWrap;
    private FlowLayoutPanel? _accountsFlow;
    private FlowLayoutPanel? _transFlow;
    private Label? _lblTotalBalance;
    private Label? _lblAccountCount;
    private Label? _lblRecentCount;    
    private Panel? _scrollPanel;
    private Panel? _leftBox;
    private Panel? _rightBox;
    private int _boxWidth;
    private Panel? _buttonRow;
    private TableLayoutPanel? _cardsFlow;
    private TableLayoutPanel? _headerAcc;
    private TableLayoutPanel? _headerTrans;
    private Panel? _titleWrap;

    public DashboardView(Action<string, string?> navigate)
    {
        _navigate = navigate;
        BackColor = Theme.Background;
        BuildUi();
    }

    private void BuildUi()
    {
        _scrollPanel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Theme.Background };
        var scroll = _scrollPanel;
        _contentWrap = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Background, Padding = new Padding(100, Theme.PadMedium, 100, Theme.PadMedium) };
        var flow = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoSize = true, BackColor = Theme.Background, Dock = DockStyle.Top };

        _titleWrap = new Panel { Height = 70, Margin = new Padding(0, 0, 0, Theme.PadMedium) };
        var titleLbl = new Label { Text = "Dashboard", Font = Theme.TitleFont, ForeColor = Theme.TextPrimary, AutoSize = true };
        var subtitleLbl = new Label { Text = "Welcome back! Here's your financial overview.", Font = Theme.BodyFont, ForeColor = Theme.TextSecondary, AutoSize = true };
        _titleWrap.Controls.Add(titleLbl);
        _titleWrap.Controls.Add(subtitleLbl);
        _titleWrap.Layout += (_, _) =>
        {
            titleLbl.Location = new Point(Math.Max(0, (_titleWrap!.ClientSize.Width - titleLbl.Width) / 2), 0);
            subtitleLbl.Location = new Point(Math.Max(0, (_titleWrap.ClientSize.Width - subtitleLbl.Width) / 2), titleLbl.Height + Theme.PadSmall);
        };
        flow.Controls.Add(_titleWrap);
        
        const int headerRowWidth = 360 + Theme.PadMedium + 360 + Theme.PadMedium + 360; // Total Balance + Accounts + Recent
        _boxWidth = (headerRowWidth - Theme.PadMedium) / 2;
        int boxContentWidth = _boxWidth - Theme.PadMedium * 2;
        const int minBoxHeight = 140;

        _cardsFlow = new TableLayoutPanel { ColumnCount = 3, RowCount = 1, Dock = DockStyle.Top, Height = 84 };
        _cardsFlow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        _cardsFlow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        _cardsFlow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
        var cardsFlow = _cardsFlow;
        _lblTotalBalance = new Label
        {
            Text = "💰 Total Balance: $0.00",
            AutoSize = false,
            Dock = DockStyle.Fill,
            BackColor = Theme.Surface,
            BorderStyle = BorderStyle.FixedSingle,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = Theme.SectionFont,
            Padding = new Padding(Theme.PadMedium),
            ForeColor = Theme.TextPrimary
        };
        _lblAccountCount = new Label
        {
            Text = "🏦 Accounts: 0",
            AutoSize = false,
            Dock = DockStyle.Fill,
            BackColor = Theme.Surface,
            BorderStyle = BorderStyle.FixedSingle,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = Theme.SectionFont,
            Margin = Padding.Empty
        };
        _lblRecentCount = new Label
        {
            Text = "📊 Recent: 0",
            AutoSize = false,
            Dock = DockStyle.Fill,
            BackColor = Theme.Surface,
            BorderStyle = BorderStyle.FixedSingle,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = Theme.SectionFont,
            Margin = Padding.Empty
        };
        cardsFlow.Controls.Add(_lblTotalBalance, 0, 0);
        cardsFlow.Controls.Add(_lblAccountCount, 1, 0);
        cardsFlow.Controls.Add(_lblRecentCount, 2, 0);
        flow.Controls.Add(cardsFlow);

        var twoCols = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true, WrapContents = false, Margin = new Padding(0, Theme.PadXLarge, 0, 0) };

        var leftBox = new Panel
        {
            BackColor = Theme.Surface,
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(Theme.PadMedium),
            MinimumSize = new Size(_boxWidth, minBoxHeight),
            Size = new Size(_boxWidth, minBoxHeight),
            Margin = new Padding(0, 0, Theme.PadMedium, 0)
        };
        _leftBox = leftBox;
        var leftCol = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, AutoSize = true };
        _headerAcc = new TableLayoutPanel { RowCount = 1, ColumnCount = 2, Size = new Size(boxContentWidth, 28), Margin = new Padding(0, 0, 0, Theme.PadSmall) };
        _headerAcc.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _headerAcc.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        _headerAcc.Controls.Add(new Label { Text = "Your Accounts", Font = Theme.SectionFont, ForeColor = Theme.TextPrimary, AutoSize = true, TextAlign = ContentAlignment.MiddleCenter }, 0, 0);
        var viewAllAcc = new LinkLabel { Text = "View All →", AutoSize = true, ForeColor = Theme.Primary, Font = Theme.BodyFont, Anchor = AnchorStyles.Right };
        viewAllAcc.Click += (_, _) => _navigate("Accounts", null);
        _headerAcc.Controls.Add(viewAllAcc, 1, 0);
        leftCol.Controls.Add(_headerAcc);
        _accountsFlow = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true };
        leftCol.Controls.Add(_accountsFlow);
        leftBox.Controls.Add(leftCol);

        var rightBox = new Panel
        {
            BackColor = Theme.Surface,
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(Theme.PadMedium),
            MinimumSize = new Size(_boxWidth, minBoxHeight),
            Size = new Size(_boxWidth, minBoxHeight),
            Margin = new Padding(0, 0, 0, 0)
        };
        _rightBox = rightBox;
        var rightCol = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            Padding = Padding.Empty,
            Margin = Padding.Empty
        };
        rightCol.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rightCol.RowStyles.Add(new RowStyle(SizeType.Absolute, 28 + Theme.PadSmall));
        rightCol.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _headerTrans = new TableLayoutPanel { RowCount = 1, ColumnCount = 2, Size = new Size(boxContentWidth, 28), Margin = new Padding(0, 0, 0, Theme.PadSmall) };
        _headerTrans.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _headerTrans.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        _headerTrans.Controls.Add(new Label { Text = "Recent Transactions", Font = Theme.SectionFont, ForeColor = Theme.TextPrimary, AutoSize = true, TextAlign = ContentAlignment.MiddleCenter }, 0, 0);
        var viewAllTrans = new LinkLabel { Text = "View All →", AutoSize = true, ForeColor = Theme.Primary, Font = Theme.BodyFont, Anchor = AnchorStyles.Right };
        viewAllTrans.Click += (_, _) => _navigate("Transactions", null);
        _headerTrans.Controls.Add(viewAllTrans, 1, 0);
        rightCol.Controls.Add(_headerTrans, 0, 0);
        _transFlow = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true, MinimumSize = new Size(boxContentWidth, 0), Dock = DockStyle.Top };
        rightCol.Controls.Add(_transFlow, 0, 1);
        rightBox.Controls.Add(rightCol);

        twoCols.Controls.Add(leftBox);
        twoCols.Controls.Add(rightBox);
        flow.Controls.Add(twoCols);

        // Transfer Money and View Transactions right underneath the two panels
        int buttonRowWidth = 2 * _boxWidth + Theme.PadMedium;
        var buttonRow = new Panel { Size = new Size(buttonRowWidth, 60), MinimumSize = new Size(0, 60), Margin = new Padding(0, Theme.PadMedium, 0, 0) };
        var quickFlow = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.LeftToRight,
            AutoSize = true,
            WrapContents = false,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        var btnTransfer = new Button { Text = "💸 Transfer Money", Size = new Size(190, 40), BackColor = Theme.Primary, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = Theme.BodyFont, Margin = new Padding(0, 0, Theme.PadMedium, 0) };
        var btnViewTrans = new Button { Text = "📋 View Transactions", Size = new Size(190, 40), BackColor = Theme.Primary, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = Theme.BodyFont, Margin = new Padding(0) };
        btnTransfer.Click += (_, _) => _navigate("Transfer", null);
        btnViewTrans.Click += (_, _) => _navigate("Transactions", null);
        quickFlow.Controls.Add(btnTransfer);
        quickFlow.Controls.Add(btnViewTrans);
        buttonRow.Controls.Add(quickFlow);
        buttonRow.Layout += (_, _) =>
        {
            quickFlow.PerformLayout();
            int x = Math.Max(0, (buttonRow.ClientSize.Width - quickFlow.Width) / 2);
            int y = Math.Max(0, (buttonRow.ClientSize.Height - quickFlow.Height) / 2);
            quickFlow.Location = new Point(x, y);
        };
        flow.Controls.Add(buttonRow);
        _buttonRow = buttonRow;

        _contentWrap.Controls.Add(flow);
        flow.Location = new Point(0, 0);
        scroll.Controls.Add(_contentWrap);
        Controls.Add(scroll);
        void OnContentResize()
        {
            if (_contentWrap == null || _scrollPanel == null) return;
            int availableWidth = _contentWrap.ClientSize.Width - _contentWrap.Padding.Horizontal;
            if (availableWidth <= 0) return;
            _boxWidth = (availableWidth - Theme.PadMedium) / 2;
            int boxContentWidth = _boxWidth - Theme.PadMedium * 2;
            if (_leftBox != null) _leftBox.Width = _boxWidth;
            if (_rightBox != null) _rightBox.Width = _boxWidth;
            if (_headerAcc != null) _headerAcc.Width = boxContentWidth;
            if (_headerTrans != null) _headerTrans.Width = boxContentWidth;
            if (_buttonRow != null) _buttonRow.Width = availableWidth;
            if (_cardsFlow != null) _cardsFlow.Width = availableWidth;
            if (_titleWrap != null) _titleWrap.Width = availableWidth;
            _contentWrap.PerformLayout();
        }
        _contentWrap.Resize += (_, _) => OnContentResize();
        Load += (_, _) => OnContentResize();
    }

    public void RefreshData()
    {
        var accountService = AccountService.Instance;
        var transactionService = TransactionService.Instance;
        var accounts = accountService.GetAccounts();
        var transactions = transactionService.GetTransactions().Take(5).ToList();

        if (_lblTotalBalance != null) _lblTotalBalance.Text = $"💰 Total Balance: {accountService.GetTotalBalance():C2}";
        if (_lblAccountCount != null) _lblAccountCount.Text = $"🏦 Accounts: {accounts.Count}";
        if (_lblRecentCount != null) _lblRecentCount.Text = $"📊 Recent: {transactions.Count}";

        if (_accountsFlow != null)
        {
            _accountsFlow.Controls.Clear();
            int contentWidth = _boxWidth - Theme.PadMedium * 2;
            foreach (var acc in accounts)
            {
                var card = new Button
                {
                    Text = $"{acc.AccountName} ({acc.AccountNumber})\r\n{acc.Balance:C2}",
                    Size = new Size(contentWidth, 64),
                    Tag = acc.Id,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Theme.Surface,
                    ForeColor = acc.Balance < 0 ? Theme.Danger : Theme.TextPrimary,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = Theme.BodyFont
                };
                card.Click += (s, _) => _navigate("AccountDetails", (string)((Button)s!).Tag!);
                var wrap = new Panel { Size = new Size(contentWidth, 68), Margin = new Padding(0, 0, 0, Theme.PadSmall) };
                wrap.Controls.Add(card);
                card.Location = new Point(0, 2);
                _accountsFlow.Controls.Add(wrap);
            }
        }

        if (_transFlow != null)
        {
            _transFlow.Controls.Clear();
            int contentWidth = _boxWidth - Theme.PadMedium * 2;
            foreach (var t in transactions)
            {
                var icon = t.Type == "debit" ? "↓" : t.Type == "credit" ? "↑" : "⇄";
                var sign = t.Type == "debit" ? "-" : "+";
                var color = t.Type == "debit" ? Theme.Danger : t.Type == "credit" ? Theme.Secondary : Theme.Primary;
                var row = new Panel { Size = new Size(contentWidth, 52), BackColor = Theme.Surface, BorderStyle = BorderStyle.FixedSingle };
                var lblIcon = new Label { Text = icon, Size = new Size(40, 40), Location = new Point(Theme.PadSmall, 2), BackColor = Theme.Background, TextAlign = ContentAlignment.MiddleCenter, Font = Theme.BodyFont, ForeColor = color };
                var lblText = new Label { Text = $"{t.Description} | {t.Date:MMM d, y} | {sign}{t.Amount:C2}", Location = new Point(56, 10), AutoSize = true, Font = Theme.BodyFont, ForeColor = Theme.TextPrimary, MaximumSize = new Size(contentWidth - 60, 0) };
                row.Controls.Add(lblIcon);
                row.Controls.Add(lblText);
                var wrap = new Panel { Size = new Size(contentWidth, 56), Margin = new Padding(0, 0, 0, Theme.PadSmall) };
                wrap.Controls.Add(row);
                row.Location = new Point(0, 2);
                _transFlow.Controls.Add(wrap);
            }
            _transFlow.PerformLayout();
            _transFlow.Refresh();
        }

        // Expand both boxes to fit content and keep them the same height
        if (_leftBox != null && _rightBox != null)
        {
            const int headerHeight = 28 + Theme.PadSmall;
            const int accountRowHeight = 68 + Theme.PadSmall;
            const int transRowHeight = 56 + Theme.PadSmall;
            int leftContentHeight = headerHeight + (_accountsFlow?.Controls.Count ?? 0) * accountRowHeight;
            int rightContentHeight = headerHeight + (_transFlow?.Controls.Count ?? 0) * transRowHeight;
            // Add small buffer so last row isn't clipped by layout
            int sharedContentHeight = Math.Max(leftContentHeight, rightContentHeight) + Theme.PadSmall;
            int boxHeight = Theme.PadMedium * 2 + sharedContentHeight;
            boxHeight = Math.Max(boxHeight, 140);
            var size = new Size(_boxWidth, boxHeight);
            _leftBox.Size = size;
            _rightBox.Size = size;
            _leftBox.PerformLayout();
            _rightBox.PerformLayout();
        }
    }
}
