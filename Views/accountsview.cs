using BankingApp.WinForms.Models;
using BankingApp.WinForms.Services;

namespace BankingApp.WinForms.Views;

public class AccountsView : UserControl
{
    private readonly Action<string, string?> _navigate;
    private Panel? _contentWrap;
    private Panel? _titleWrap;
    private FlowLayoutPanel? _cardsFlow;
    private Panel? _cardsWrapper;
    private Panel? _totalBar;
    private Label? _lblTotalAmount;

    public AccountsView(Action<string, string?> navigate)
    {
        _navigate = navigate;
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
            Dock = DockStyle.Top
        };

        _titleWrap = new Panel { Height = 70, Margin = new Padding(0, 0, 0, Theme.PadMedium) };
        var titleLbl = new Label { Text = "My Accounts", Font = Theme.TitleFont, ForeColor = Theme.TextPrimary, AutoSize = true };
        var subtitleLbl = new Label { Text = "Manage and view all your accounts", Font = Theme.BodyFont, ForeColor = Theme.TextSecondary, AutoSize = true };
        _titleWrap.Controls.Add(titleLbl);
        _titleWrap.Controls.Add(subtitleLbl);
        _titleWrap.Layout += (_, _) =>
        {
            if (_titleWrap == null) return;
            titleLbl.Location = new Point(Math.Max(0, (_titleWrap.ClientSize.Width - titleLbl.Width) / 2), 0);
            subtitleLbl.Location = new Point(Math.Max(0, (_titleWrap.ClientSize.Width - subtitleLbl.Width) / 2), titleLbl.Height + Theme.PadSmall);
        };
        flow.Controls.Add(_titleWrap);

        _cardsWrapper = new Panel { Margin = new Padding(0, 0, 0, Theme.PadXLarge) };
        _cardsFlow = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            AutoSize = true,
            BackColor = Theme.Background,
            Padding = Padding.Empty,
            Margin = Padding.Empty
        };
        _totalBar = new Panel { Height = 112, BackColor = Theme.Primary, Margin = new Padding(0, Theme.PadMedium, 0, 0) };
        var totalTitleLbl = new Label { Text = "Total Balance", Font = Theme.BodyFont, ForeColor = Color.White, AutoSize = true };
        _lblTotalAmount = new Label { Text = "$0.00", Font = new Font("Segoe UI", 22, FontStyle.Bold), ForeColor = Color.White, AutoSize = true };
        _totalBar.Controls.Add(totalTitleLbl);
        _totalBar.Controls.Add(_lblTotalAmount);
        _totalBar.Layout += (_, _) =>
        {
            if (_totalBar == null || _lblTotalAmount == null) return;
            int gap = 8;
            int blockHeight = totalTitleLbl.Height + gap + _lblTotalAmount.Height;
            int yStart = Math.Max(0, (_totalBar.ClientSize.Height - blockHeight) / 2);
            totalTitleLbl.Location = new Point(Math.Max(0, (_totalBar.ClientSize.Width - totalTitleLbl.Width) / 2), yStart);
            _lblTotalAmount.Location = new Point(Math.Max(0, (_totalBar.ClientSize.Width - _lblTotalAmount.Width) / 2), yStart + totalTitleLbl.Height + gap);
        };
        _cardsWrapper.Controls.Add(_cardsFlow);
        _cardsWrapper.Controls.Add(_totalBar);
        _cardsWrapper.Layout += (_, _) =>
        {
            if (_cardsWrapper == null || _cardsFlow == null || _totalBar == null) return;
            int x = Math.Max(0, (_cardsWrapper.ClientSize.Width - _cardsFlow.Width) / 2);
            _cardsFlow.Location = new Point(x, 0);
            _totalBar.Location = new Point(x, _cardsFlow.Height + Theme.PadMedium);
            _totalBar.Width = _cardsFlow.Width;
        };
        flow.Controls.Add(_cardsWrapper);

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
            if (_titleWrap != null) _titleWrap.Width = availableWidth;
            if (_cardsWrapper != null) _cardsWrapper.Width = availableWidth;
            _contentWrap.PerformLayout();
        }
        _contentWrap.Resize += (_, _) => OnContentResize();
        Load += (_, _) => OnContentResize();
    }

    public void RefreshData()
    {
        var accountService = AccountService.Instance;
        var accounts = accountService.GetAccounts();

        if (_lblTotalAmount != null)
            _lblTotalAmount.Text = accountService.GetTotalBalance().ToString("C2");

        if (_cardsFlow == null) return;
        _cardsFlow.Controls.Clear();

        const int cardWidth = 300;
        const int cardHeight = 280;
        const int pad = Theme.PadMedium;
        int yHeader = pad;
        int iconHeight = 32;
        int lineHeight = 20;
        int nameBlockHeight = 44;

        foreach (var acc in accounts)
        {
            var icon = acc.AccountType.ToLowerInvariant() == "savings" ? "💰" : "💳";
            var typeDisplay = acc.AccountType.Length > 0
                ? char.ToUpperInvariant(acc.AccountType[0]) + acc.AccountType[1..].ToLowerInvariant()
                : acc.AccountType;

            var card = new Panel
            {
                Size = new Size(cardWidth, cardHeight),
                BackColor = Theme.Surface,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(pad),
                Margin = new Padding(0, 0, Theme.PadMedium, Theme.PadMedium),
                Cursor = Cursors.Hand,
                Tag = acc.Id
            };

            var iconLbl = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 18),
                Location = new Point(pad, yHeader),
                AutoSize = true
            };
            int nameY = yHeader + iconHeight;
            var nameLbl = new Label
            {
                Text = acc.AccountName,
                Font = Theme.SectionFont,
                ForeColor = Theme.TextPrimary,
                Location = new Point(pad, nameY),
                AutoSize = true,
                MaximumSize = new Size(cardWidth - pad * 2, nameBlockHeight)
            };
            int numberY = nameY + nameBlockHeight;
            var numberLbl = new Label
            {
                Text = acc.AccountNumber,
                Font = Theme.BodyFont,
                ForeColor = Theme.TextSecondary,
                Location = new Point(pad, numberY),
                AutoSize = true
            };
            int detailsY = numberY + lineHeight + Theme.PadSmall;
            var typeLbl = new Label { Text = "Type", Font = Theme.BodyFont, ForeColor = Theme.TextSecondary, Location = new Point(pad, detailsY), AutoSize = true };
            var typeValLbl = new Label { Text = typeDisplay, Font = Theme.BodyFont, ForeColor = Theme.TextPrimary, Location = new Point(cardWidth - pad - 80, detailsY), AutoSize = true };
            var currLbl = new Label { Text = "Currency", Font = Theme.BodyFont, ForeColor = Theme.TextSecondary, Location = new Point(pad, detailsY + 20), AutoSize = true };
            var currValLbl = new Label { Text = acc.Currency, Font = Theme.BodyFont, ForeColor = Theme.TextPrimary, Location = new Point(cardWidth - pad - 80, detailsY + 20), AutoSize = true };
            var balLbl = new Label { Text = "Balance", Font = Theme.BodyFont, ForeColor = Theme.TextSecondary, Location = new Point(pad, detailsY + 56), AutoSize = true };
            var balValLbl = new Label
            {
                Text = acc.Balance.ToString("C2"),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = acc.Balance < 0 ? Theme.Danger : Theme.TextPrimary,
                Location = new Point(pad, detailsY + 76),
                AutoSize = true
            };

            card.Controls.AddRange(new Control[]
            {
                iconLbl, nameLbl, numberLbl,
                typeLbl, typeValLbl, currLbl, currValLbl,
                balLbl, balValLbl
            });
            balValLbl.Left = cardWidth - pad - balValLbl.PreferredWidth;

            card.Click += (s, _) => _navigate("AccountDetails", (string)((Panel)s!).Tag!);
            foreach (Control c in card.Controls)
            {
                c.Cursor = Cursors.Hand;
                c.Click += (s, _) => _navigate("AccountDetails", (string)((Control)s!).Parent!.Tag!);
            }

            _cardsFlow.Controls.Add(card);
        }

        if (_cardsFlow != null && _cardsWrapper != null && _totalBar != null)
        {
            _cardsFlow.PerformLayout();
            _cardsWrapper.Height = _cardsFlow.Height + Theme.PadMedium + _totalBar.Height;
        }
    }
}
